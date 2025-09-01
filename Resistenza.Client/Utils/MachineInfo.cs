using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using System.Management;
using Windows.Web.AtomPub;
using System.Net.Sockets;

namespace Resistenza.Client.Utils
{
    internal class MachineInfo
    {
        public static async Task<string> GetExternalIpAsync()
        {
            var externalIpString = (await new HttpClient().GetStringAsync("http://icanhazip.com"))
        .Replace("\\r\\n", "").Replace("\\n", "").Trim();
            if (!IPAddress.TryParse(externalIpString, out var ipAddress)) return null;
            return ipAddress.ToString();
        }

        public static string GetLanIp()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return string.Empty;
        }

        public static bool IsAdmin()
        {
            try
            {
                File.Open("C:\\Windows\\regedit.exe", FileMode.Open);
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            return true;
        }

        public static string GetOs()
        {
            string fullString = Environment.OSVersion.ToString();
            return fullString.Replace("Microsoft", "").Replace("NT", "");
        }

        public static string GetAntivirus()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"\\" + Environment.MachineName + @"\root\SecurityCenter2", "SELECT * FROM AntivirusProduct");
            ManagementObjectCollection data = searcher.Get();
            if(data.Count == 0) { return "None"; }
            ManagementObject ManagementObj = data.OfType<ManagementObject>().FirstOrDefault();
            return ManagementObj["displayName"].ToString();
           
        }
    }
}
