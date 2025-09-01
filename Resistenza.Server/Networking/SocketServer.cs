using Resistenza.Server.Config;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using Resistenza.Server.Encryption;
using System.Net.Security;
using System.Net.Http;
using System.Security.Authentication;

using System.ComponentModel;


using Resistenza.Server.Utilities;
using Resistenza.Common.Packets;
using System.Diagnostics.Eventing.Reader;
using Newtonsoft.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

enum ServerStatus
{
    Running,
    Stopped,
    Error
}

namespace Resistenza.Server.Networking
{
    internal sealed class SocketServer
    {

        //singleton pattern
        private SocketServer()
        {

        }

        private CancellationTokenSource stopListeningTokenSource = new CancellationTokenSource();
        

        private static readonly SocketServer _instance = new SocketServer();

        public async Task<bool> Initialize()
        {
            bool Result = await CreateServerCertificateAsync();
            if (!Result)
            {
                MessageBox.Show("Uknown error while creating TLS certificate. Check session log file for more info.", "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }
            ConnectedClients = new ConnectedClientsManager(); 

            return true;
        }
        public static SocketServer GetInstance()
        {
            return _instance;
        }

        public struct ConnectedClientsManager
        {
            private List<ConnectedClient> _clientsList;

            public ConnectedClientsManager()
            {
                _clientsList = new List<ConnectedClient>();
            }

            public void Add(ConnectedClient ClientToAdd)
            {
                _clientsList.Add(ClientToAdd);
            }

            public int GetLength()
            {
                return _clientsList.Count;
            }

            public void DisconnectAll()
            {
                foreach(ConnectedClient ConnectedClient in _clientsList)
                {
                    ConnectedClient.Disconnect();
                    
                }

                _clientsList.Clear();

            }

            public void DisconnectOne(ConnectedClient ClientToDisconnect)
            {
                if (_clientsList.Contains(ClientToDisconnect))
                {
                    ClientToDisconnect.CustomStream.Disconnect();
                    _clientsList.Remove(ClientToDisconnect);
                }
            }


        }

        private TcpListener ServerListener;
        public bool IsRunning = false;
        private bool _isListening = false;
        
        public X509Certificate2 ServerCert;
        public ConnectedClientsManager ConnectedClients;


        public delegate void OnNewServerStatusHandler(ServerStatus NewServerStatus);
        public event OnNewServerStatusHandler ServerStatusHasChanged;

        public delegate void OnClientConnected(ConnectedClient NewClient, ComputerInfoResponse NewClientInfo);
        public event OnClientConnected NewClientHasConnected;

        private async Task<bool> CreateServerCertificateAsync()
        {
            string exePath = System.Reflection.Assembly.GetEntryAssembly().Location;
            string certDir = Path.Combine(Directory.GetParent(exePath).FullName, "Certificates");
            Directory.CreateDirectory(certDir);

            string pfxPath = Path.Combine(certDir, "RServerCert.pfx");
            string password = "pass";

            // Se il certificato PFX non esiste, generalo
            if (!File.Exists(pfxPath))
            {
                bool created = await ServerCertificate.GenerateSelfSignedPfxAsync(pfxPath, password);
                if (!created)
                    return false;
            }

            // Carica il certificato con chiave privata disponibile per TLS
            ServerCert = new X509Certificate2(
                pfxPath,
                password,
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable
            );

            Console.WriteLine($"Certificato caricato. HasPrivateKey={ServerCert.HasPrivateKey}");
            return true;
        }
        public async Task StartServerAsync()
        {

            IPAddress ServerAddress = IPAddress.Any;
            IPEndPoint ServerEndpoint = new IPEndPoint(ServerAddress, ServerSettings.ListeningPort);
            ServerListener = new TcpListener(ServerEndpoint);

            //starting the server
           
            ServerListener.Start();
            ServerStatusHasChanged?.Invoke(ServerStatus.Running);
            IsRunning = true;
            LogEvent.Write(LogEvent.CreateLogString(LogEvent.LogLevel.Info, $"Server started successfully, running on port {ServerSettings.ListeningPort}"));
            await AcceptClientsAsync();
                
            
            

        }

        public async Task StopServerAsync()
        {
            
            if (!IsRunning)
            {
                return;
            }

            stopListeningTokenSource.Cancel();

            while (_isListening)
            {
                await Task.Delay(25);
            }

            ConnectedClients.DisconnectAll();

            ServerStatusHasChanged?.Invoke(ServerStatus.Stopped);

            IsRunning = false;
                

            LogEvent.Write(LogEvent.CreateLogString(LogEvent.LogLevel.Info, "Server stopped by user."));

        }

        private async Task AcceptClientsAsync()
        {

            try {

                while (true)
                {
                    _isListening = true;
                    TcpClient Connection;

                    Connection = await ServerListener.AcceptTcpClientAsync(stopListeningTokenSource.Token);
                    ConnectedClient newClient = new ConnectedClient(Connection);
                    ComputerInfoResponse? newClientInfo = await newClient.AuthenticateAsync();
                    if (newClientInfo == null)
                    {
                        LogEvent.Write(LogEvent.CreateLogString(LogEvent.LogLevel.Error, "Client with IP:", newClient.IpAddress, "failed authentication, forcing disconnection"));

                        ConnectedClients.DisconnectOne(newClient);

                    }
                    else
                    {

                        ConnectedClients.Add(newClient);


                        foreach (OnClientConnected NewClientHandler in NewClientHasConnected.GetInvocationList())
                        {
                            if (NewClientHandler.Target is Control control)
                            {
                                control.Invoke(NewClientHandler, newClient, newClientInfo);
                            }
                        }


                        LogEvent.Write(LogEvent.CreateLogString(LogEvent.LogLevel.Info, "Client with IP:", newClient.IpAddress, "connected successfully"));

                        Task ReadingLoop = newClient.BeginReadingAsync();

                    }


                }
            
             }
                           
            catch (OperationCanceledException) 
            {
                ServerListener.Stop();
                _isListening = false;
            
                stopListeningTokenSource = new CancellationTokenSource();

                return;
            }
        }
    }

    


    }

