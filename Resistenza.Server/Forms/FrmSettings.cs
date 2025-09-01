using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Resistenza.Server.Networking;

namespace Resistenza.Server.Forms
{
    public partial class FrmSettings : Form
    {

        private SocketServer ServerInstance = SocketServer.GetInstance();
        private string RegexFilePathValidator = @"^[a-zA-Z]:\\(((?![<>:""/\\|?*]).)+((?<![ .])\\)?)*$";

        private string ExePath = System.Reflection.Assembly.GetEntryAssembly().Location;
        private string ParentDir = Directory.GetParent(System.Reflection.Assembly.GetEntryAssembly().Location).FullName;

        public event EventHandler SettingsOpened;
        public event EventHandler SettingsClosed;

        public FrmSettings()
        {
            InitializeComponent();
            this.Padding = new Padding(2);
            this.BackColor = Color.FromArgb(51, 20, 98);

            this.mainPanel.BackColor = Color.FromArgb(30, 32, 37);
            this.mainPanel.Dock = DockStyle.Fill;


        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();



        private void EnableAllSettings()
        {
            PortNumericUpDown.Enabled = true;
            UsernameTextBox.Enabled = true;
            PasswordTextBox.Enabled = true;
            UsernameLabel.Enabled = true;
            PasswordLabel.Enabled = true;
            PortLabel.Enabled = true;
            AdditionalOptionsLabel.Enabled = true;
            SoundNotificationCheckbox.Enabled = true;
            AuthFailureCheckbox.Enabled = true;
            StartAtBootCheckbox.Enabled = true;
            StartListeningBtn.Enabled = true;
            EnableLoggingCheckbox.Enabled = true;
            StopListeningBtn.Enabled = false;


            if (EnableLoggingCheckbox.Checked)
            {
                EnableLoggingCheckbox.Enabled = true;
                LogFilePathLabel.Enabled = true;
                LogFilePathTextbox.Enabled = true;

            }


        }
        private void DisableAllSettings()
        {
            PortNumericUpDown.Enabled = false;
            UsernameTextBox.Enabled = false;
            PasswordTextBox.Enabled = false;
            UsernameLabel.Enabled = false;
            PasswordLabel.Enabled = false;
            PortLabel.Enabled = false;
            AdditionalOptionsLabel.Enabled = false;
            SoundNotificationCheckbox.Enabled = false;
            AuthFailureCheckbox.Enabled = false;
            StartAtBootCheckbox.Enabled = false;
            EnableLoggingCheckbox.Enabled = false;
            LogFilePathLabel.Enabled = false;
            LogFilePathTextbox.Enabled = false;
            StartListeningBtn.Enabled = false;
            StopListeningBtn.Enabled = true;
        }

        private void SetCorrectLogPath()
        {
            string DefaultLogPath = Path.Combine(Directory.GetParent(ExePath).FullName, "RLogs", $"RLog {DateAndTime.DateString}.txt");
            if (File.Exists(DefaultLogPath))
            {
                int DaySession = 1;

                while (File.Exists(DefaultLogPath))
                {
                    DefaultLogPath = Path.Combine(Directory.GetParent(ExePath).FullName, "RLogs", $"RLog {DateAndTime.DateString}({DaySession}).txt");
                    DaySession++;
                }
            }

            LogFilePathTextbox.Text = DefaultLogPath;
        }


        private void EnableLogging_CheckChanged(object sender, EventArgs e)
        {
            if (EnableLoggingCheckbox.Checked)
            {
                LogFilePathLabel.Enabled = true;
                LogFilePathTextbox.Enabled = true;

                SetCorrectLogPath();
            }
            else
            {
                LogFilePathLabel.Enabled = false;
                LogFilePathTextbox.Enabled = false;
                LogFilePathTextbox.Text = null;
            }

        }

        private async void StartListeningBtn_Click(object sender, EventArgs e)
        {


            if (UsernameTextBox.Text == "" && PasswordTextBox.Text == "")
            {
                InvalidUsernameOrPasswordLabel.Text = "Username and password \ncan't be empty!";
                InvalidUsernameOrPasswordLabel.Visible = true;
                return;

            }
            if (UsernameTextBox.Text == "")
            {
                InvalidUsernameOrPasswordLabel.Text = "Username can't be empty!";
                InvalidUsernameOrPasswordLabel.Visible = true;
                return;

            }
            if (PasswordTextBox.Text == "")
            {
                InvalidUsernameOrPasswordLabel.Text = "Password cant't be empty!";
                InvalidUsernameOrPasswordLabel.Visible = true;
                return;
            }



            string LogFilePath = LogFilePathTextbox.Text;


            //controllo validità path log 
            if (LogFilePathTextbox.Enabled)
            {
                if (LogFilePath == "")
                {
                    InvalidLogPathLabel.Text = "Log file path can't be empty!";
                    InvalidLogPathLabel.Visible = true;
                    return;
                }
                if (!Regex.IsMatch(LogFilePath, RegexFilePathValidator))
                {
                    InvalidLogPathLabel.Text = "Invalid path!";
                    InvalidLogPathLabel.Visible = true;
                    return;
                }

                try
                {
                    if (!Directory.Exists(Directory.GetParent(LogFilePath).FullName))
                    {
                        Directory.CreateDirectory(Directory.GetParent(LogFilePath).FullName);
                    }
                    using (FileStream fs = File.Create(
                        Path.Combine(
                            Directory.GetParent(LogFilePath).FullName,
                            Path.GetRandomFileName()
                        ),
                        1,
                        FileOptions.DeleteOnClose)
                    )
                    { }
                }
                catch (UnauthorizedAccessException)
                {
                    InvalidLogPathLabel.Text = "You have no access to that directory! \n Try running as admin.";
                    InvalidLogPathLabel.Visible = true;
                    return;
                }
            }


            InvalidLogPathLabel.Visible = false;
            InvalidUsernameOrPasswordLabel.Visible = false;


            //Le impostazioni vengono scritte nella classe Settings

            Config.ServerSettings.ListeningPort = (int)PortNumericUpDown.Value;
            Config.ServerSettings.Username = UsernameTextBox.Text;
            Config.ServerSettings.Password = PasswordTextBox.Text;
            Config.ServerSettings.SoundNotification = SoundNotificationCheckbox.Checked ? true : false;
            Config.ServerSettings.EnableLogging = EnableLoggingCheckbox.Checked ? true : false;
            Config.ServerSettings.LogFilePath = LogFilePathTextbox.Text;
            Config.ServerSettings.AuthFailNotification = AuthFailureCheckbox.Checked ? true : false;
            Config.ServerSettings.StartServerAtBoot = StartAtBootCheckbox.Checked ? true : false;

            Config.ServerSettings SettingsInstance = new Config.ServerSettings();

            Config.ServerSettings.WriteToFile(ParentDir + "\\server_settings.json");


            Task StartServerTask = ServerInstance.StartServerAsync();

            DisableAllSettings();
            StartListeningBtn.Visible = false;
            StopListeningBtn.Visible = true;

            await StartServerTask;



        }

        private void FrmSettings_Load(object sender, EventArgs e)

        {
            SettingsOpened?.Invoke(this, EventArgs.Empty);

            if (File.Exists(ParentDir + "\\server_settings.json"))
            {
                Config.ServerSettings.LoadFromFile(ParentDir + "\\server_settings.json");
                UsernameTextBox.Text = Config.ServerSettings.Username;
                PasswordTextBox.Text = Config.ServerSettings.Password;
                PortNumericUpDown.Value = Config.ServerSettings.ListeningPort;
                SoundNotificationCheckbox.Checked = Config.ServerSettings.SoundNotification;
                AuthFailureCheckbox.Checked = Config.ServerSettings.AuthFailNotification;
                EnableLoggingCheckbox.Checked = Config.ServerSettings.EnableLogging;
                StartAtBootCheckbox.Checked = Config.ServerSettings.StartServerAtBoot;
                if (EnableLoggingCheckbox.Checked)
                {
                    SetCorrectLogPath();
                }

            }



        }

        private async void StopListeningBtn_Click(object sender, EventArgs e)
        {
         
            await ServerInstance.StopServerAsync();
            EnableAllSettings();
            SetCorrectLogPath();

            StopListeningBtn.Visible = false;
            StartListeningBtn.Visible = true;



        }

        private void FrmSettings_Closing(object sender, FormClosingEventArgs e)
        {

            SettingsClosed?.Invoke(this, EventArgs.Empty);
            this.Hide();
            e.Cancel = true; //evita che il form sia smaltito
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmSettings_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void mainPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
