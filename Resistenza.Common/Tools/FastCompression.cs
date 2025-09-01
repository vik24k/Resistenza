using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace Resistenza.Common.Tools
{
    public class FastCompression
    {
        public static byte[] Compress(byte[] DataToCompress)
        {
            using var NewStream = new MemoryStream();
            using (var OldStream = new MemoryStream(DataToCompress))
            {
                using (var Compressor = new GZipStream(NewStream, CompressionMode.Compress))
                {
                    // Create a buffer to hold the compressed data
                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    // Read from the input stream in chunks and write to the compressed stream
                    while ((bytesRead = OldStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        Compressor.Write(buffer, 0, bytesRead);
                    }
                }
            }

            return NewStream.ToArray();
        }

        public static byte[] Decompress(byte[] DataToDecompress)
        {
            using var compressedStream = new MemoryStream(DataToDecompress);
            using var decompressedStream = new MemoryStream();

            using (var decompressor = new GZipStream(compressedStream, CompressionMode.Decompress))
            {
                // Create a buffer to hold the decompressed data
                byte[] buffer = new byte[1024];
                int bytesRead;

                // Read from the compressed stream in chunks and write to the decompressed stream
                while ((bytesRead = decompressor.Read(buffer, 0, buffer.Length)) > 0)
                {
                    decompressedStream.Write(buffer, 0, bytesRead);
                }
            }

            return decompressedStream.ToArray();
        }
    }
}
