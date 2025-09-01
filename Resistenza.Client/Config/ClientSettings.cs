using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resistenza.Client.Config
{
    public static class ClientSettings
    {
        public static readonly string Host = "127.0.0.1";
        public static readonly int Port = 2424;


        public static readonly int MillisecondsBeforeTryingToReconnect = 300; 
        public static readonly string MutexName = "ResistenzaMutex";
        public static readonly bool InstallProgram = false;
        public static readonly string InstallationPath = "C:\\";
        public static readonly string InstallationNewName = "notmalware.exe";
        public static readonly bool FakeError = true;
        public static readonly string ErrorTitle = "Error title";
        public static readonly string ErrorBody = "Error body";
        public static readonly string Username = "vik24k";
    }
}
