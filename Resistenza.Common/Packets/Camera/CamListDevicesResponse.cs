using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resistenza.Common.Packets.Camera
{
    public class CamListDevicesResponse
    {
        public string Type { get; set; }
        public List<string> DevicesNames { get; set; }

        public CamListDevicesResponse()
        {

            Type = this.GetType().ToString();
            DevicesNames = new List<string>();
        }
    }
}
