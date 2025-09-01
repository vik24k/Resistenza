using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resistenza.Common.Packets
{
    public class MountedPartitionsResponse : IPacket
    {

        public string Type { get; set; }
        public string Error { get; set; }
        public List<DriveInformation> RootAndType { get; set; }

        public MountedPartitionsResponse()
        {
            Type = this.GetType().ToString();
            RootAndType = new List<DriveInformation>();
        }


    }

    public class DriveInformation
    {
        public string RootDirectory { get; set; }
        public DriveType DriveType { get; set; }
    }
}
