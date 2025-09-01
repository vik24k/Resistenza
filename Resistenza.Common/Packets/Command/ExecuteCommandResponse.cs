using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resistenza.Common.Packets.Command
{
    public class ExecuteCommandResponse : IPacket
    {
        public string Type { get; set; }
        public string Output { get; set; }
        public string Error { get; set; }

        public bool IsPart { get; set; }   //usato per shell remota interattiva
        public bool HasExecutionEnded { get; set; }


        public ExecuteCommandResponse()
        {
            Type = GetType().ToString();
        }
    }
}
