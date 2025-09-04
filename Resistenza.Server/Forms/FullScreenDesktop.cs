using System;
using System.Windows.Forms;
using Resistenza.Common;
using Resistenza.Common.Networking;
using Resistenza.Server.Networking;

namespace Resistenza.Server.Forms
{
    public partial class FullScreenDesktop : Form
    {
        public FullScreenDesktop(ConnectedClient Client, string MonitorName)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

            this.Text = $"{MonitorName}@{Client.IpAddress}";

            FramePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

            _Client = Client;

        }

        private ConnectedClient _Client;

        public void AddFrame(Bitmap Frame)
        {
            FramePictureBox.Image = Frame;
        }

        public void StartInteractionMode()
        {

        }

        public void StopInteractionMode()
        {
        }


       
    }
}
