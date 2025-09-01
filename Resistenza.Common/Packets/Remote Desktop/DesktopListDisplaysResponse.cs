using Resistenza.Common.Packets.Microphone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resistenza.Common.Packets.Remote_Desktop
{
    public class DesktopListDisplaysResponse
    {
        public string Type { get; set; }
        public List<MonitorDeviceInfo> Devices { get; set; }

        public DesktopListDisplaysResponse()
        {

            Type = this.GetType().ToString();
            Devices = new List<MonitorDeviceInfo>();
        }
    }
}
