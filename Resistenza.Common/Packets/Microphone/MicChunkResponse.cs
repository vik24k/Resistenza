using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resistenza.Common.Packets.Microphone
{
    public class MicChunkResponse
    {

        public string Type { get; set; }
        public byte[] Data { get; set; }


        public MicChunkResponse() {
            Type = this.GetType().ToString();



        }
    }
}
