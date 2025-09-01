using Resistenza.Client.Logic;
using Resistenza.Common.Networking;
using Resistenza.Common.Packets.Microphone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resistenza.Common.Packets.Remote_Desktop
{
    public class DesktopStartRequest
    {
        public string Type { get; set; }
        public bool ListDevices { get; set; }
        public bool StartTransmit { get; set; }
        public string NameTargetDisplay { get; set; }



        public DesktopStartRequest()
        {
            Type = this.GetType().ToString();
        }

        public async Task HandleAsync(SecureStream ServerStream, CancellationTokenSourceWithId CancelOperation, SemaphoreSlim Lock)
        {
            if (ListDevices)
            {
                List<MonitorDeviceInfo> Devices = DesktopBroadcast.GetDevicesInfo();

                var DevicesPacket = new DesktopListDisplaysResponse();
                DevicesPacket.Devices = Devices;

                await Lock.WaitAsync();
                bool Res = await ServerStream.SendPacketAsync(DevicesPacket);
                Lock.Release();

                return;
            }

            if (StartTransmit)
            {
                DesktopBroadcast Streamer = new DesktopBroadcast(ServerStream, Lock);
                try
                {

                    await Streamer.StartStreaming(NameTargetDisplay, CancelOperation);
                    
                }
                catch (OperationCanceledException)
                {
                    CancelOperation.Token.ThrowIfCancellationRequested();
                }


            }


        }
    }
}
