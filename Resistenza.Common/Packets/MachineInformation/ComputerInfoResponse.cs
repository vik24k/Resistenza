using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

namespace Resistenza.Common.Packets
{

    public class ComputerInfoResponse : IPacket
    {

        public string ClientUsername { get; set; }
        public string ipAddress { get; set; }
        public string lanAddress { get; set; }
        public string operatingSystem { get; set; }

        public string computerName { get; set; }
        public string ComputerUsername { get; set; }
        public bool isAdmin { get; set; }

        public string Antivirus { get; set; }

        public string Country { get; set; }

        public string Type { get; set; }
        public string Error { get; set; }

        public ComputerInfoResponse() {

            Type = this.GetType().ToString();

        }
        
        
        
        




    }
}
