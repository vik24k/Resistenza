using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resistenza.Common.Packets.Microphone
{
    public class MicListDevicesResponse
    {
        public string Type { get; set; }
        public List<MicrophoneDeviceInfo> DevicesNames { get; set; }

        public MicListDevicesResponse() {

            Type = this.GetType().ToString();
            DevicesNames = new List<MicrophoneDeviceInfo>();
        }
    }
}
