using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resistenza.Common.Packets.Logic
{
    public class CancelRemoteOperationResponse : IPacket
    {
        public string Type { get; set; }
        public string Error { get; set; }
        public int TaskId { get; set; }
        

        public CancelRemoteOperationResponse() {

            Type = this.GetType().ToString();

        }
    }
}
