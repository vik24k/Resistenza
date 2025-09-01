using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resistenza.Common.Packets.Logic
{
    public class CancelRemoteOperationRequest : IPacket
    {
        public string Type { get; set; }
        public int TaskId { get; set; }
        

        public CancelRemoteOperationRequest()
        {
            Type = this.GetType().ToString();
        }
    }
}
