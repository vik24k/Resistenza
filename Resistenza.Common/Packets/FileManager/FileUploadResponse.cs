using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resistenza.Common.Packets
{
    public class FileUploadResponse : IPacket
    {
        public string Type { get; set; }
        public string Error { get; set; }
        public bool IsLastOfRequest { get; set; }

        public FileUploadResponse()
        {
            Type = this.GetType().ToString();

        }
    }
}
