using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resistenza.Common.Packets
{
    public class ComputerInfoRequest
    {
        public string Type { get; set; }    
        public ComputerInfoRequest() {
            Type = this.GetType().ToString();
        }
        
    }
}
