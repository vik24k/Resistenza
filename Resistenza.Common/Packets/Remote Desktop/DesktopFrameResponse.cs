using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resistenza.Common.Packets.Remote_Desktop
{
    public class DesktopFrameResponse
    {

        public string Type { get; set; }
        public byte[] ScreenCapture { get; set; }
        public int CursorLocationX { get; set; }
        public int CursorLocationY { get; set; }


        public DesktopFrameResponse()
        {
            Type = this.GetType().ToString();
        }
    }
}
