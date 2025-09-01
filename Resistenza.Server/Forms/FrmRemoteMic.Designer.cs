namespace Resistenza.Server.Forms
{
    partial class FrmRemoteMic
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            AvailableMicsLabel = new Label();
            MicrophonesBox = new ComboBox();
            FilePathLabel = new Label();
            FilePathTextbox = new TextBox();
            OperationInProgressLabel = new Label();
            StartListeningButton = new FormsAddons.RoundedButton();
            StartRecordingButton = new FormsAddons.RoundedButton();
            pictureBox1 = new PictureBox();
            MicIcon = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MicIcon).BeginInit();
            SuspendLayout();
            // 
            // AvailableMicsLabel
            // 
            AvailableMicsLabel.AutoSize = true;
            AvailableMicsLabel.ForeColor = Color.White;
            AvailableMicsLabel.Location = new Point(32, 30);
            AvailableMicsLabel.Name = "AvailableMicsLabel";
            AvailableMicsLabel.Size = new Size(131, 15);
            AvailableMicsLabel.TabIndex = 0;
            AvailableMicsLabel.Text = "Available Microphones:";
            // 
            // MicrophonesBox
            // 
            MicrophonesBox.BackColor = Color.FromArgb(17, 17, 25);
            MicrophonesBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MicrophonesBox.FlatStyle = FlatStyle.System;
            MicrophonesBox.ForeColor = Color.White;
            MicrophonesBox.FormattingEnabled = true;
            MicrophonesBox.Location = new Point(178, 27);
            MicrophonesBox.MaxDropDownItems = 50;
            MicrophonesBox.Name = "MicrophonesBox";
            MicrophonesBox.Size = new Size(211, 23);
            MicrophonesBox.TabIndex = 1;
            MicrophonesBox.SelectedIndexChanged += MicrophonesBox_SelectedIndexChanged;
            // 
            // FilePathLabel
            // 
            FilePathLabel.AutoSize = true;
            FilePathLabel.ForeColor = Color.White;
            FilePathLabel.Location = new Point(29, 67);
            FilePathLabel.Name = "FilePathLabel";
            FilePathLabel.Size = new Size(134, 15);
            FilePathLabel.TabIndex = 3;
            FilePathLabel.Text = ".Wav Recorded File Path";
            // 
            // FilePathTextbox
            // 
            FilePathTextbox.BackColor = Color.FromArgb(32, 34, 46);
            FilePathTextbox.BorderStyle = BorderStyle.FixedSingle;
            FilePathTextbox.ForeColor = Color.White;
            FilePathTextbox.Location = new Point(178, 65);
            FilePathTextbox.Name = "FilePathTextbox";
            FilePathTextbox.Size = new Size(211, 23);
            FilePathTextbox.TabIndex = 4;
            // 
            // OperationInProgressLabel
            // 
            OperationInProgressLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            OperationInProgressLabel.AutoEllipsis = true;
            OperationInProgressLabel.AutoSize = true;
            OperationInProgressLabel.ForeColor = Color.White;
            OperationInProgressLabel.Location = new Point(12, 636);
            OperationInProgressLabel.Name = "OperationInProgressLabel";
            OperationInProgressLabel.Size = new Size(0, 15);
            OperationInProgressLabel.TabIndex = 6;
            OperationInProgressLabel.Visible = false;
            // 
            // StartListeningButton
            // 
            StartListeningButton.Anchor = AnchorStyles.Bottom;
            StartListeningButton.BackColor = Color.FromArgb(32, 34, 46);
            StartListeningButton.BorderColor = Color.FromArgb(32, 34, 46);
            StartListeningButton.BorderDownColor = Color.Empty;
            StartListeningButton.BorderDownWidth = 0F;
            StartListeningButton.BorderOverColor = Color.Empty;
            StartListeningButton.BorderOverWidth = 0F;
            StartListeningButton.BorderRadius = 50;
            StartListeningButton.BorderWidth = 1.75F;
            StartListeningButton.ForeColor = Color.White;
            StartListeningButton.Location = new Point(150, 525);
            StartListeningButton.Name = "StartListeningButton";
            StartListeningButton.Size = new Size(370, 50);
            StartListeningButton.TabIndex = 11;
            StartListeningButton.Text = "Start Listening ";
            StartListeningButton.UseVisualStyleBackColor = false;
            StartListeningButton.Click += StartPlayingButton_Click;
            // 
            // StartRecordingButton
            // 
            StartRecordingButton.BackColor = Color.FromArgb(32, 34, 46);
            StartRecordingButton.BorderColor = Color.FromArgb(32, 34, 46);
            StartRecordingButton.BorderDownColor = Color.Empty;
            StartRecordingButton.BorderDownWidth = 0F;
            StartRecordingButton.BorderOverColor = Color.Empty;
            StartRecordingButton.BorderOverWidth = 0F;
            StartRecordingButton.BorderRadius = 50;
            StartRecordingButton.BorderWidth = 1.75F;
            StartRecordingButton.ForeColor = Color.White;
            StartRecordingButton.Location = new Point(620, 525);
            StartRecordingButton.Name = "StartRecordingButton";
            StartRecordingButton.Size = new Size(370, 50);
            StartRecordingButton.TabIndex = 12;
            StartRecordingButton.Text = "Start Recording";
            StartRecordingButton.UseVisualStyleBackColor = false;
            StartRecordingButton.Visible = false;
            StartRecordingButton.Click += StartRecordingButton_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(346, 267);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(436, 89);
            pictureBox1.TabIndex = 13;
            pictureBox1.TabStop = false;
            // 
            // MicIcon
            // 
            MicIcon.Image = Properties.Resources.microphone_icon_374x512_xk3pbc2s_2_;
            MicIcon.Location = new Point(543, 395);
            MicIcon.Name = "MicIcon";
            MicIcon.Size = new Size(33, 47);
            MicIcon.TabIndex = 14;
            MicIcon.TabStop = false;
            // 
            // FrmRemoteMic
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(17, 17, 25);
            ClientSize = new Size(1100, 660);
            Controls.Add(MicIcon);
            Controls.Add(pictureBox1);
            Controls.Add(StartRecordingButton);
            Controls.Add(StartListeningButton);
            Controls.Add(OperationInProgressLabel);
            Controls.Add(FilePathTextbox);
            Controls.Add(FilePathLabel);
            Controls.Add(MicrophonesBox);
            Controls.Add(AvailableMicsLabel);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmRemoteMic";
            Text = "FrmRemoteMic";
            Load += FrmRemoteMic_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)MicIcon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label AvailableMicsLabel;
        private ComboBox MicrophonesBox;

        private Label FilePathLabel;
        private TextBox FilePathTextbox;
        private Label OperationInProgressLabel;
        private FormsAddons.RoundedButton StartListeningButton;
        private FormsAddons.RoundedButton StartRecordingButton;
        private PictureBox pictureBox1;
        private PictureBox MicIcon;
    }
}