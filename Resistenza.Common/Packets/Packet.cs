using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resistenza.Common.Packets
{
    public interface IPacket
    {
        string Type { get; set; }
        
    }
}
