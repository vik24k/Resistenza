using Resistenza.Server.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Resistenza.Server.Config
{

    //Enorme workaround qui, affinchè la classe Settings sia sempre accessibile ha senso che tutti i membri siano statici per evitare la creazione di instanze,
    // allo stesso tempo le classi / membri non statici non sono serializabili e quindi non possono essere scritti in un file, per cui Settings.WriteToFile()
    //crea un mirror di sè stessa che però non è statico e quindi è instanziabile e serializza quella classe, Settings.WriteFromFile() invece deserializza il file
    //creando di nuovo un'instanza di Settings Instantiable() e copia i membri di quella classe 
    internal class SettingsInstantiable
    {
        public int ListeningPort { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string CertificatePath { get; set; }

        public bool SoundNotification { get; set; }

        public bool EnableLogging { get; set; }  

        public bool AuthFailNotification { get; set; }

        public bool StartServerAtBoot { get; set; }

        public string LogFilePath { get; set; } 
    }

    public class ServerSettings
    {
        static public int ListeningPort { get; set; }
        static public string Username { get; set; }
        static public string Password { get; set; }
        static  public string CertificatePath { get; set; }
        static public bool SoundNotification { get; set; }
        static public bool EnableLogging { get; set; }
        static public bool AuthFailNotification { get; set; }
        static public bool StartServerAtBoot { get; set; }
        static public string LogFilePath { get; set; }

        static public void WriteToFile(string JsonFilePath)
        {
            SettingsInstantiable SettingsInstance = new();
            SettingsInstance.ListeningPort = ListeningPort;
            SettingsInstance.Username = Username;
            SettingsInstance.Password = Password;   
            SettingsInstance.CertificatePath = CertificatePath;
            SettingsInstance.SoundNotification = SoundNotification;
            SettingsInstance.EnableLogging = EnableLogging;
            SettingsInstance.AuthFailNotification = AuthFailNotification;
            SettingsInstance.StartServerAtBoot  = StartServerAtBoot;
            SettingsInstance.LogFilePath = LogFilePath;

            //riferimento ambiguo

            var Json = System.Text.Json.JsonSerializer.Serialize(SettingsInstance);
            File.WriteAllText(JsonFilePath, Json);
        }

        static public void LoadFromFile(string JsonFilePath)
        {
            SettingsInstantiable SettingsInstance = new SettingsInstantiable();
            using (StreamReader Reader = new StreamReader(JsonFilePath))
            {
                string Json = Reader.ReadToEnd();
                SettingsInstance = JsonConvert.DeserializeObject<SettingsInstantiable>(Json);
                ListeningPort = SettingsInstance.ListeningPort;
                Username = SettingsInstance.Username;
                Password = SettingsInstance.Password;
                CertificatePath = SettingsInstance.CertificatePath;
                SoundNotification = SettingsInstance.SoundNotification;
                EnableLogging = SettingsInstance.EnableLogging;
                AuthFailNotification = SettingsInstance.AuthFailNotification;
                StartServerAtBoot = SettingsInstance.StartServerAtBoot; 
                LogFilePath = SettingsInstance.LogFilePath;
                

            }
        }
    }

}
