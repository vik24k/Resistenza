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
              
                var DevicesPacket = new DesktopListDisplaysResponse();
                DevicesPacket.Devices = DesktopBroadcast.GetDevicesInfo();
                await ServerStream.SendPacketAsync(DevicesPacket); //ricevuto


                return;
            }

            if (StartTransmit)
            {

                
                try
                {
                    //provo a reinviare lo stesso pacchetto per vedere se il server lo riceve
                    var DevicesPacket = new DesktopListDisplaysResponse();
                    DevicesPacket.Devices = DesktopBroadcast.GetDevicesInfo();
                    await ServerStream.SendPacketAsync(DevicesPacket);


                    Console.WriteLine("[DEBUG] Creating DesktopBroadcast");
                    DesktopBroadcast Streamer = new DesktopBroadcast(ServerStream);
                    Console.WriteLine("[DEBUG] DesktopBroadcast created");

                    await Streamer.StartStreaming(NameTargetDisplay, CancelOperation);

                    //await Streamer.StartStreaming(NameTargetDisplay, CancelOperation);

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
