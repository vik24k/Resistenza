using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resistenza.Common.Packets.Camera;

    public class CamFrameResponse
    {

        public string Type { get; set; }
        public byte[] Data { get; set; }
        //public int Heigth {  get; set; }
        //public int Width { get; set; }


        public CamFrameResponse()
        {
            Type = this.GetType().ToString();

        }
    }

