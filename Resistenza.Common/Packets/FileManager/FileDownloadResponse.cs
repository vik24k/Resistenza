using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resistenza.Common.Packets
{
    public class FileDownloadResponse : IPacket
    {

        public byte[] FileBytes { get; set; }

        public string FileName { get; set; }
        
        public bool IsLastOfRequest { get; set; }

        public bool IsPart { get; set; } //files troppo grandi vengono divisi in chunk più piccoli per non caricare tutto in memoria
  
        public string Type { get; set; }
        public string Error { get; set; }


        public FileDownloadResponse() {

            Type = this.GetType().ToString();
           

        }
    }

 

}
