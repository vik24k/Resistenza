


using System.Management.Automation;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Collections.ObjectModel;
using Microsoft.VisualBasic.Logging;

using Resistenza.Server.Utilities;

namespace Resistenza.Server.Encryption
{
    internal class ServerCertificate
    {

        public static async Task<bool> CreateSelfSignedCertificateAsync(string PfxFilePath, string CerFilePath)
        {


            string Script = $"$certname = \"Resistenza.Server\"\r\n" +
                $"$cert = New-SelfSignedCertificate -Subject \"CN=$certname\" -CertStoreLocation \"Cert:\\CurrentUser\\My\" -KeyExportPolicy Exportable -KeySpec Signature -KeyLength 2048 -KeyAlgorithm RSA -HashAlgorithm SHA256\r\n" +
                $"Export-Certificate -Cert $cert -FilePath \"{CerFilePath}\"\r\n" +
                $"$mypwd = ConvertTo-SecureString -String \"pass\" -Force -AsPlainText \r\n" +
                $"Export-PfxCertificate -Cert $cert -FilePath \"{PfxFilePath}\" -Password $mypwd \r\n" +
                $"Import-PfxCertificate -FilePath \"{PfxFilePath}\" -CertStoreLocation Cert:\\CurrentUser\\My -Password (ConvertTo-SecureString -String \"pass\" -Force -AsPlainText) \r\n" +
                $"Import-Certificate -FilePath \"{CerFilePath}\" -CertStoreLocation Cert:\\CurrentUser\\My";

            using (PowerShell Instance = PowerShell.Create())
            {
                Instance.AddScript(Script);
                PSDataCollection<PSObject> PsOutput = await Instance.InvokeAsync();



                if (Instance.HadErrors)
                {
                    string Errors = "";

                    foreach (var Error in PsOutput)
                    {
                        Errors += Error.ToString() + " ";
                    }

                    LogEvent.Write(LogEvent.CreateLogString(LogEvent.LogLevel.Fatal, "Failed creating TLS cert, powershell command output:", Errors, "REPORT THIS!"));

                    return false;
                }

                LogEvent.Write(LogEvent.CreateLogString(LogEvent.LogLevel.Info, "Created TLS cert at", PfxFilePath));
                LogEvent.Write(LogEvent.CreateLogString(LogEvent.LogLevel.Info, "Created TLS cert at", CerFilePath));


                return true;
            }
        }

        public static async Task<bool> GenerateSelfSignedPfxAsync(string pfxFilePath, string password)
        {
            string certName = "Resistenza.Server";

            string script = $@"
$cert = New-SelfSignedCertificate -Subject 'CN={certName}' `
    -CertStoreLocation 'Cert:\CurrentUser\My' `
    -KeyExportPolicy Exportable `
    -KeySpec Signature `
    -KeyLength 2048 `
    -KeyAlgorithm RSA `
    -HashAlgorithm SHA256

$pwd = ConvertTo-SecureString -String '{password}' -Force -AsPlainText
Export-PfxCertificate -Cert $cert -FilePath '{pfxFilePath}' -Password $pwd
";

            using var ps = PowerShell.Create();
            ps.AddScript(script);
            var output = await ps.InvokeAsync();

            if (ps.HadErrors)
            {
                string errors = string.Join(Environment.NewLine, output.Select(o => o.ToString()));
                Console.WriteLine("Errore generando certificato TLS: " + errors);
                return false;
            }

            Console.WriteLine($"Certificato PFX creato: {pfxFilePath}");
            return true;
        }

    }


}