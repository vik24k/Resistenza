using Resistenza.Client.Config;
using System.Diagnostics;
using System.Windows.Forms;

namespace Resistenza.Client
{
    internal class Program
    {
        static async Task Main()
        {        
            try {

                Networking.ClientSocket ClientSocket = Networking.ClientSocket.GetInstance();
                while (true)
                {
                    Console.WriteLine("Trying connecting to the server...");
                    while (!await ClientSocket.ConnectAsync())
                    {
                        Console.WriteLine($"Connection failed, retrying in {ClientSettings.MillisecondsBeforeTryingToReconnect} mills");
                        Thread.Sleep(Config.ClientSettings.MillisecondsBeforeTryingToReconnect);
                        
                    }
                    Console.WriteLine("Connection to the server succeded!");
                    await ClientSocket.BeginListeningAsync(); //se la connessione crasha, BeginListeningAsync() ritorna
                }
               
            }
            
            catch(Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.ToString());
            }
            

        }
    }
}
