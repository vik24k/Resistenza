using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Formats.Tar;
using System.Linq;
using System.Management.Automation;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Org.BouncyCastle.Asn1.X509;
using Resistenza.Common.Packets;
using Resistenza.Common.Packets.Command;
using Resistenza.Common.Packets.Logic;
using Resistenza.Server.Networking;

namespace Resistenza.Server.Forms
{
    public partial class FrmRemoteShell : Form

    {

        private ConnectedClient _client;
        private Label _LastUsernameLabel;
        private Label _LastOutputLabel;
        private TextBox _LastCommandTextbox = new TextBox();

        private string ClientTargetDir;
        public FrmRemoteShell(ConnectedClient TargetClient)
        {


            InitializeComponent();
            clientUsernameLabel.Text = $"{TargetClient.UserName}@{TargetClient.IpAddress}> ";
            Size labelSize = TextRenderer.MeasureText(clientUsernameLabel.Text, clientUsernameLabel.Font);

            _client = TargetClient;
            _LastUsernameLabel = clientUsernameLabel;
            _LastCommandTextbox = commandTextbox;

            _LastCommandTextbox.Left = clientUsernameLabel.Left + labelSize.Width + 10;
            _LastCommandTextbox.Top = clientUsernameLabel.Top + (labelSize.Height - commandTextbox.Height) / 2;

            ActiveControl = _LastCommandTextbox;

            BackgroundPanel.AutoScroll = true;
            BackgroundPanel.AutoSizeMode = AutoSizeMode.GrowOnly;
            //panel1.AutoScrollMinSize = new Size(0, panel1.Height + 1);

            ClientTargetDir = "C:\\Users\\" + _client.UserName;

            _LastCommandTextbox.KeyPress += _LastCommandTextbox_KeyPress;
            _LastCommandTextbox.TextChanged += ResizeTextBox;

            _client.IncomingPacket += OnClientIncomingPacket;
        }

        private void OnClientIncomingPacket(object PacketReceived)
        {
            switch (PacketReceived)
            {
                case DirectoryDataResponse:

                    DirectoryDataResponse DoesDirectoryExists = (DirectoryDataResponse)PacketReceived;

                    if (DoesDirectoryExists.Error != null && DoesDirectoryExists.Error != string.Empty) //errore, non esiste o inaccessibile
                    {
                        PrintOutput((DoesDirectoryExists.Error).Replace("\n", ""), false, true);
                    }
                    else //non c'è errore, la directory esiste
                    {
                        PrintOutput((DoesDirectoryExists.Directory).Replace("\n", ""), false, true);
                        ClientTargetDir = DoesDirectoryExists.Directory;
                    }
                    return;
                case ExecuteCommandResponse:

                    ExecuteCommandResponse OutputAndError = (ExecuteCommandResponse)PacketReceived;
                    if (OutputAndError != null)
                    {
                        if (OutputAndError.IsPart)
                        {
                            //parte dell'output di un comando 
                            PrintOutput(OutputAndError.Output + OutputAndError.Error, true);
                        }

                        else
                        {
                            if (OutputAndError.HasExecutionEnded)
                            {
                                PrintOutput("", false);
                            }
                            else
                            {
                                PrintOutput((OutputAndError.Output + OutputAndError.Error).Replace("\n", ""), false);
                            }

                        }


                    }

                    return;

            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Rimuovi l'handler dell'evento MyEvent

            _client.IncomingPacket -= OnClientIncomingPacket; // MyEventHandlerMethod è il metodo che gestisce l'evento


        }

        private void PrintOutput(string Output, bool IsPart, bool PrintingCdResult = false)
        {
            TextBox newCommandTextBox = new TextBox();
            Label newUserNameLabel = new Label();
            Size labelSize = new Size();

            Label newCommandOutput = new Label();
            newCommandOutput.Text = Output; //setting output

            if (_LastOutputLabel == null || !IsPart) // ==null nel senso che è il primo pacchetto di una parte interattiva
            {
                newCommandOutput.Location = new Point(_LastUsernameLabel.Left, _LastUsernameLabel.Bottom + 10);
            }
            else
            {
                newCommandOutput.Location = new Point(_LastOutputLabel.Left, _LastOutputLabel.Bottom);
            }

            newCommandOutput.AutoSize = true;
            newCommandOutput.ForeColor = Color.Green;
            newCommandOutput.BackColor = Color.Black;
            BackgroundPanel.Controls.Add(newCommandOutput);


            if (IsPart)
            {
                _LastOutputLabel = newCommandOutput;
                return;
            }


            newUserNameLabel = CopyLabel(_LastUsernameLabel);
            if (_LastOutputLabel == null)
            {
                newUserNameLabel.Location = new Point(newCommandOutput.Left, newCommandOutput.Bottom + 10);
            }
            else
            {
                newUserNameLabel.Location = new Point(_LastOutputLabel.Left, _LastOutputLabel.Bottom + 10);
            }
            newUserNameLabel.AutoSize = true;
            BackgroundPanel.Controls.Add(newUserNameLabel);


            _LastCommandTextbox.ReadOnly = _LastCommandTextbox.Text != string.Empty ? true : false;

            newCommandTextBox = CopyTextbox(_LastCommandTextbox);

            labelSize = TextRenderer.MeasureText(newUserNameLabel.Text, newUserNameLabel.Font);

            newCommandTextBox.Left = newUserNameLabel.Left + labelSize.Width + 10;
            newCommandTextBox.Top = newUserNameLabel.Top + (labelSize.Height - commandTextbox.Height) / 2;

            BackgroundPanel.Controls.Add(newCommandTextBox);

            newCommandTextBox.Focus();
            newCommandTextBox.Select(0, 0);
            _LastCommandTextbox = newCommandTextBox;
            _LastUsernameLabel = newUserNameLabel;
            _LastOutputLabel = newCommandOutput;

            //l'oggetto è cambiato, quindi è necessario riscrivere l'evento
            _LastCommandTextbox.TextChanged += ResizeTextBox;
            _LastCommandTextbox.KeyPress += _LastCommandTextbox_KeyPress;

            if (Output == string.Empty || PrintingCdResult)
            {
                _LastOutputLabel = null;
            }

        }

        private void ResizeTextBox(object Sender, EventArgs e)
        {
            Size TextBoxSize = TextRenderer.MeasureText(_LastCommandTextbox.Text, _LastCommandTextbox.Font);

            if (TextBoxSize.Width > _LastCommandTextbox.Width)
            {
                _LastCommandTextbox.Width = _LastCommandTextbox.Width + (TextBoxSize.Width - _LastCommandTextbox.Width);
            }

            //_LastCommandTextbox.Width = TextBoxSize.Width;
            //_LastCommandTextbox.Height = TextBoxSize.Height;
            //if (_LastCommandTextbox.Width > BackgroundPanel.Width)
            //{
            //    MessageBox.Show("out of border");
            //}

        }


        private TextBox CopyTextbox(TextBox TextboxToCopy)
        {
            TextBox Copied = new TextBox();
            Copied.Location = TextboxToCopy.Location;
            Copied.BackColor = TextboxToCopy.BackColor;
            Copied.ForeColor = TextboxToCopy.ForeColor;
            Copied.BorderStyle = TextboxToCopy.BorderStyle;


            return Copied;
        }

        private Label CopyLabel(Label LabelToCopy)
        {
            Label Copied = new Label();
            Copied.Text = LabelToCopy.Text;
            Copied.Location = LabelToCopy.Location;
            Copied.ForeColor = LabelToCopy.ForeColor;
            Copied.BackColor = LabelToCopy.BackColor;
            Copied.Font = LabelToCopy.Font;




            return Copied;

        }

        static void CommandEnteredHandler(object sender, KeyEventArgs e)
        {

        }

        private void commandTextbox_TextChanged(object sender, EventArgs e)
        {

        }

        private async void _LastCommandTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {

            Label newUserNameLabel = new Label();
            TextBox newCommandTextBox = new TextBox();
            Size labelSize = new Size();

            if (e.KeyChar == (char)Keys.Enter)
            {

                if (_LastCommandTextbox.Text.StartsWith("cd") || _LastCommandTextbox.Text.StartsWith("Cd") || _LastCommandTextbox.Text.StartsWith("CD"))
                {
                    string ChoosenDir = new string(_LastCommandTextbox.Text.Skip(2).ToArray()).Replace(" ", "");
                    string DirToTest = string.Empty;
                    switch (ChoosenDir)
                    {
                        case ".":
                            DirToTest = ClientTargetDir;
                            break;
                        case "..":
                            if (Directory.GetParent(ClientTargetDir) == null)//root del percorso, ad esempio C:\
                            {
                                break;
                            }
                            DirToTest = Directory.GetParent(ClientTargetDir).FullName;
                            break;
                        default:
                            if (ChoosenDir.StartsWith(ClientTargetDir.Split("\\")[0]))
                            {
                                DirToTest = ChoosenDir;
                                break;
                            }
                            DirToTest = Path.Join(ClientTargetDir, ChoosenDir);
                            break;

                    }

                    //è necessario verificare che la directory esista

                    var RequestDirDataCommand = new DirectoryDataRequest
                    {
                        Directory = DirToTest

                    };

                    await _client.CustomStream.SendPacketAsync(RequestDirDataCommand);
                    return;

                }

                switch (_LastCommandTextbox.Text)
                {


                    case "":
                        //newUserNameLabel = CopyLabel(_LastUsernameLabel);
                        //newUserNameLabel.Location = new Point(_LastUsernameLabel.Left, _LastUsernameLabel.Bottom + 10);
                        //newUserNameLabel.AutoSize = true;
                        //BackgroundPanel.Controls.Add(newUserNameLabel);

                        //_LastUsernameLabel = CopyLabel(newUserNameLabel);
                        //_LastCommandTextbox.ReadOnly = true;


                        //newCommandTextBox = CopyTextbox(_LastCommandTextbox);

                        //labelSize = TextRenderer.MeasureText(newUserNameLabel.Text, newUserNameLabel.Font);

                        //newCommandTextBox.Left = newUserNameLabel.Left + labelSize.Width + 10;
                        //newCommandTextBox.Top = newUserNameLabel.Top + (labelSize.Height - commandTextbox.Height) / 2;

                        //BackgroundPanel.Controls.Add(newCommandTextBox);
                        PrintOutput(string.Empty, false);
                        break;
                    case "clear":
                    case "Clear":
                    case "CLEAR":

                        BackgroundPanel.Controls.Clear();
                        newUserNameLabel = CopyLabel(_LastUsernameLabel);
                        newUserNameLabel.Location = new Point(12, 27);
                        newUserNameLabel.AutoSize = true;

                        newUserNameLabel.ForeColor = Color.Green;
                        newUserNameLabel.BackColor = Color.Black;
                        BackgroundPanel.Controls.Add(newUserNameLabel);


                        newCommandTextBox = CopyTextbox(_LastCommandTextbox);

                        labelSize = TextRenderer.MeasureText(newUserNameLabel.Text, newUserNameLabel.Font);

                        newCommandTextBox.Left = newUserNameLabel.Left + labelSize.Width + 10;
                        newCommandTextBox.Top = newUserNameLabel.Top + (labelSize.Height - commandTextbox.Height) / 2;


                        BackgroundPanel.Controls.Add(newCommandTextBox);

                        _LastCommandTextbox = newCommandTextBox;
                        _LastUsernameLabel = newUserNameLabel;

                        //l'oggetto è cambiato, quindi è necessario riscrivere l'evento
                        _LastCommandTextbox.TextChanged += ResizeTextBox;
                        _LastCommandTextbox.KeyPress += _LastCommandTextbox_KeyPress;
                        break;

                    default:
                        ExecuteCommandRequest Pkt = new ExecuteCommandRequest
                        {
                            Command = _LastCommandTextbox.Text,
                            TargetDir = ClientTargetDir,
                            InteractiveOutput = true

                        };


                        await _client.CustomStream.SendPacketAsync(Pkt);
                        break;
                }


            }



        }


    }

}
