using Resistenza.Server.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resistenza.Server.Utilities
{
    public static class LogEvent
    {

       public enum LogLevel
        {
            Warning,
            Error,
            Info,
            Fatal
        }
       static public string CreateLogString(LogLevel Level, params string[] LogChunks)
        {
            string LogString = "";

          
            foreach(var LogChunk in LogChunks)
            {
                LogString += " ";
                LogString += LogChunk;

            }

            switch (Level)
            {
                case LogLevel.Warning:
                    LogString = LogString.Insert(0, "[WARNING] ");
                    break;
                case LogLevel.Fatal:
                    LogString = LogString.Insert(0, "[!!FATAL!!] ");
                    break;
                case LogLevel.Error:
                    LogString = LogString.Insert(0, "[ERROR]");
                    break;
                case LogLevel.Info:
                    LogString = LogString.Insert(0, "[INFO]");
                    break;
            }

            return LogString;
        }

        static public void Write(string LogString)
        {
            if (!ServerSettings.EnableLogging)
            {
                return;
            }

            File.AppendAllText(ServerSettings.LogFilePath, LogString + Environment.NewLine);
            
        }
    }
}
