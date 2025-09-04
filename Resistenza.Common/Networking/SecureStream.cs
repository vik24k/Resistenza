using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Resistenza.Common.Packets;
using PacketSerializer = Resistenza.Common.Packets.PacketSerializer;

namespace Resistenza.Common.Networking
{
    public class SecureStream : SslStream
    {
        public event EventHandler<EventArgs> ErrorReadingSocket;
        public event EventHandler<EventArgs> ErrorWritingSocket;

        public SecureStream(Stream innerStream, RemoteCertificateValidationCallback? certCallback = null)
            : base(innerStream, leaveInnerStreamOpen: true, userCertificateValidationCallback: certCallback)
        {
        }

        // SERVER SIDE AUTHENTICATION
        public async Task<bool> AuthenticateAsServerAsync(
            X509Certificate serverCertificate,
            bool clientCertificateRequired,
            SslProtocols enabledSslProtocols,
            bool checkCertificateRevocation,
            int timeoutMs = 5000)
        {
            using var cts = new CancellationTokenSource(timeoutMs);
            try
            {
                var authTask = base.AuthenticateAsServerAsync(
                    serverCertificate,
                    clientCertificateRequired,
                    enabledSslProtocols,
                    checkCertificateRevocation);

                await authTask.WaitAsync(cts.Token);
                return true;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Server authentication timed out.");
                return false;
            }
            catch (AuthenticationException ex)
            {
                Console.WriteLine("Server TLS handshake failed:");
                Console.WriteLine($"Message: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                return false;
            }
        }

        // CLIENT SIDE AUTHENTICATION (ignora certificati non trusted)
        public async Task<bool> AuthenticateAsClientAsync(
            string targetHost,
            X509CertificateCollection? clientCertificates,
            SslProtocols enabledSslProtocols,
            bool checkCertificateRevocation,
            int timeoutMs = 5000)
        {
            using var cts = new CancellationTokenSource(timeoutMs);
            try
            {
                
                await base.AuthenticateAsClientAsync(targetHost, clientCertificates, enabledSslProtocols, checkCertificateRevocation)
                          .WaitAsync(cts.Token);
                return true;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Client authentication timed out.");
                return false;
            }
            catch (AuthenticationException ex)
            {
                Console.WriteLine("Client TLS handshake failed:");
                Console.WriteLine($"Message: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                return false;
            }
        }

        public void Disconnect()
        {
            this.Close();
        }

        // INVIO PACCHETTO CON TIMEOUT
        public async Task<bool> SendPacketAsync2(object pkt, int timeoutMs = 2500)
        {
            byte[] rawBuffer = PacketSerializer.Serialize(pkt);
            long packetSize = rawBuffer.Length;
            byte[] sizeBuffer = BitConverter.GetBytes(packetSize);

            var writeSizeTask = this.WriteAsync(sizeBuffer).AsTask();
            var writeDataTask = this.WriteAsync(rawBuffer).AsTask();
            var timeoutTask = Task.Delay(timeoutMs);

            var completed = await Task.WhenAny(Task.WhenAll(writeSizeTask, writeDataTask), timeoutTask);

            if (timeoutTask.IsCompleted)
            {
                ErrorWritingSocket?.Invoke(this, EventArgs.Empty);
                return false;
            }

            return true;
        }

        public async Task<bool> SendPacketAsync(object pkt, int timeoutMs = 5000, int chunkSize = 8192)
        {
            byte[] rawBuffer = PacketSerializer.Serialize(pkt);
            long packetSize = rawBuffer.Length;

            byte[] sizeBuffer = BitConverter.GetBytes(packetSize);

            
            using var cts = new CancellationTokenSource(timeoutMs);

            try
            {
                // Invia prima la dimensione
                await this.WriteAsync(sizeBuffer, 0, sizeBuffer.Length, cts.Token);

                int offset = 0;
                while (offset < rawBuffer.Length)
                {
                    int toSend = Math.Min(chunkSize, rawBuffer.Length - offset);
                    await this.WriteAsync(rawBuffer, offset, toSend, cts.Token);
                    offset += toSend;
                }

                //Console.WriteLine($"[DEBUG] Packet sent successfully: raw={packetSize} bytes");
                return true;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"[ERROR] SendPacketAsync timed out ({timeoutMs}ms).");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] SendPacketAsync exception: {ex}");
                return false;
            }
        }







        // LETTURA PACCHETTO CON CANCELLAZIONE
        public async Task<object?> ReadPacketAsync(CancellationToken cancellation = default)
        {
            try
            {
                byte[] sizeBuffer = new byte[8];
                await this.ReadExactlyAsync(sizeBuffer, cancellation);

                long packetSize = BitConverter.ToInt64(sizeBuffer);
                byte[] rawPacket = new byte[packetSize];

                await this.ReadExactlyAsync(rawPacket, cancellation);

                object? DeserializedPacket = PacketSerializer.Deserialize(rawPacket);

                return DeserializedPacket;
            }
            catch (OperationCanceledException)
            {
                cancellation.ThrowIfCancellationRequested();
                return null;
            }
            catch (Exception)
            {
                ErrorReadingSocket?.Invoke(this, EventArgs.Empty);
                return null;
            }
        }

        // ESTENSIONE PER LETTURA EXACTLY
        private async Task ReadExactlyAsync(byte[] buffer, CancellationToken cancellation = default)
        {
            int offset = 0;
            int remaining = buffer.Length;

            while (remaining > 0)
            {
                int read = await this.ReadAsync(buffer, offset, remaining, cancellation);
                if (read == 0) throw new IOException("Unexpected EOF or connection closed.");
                offset += read;
                remaining -= read;
            }
        }
    }
}
