using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resistenza.Common.Packets.Task_Manager
{
    public class ActionOnProcessResponse
    {

        public string Type { get; set; }
        public string Name { get; set; }
        public string Error { get; set; }
        public PossibleActionsOnProcess PossibleActionsOnProcess { get; set; }
        public ActionOnProcessResponse() { 

            Type = this.GetType().ToString();
        }    
    }
}
