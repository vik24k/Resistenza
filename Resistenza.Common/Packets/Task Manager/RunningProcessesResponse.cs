using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resistenza.Common.Packets.Task_Manager
{
    public class RunningProcessesResponse
    {

        public string Type { get; set; }
        public ProcessInfo Entry{ get; set; }
        public bool IsFirst {  get; set; }
        public bool IsLast { get; set; }
        


        public RunningProcessesResponse()
        {
            Type = this.GetType().ToString();
        }
    }
}
