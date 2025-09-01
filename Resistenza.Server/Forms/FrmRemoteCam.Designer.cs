namespace Resistenza.Server.Forms
{
    partial class FrmRemoteCam
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
            CamerasBox = new ComboBox();
            AvailableCamsLabel = new Label();
            CameraPictureBox = new PictureBox();
            StartWatchingButton = new FormsAddons.RoundedButton();
            FrameRateLabel = new Label();
            StartRecordingButton = new FormsAddons.RoundedButton();
            PathLabel = new Label();
            RecordingPathTextbox = new TextBox();
            TurnOnAudioIcon = new PictureBox();
            InstructionToEscLabel = new Label();
            TakePictureBtn = new FormsAddons.RoundedButton();
            ScreenshotProgressLabel = new Label();
            DownloadScreenshotBtn = new FormsAddons.RoundedButton();
            DiscardScreenshotBtn = new FormsAddons.RoundedButton();
            ((System.ComponentModel.ISupportInitialize)CameraPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TurnOnAudioIcon).BeginInit();
            SuspendLayout();
            // 
            // CamerasBox
            // 
            CamerasBox.BackColor = Color.FromArgb(17, 17, 25);
            CamerasBox.DropDownStyle = ComboBoxStyle.DropDownList;
            CamerasBox.FlatStyle = FlatStyle.System;
            CamerasBox.ForeColor = Color.White;
            CamerasBox.FormattingEnabled = true;
            CamerasBox.Location = new Point(172, 56);
            CamerasBox.Margin = new Padding(3, 4, 3, 4);
            CamerasBox.MaxDropDownItems = 50;
            CamerasBox.Name = "CamerasBox";
            CamerasBox.Size = new Size(241, 28);
            CamerasBox.TabIndex = 3;
            CamerasBox.SelectedIndexChanged += CamerasBox_SelectedIndexChanged;
            // 
            // AvailableCamsLabel
            // 
            AvailableCamsLabel.AutoSize = true;
            AvailableCamsLabel.ForeColor = Color.White;
            AvailableCamsLabel.Location = new Point(31, 59);
            AvailableCamsLabel.Name = "AvailableCamsLabel";
            AvailableCamsLabel.Size = new Size(135, 20);
            AvailableCamsLabel.TabIndex = 2;
            AvailableCamsLabel.Text = "Available Cameras:";
            // 
            // CameraPictureBox
            // 
            CameraPictureBox.BackColor = Color.Black;
            CameraPictureBox.Location = new Point(178, 177);
            CameraPictureBox.Margin = new Padding(3, 4, 3, 4);
            CameraPictureBox.Name = "CameraPictureBox";
            CameraPictureBox.Size = new Size(914, 511);
            CameraPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            CameraPictureBox.TabIndex = 4;
            CameraPictureBox.TabStop = false;
            CameraPictureBox.DoubleClick += CameraPictureBox_DoubleClick;
            // 
            // StartWatchingButton
            // 
            StartWatchingButton.Anchor = AnchorStyles.Bottom;
            StartWatchingButton.BackColor = Color.FromArgb(32, 34, 46);
            StartWatchingButton.BorderColor = Color.FromArgb(32, 34, 46);
            StartWatchingButton.BorderDownColor = Color.Empty;
            StartWatchingButton.BorderDownWidth = 0F;
            StartWatchingButton.BorderOverColor = Color.Empty;
            StartWatchingButton.BorderOverWidth = 0F;
            StartWatchingButton.BorderRadius = 50;
            StartWatchingButton.BorderWidth = 1.75F;
            StartWatchingButton.ForeColor = Color.White;
            StartWatchingButton.Location = new Point(417, 740);
            StartWatchingButton.Margin = new Padding(3, 4, 3, 4);
            StartWatchingButton.Name = "StartWatchingButton";
            StartWatchingButton.Size = new Size(423, 69);
            StartWatchingButton.TabIndex = 12;
            StartWatchingButton.Text = "Start Watching Live";
            StartWatchingButton.UseVisualStyleBackColor = false;
            StartWatchingButton.Click += StartWatchingButton_Click;
            // 
            // FrameRateLabel
            // 
            FrameRateLabel.AutoSize = true;
            FrameRateLabel.ForeColor = Color.White;
            FrameRateLabel.Location = new Point(1185, 846);
            FrameRateLabel.Name = "FrameRateLabel";
            FrameRateLabel.Size = new Size(35, 20);
            FrameRateLabel.TabIndex = 13;
            FrameRateLabel.Text = "FPS:";
            FrameRateLabel.Visible = false;
            // 
            // StartRecordingButton
            // 
            StartRecordingButton.Anchor = AnchorStyles.Bottom;
            StartRecordingButton.BackColor = Color.FromArgb(32, 34, 46);
            StartRecordingButton.BorderColor = Color.FromArgb(32, 34, 46);
            StartRecordingButton.BorderDownColor = Color.Empty;
            StartRecordingButton.BorderDownWidth = 0F;
            StartRecordingButton.BorderOverColor = Color.Empty;
            StartRecordingButton.BorderOverWidth = 0F;
            StartRecordingButton.BorderRadius = 50;
            StartRecordingButton.BorderWidth = 1.75F;
            StartRecordingButton.ForeColor = Color.White;
            StartRecordingButton.Location = new Point(847, 773);
            StartRecordingButton.Margin = new Padding(3, 4, 3, 4);
            StartRecordingButton.Name = "StartRecordingButton";
            StartRecordingButton.Size = new Size(423, 69);
            StartRecordingButton.TabIndex = 15;
            StartRecordingButton.Text = "Start Recording";
            StartRecordingButton.UseVisualStyleBackColor = false;
            StartRecordingButton.Visible = false;
            StartRecordingButton.Click += StartRecordingButton_Click;
            // 
            // PathLabel
            // 
            PathLabel.AutoSize = true;
            PathLabel.ForeColor = Color.White;
            PathLabel.Location = new Point(19, 105);
            PathLabel.Name = "PathLabel";
            PathLabel.Size = new Size(145, 20);
            PathLabel.TabIndex = 16;
            PathLabel.Text = "Recordings File Path:";
            // 
            // RecordingPathTextbox
            // 
            RecordingPathTextbox.BackColor = Color.FromArgb(32, 34, 46);
            RecordingPathTextbox.BorderStyle = BorderStyle.FixedSingle;
            RecordingPathTextbox.ForeColor = Color.White;
            RecordingPathTextbox.Location = new Point(170, 103);
            RecordingPathTextbox.Margin = new Padding(3, 4, 3, 4);
            RecordingPathTextbox.Name = "RecordingPathTextbox";
            RecordingPathTextbox.Size = new Size(241, 27);
            RecordingPathTextbox.TabIndex = 17;
            // 
            // TurnOnAudioIcon
            // 
            TurnOnAudioIcon.BackColor = Color.Transparent;
            TurnOnAudioIcon.Image = Properties.Resources.microphone_sensitivity_muted;
            TurnOnAudioIcon.Location = new Point(1066, 96);
            TurnOnAudioIcon.Margin = new Padding(3, 4, 3, 4);
            TurnOnAudioIcon.Name = "TurnOnAudioIcon";
            TurnOnAudioIcon.Size = new Size(26, 29);
            TurnOnAudioIcon.TabIndex = 18;
            TurnOnAudioIcon.TabStop = false;
            TurnOnAudioIcon.Visible = false;
            TurnOnAudioIcon.Click += TurnOnAudioIcon_Click;
            // 
            // InstructionToEscLabel
            // 
            InstructionToEscLabel.AutoSize = true;
            InstructionToEscLabel.BackColor = Color.Transparent;
            InstructionToEscLabel.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            InstructionToEscLabel.ForeColor = Color.White;
            InstructionToEscLabel.Location = new Point(562, 820);
            InstructionToEscLabel.Name = "InstructionToEscLabel";
            InstructionToEscLabel.Size = new Size(326, 46);
            InstructionToEscLabel.TabIndex = 19;
            InstructionToEscLabel.Text = "Double click to ESC";
            InstructionToEscLabel.Visible = false;
            // 
            // TakePictureBtn
            // 
            TakePictureBtn.Anchor = AnchorStyles.Bottom;
            TakePictureBtn.BackColor = Color.FromArgb(32, 34, 46);
            TakePictureBtn.BorderColor = Color.FromArgb(32, 34, 46);
            TakePictureBtn.BorderDownColor = Color.Empty;
            TakePictureBtn.BorderDownWidth = 0F;
            TakePictureBtn.BorderOverColor = Color.Empty;
            TakePictureBtn.BorderOverWidth = 0F;
            TakePictureBtn.BorderRadius = 50;
            TakePictureBtn.BorderWidth = 1.75F;
            TakePictureBtn.ForeColor = Color.White;
            TakePictureBtn.Location = new Point(846, 740);
            TakePictureBtn.Margin = new Padding(3, 4, 3, 4);
            TakePictureBtn.Name = "TakePictureBtn";
            TakePictureBtn.Size = new Size(423, 69);
            TakePictureBtn.TabIndex = 20;
            TakePictureBtn.Text = "Take Picture";
            TakePictureBtn.UseVisualStyleBackColor = false;
            TakePictureBtn.Click += TakePictureBtn_Click;
            // 
            // ScreenshotProgressLabel
            // 
            ScreenshotProgressLabel.AutoSize = true;
            ScreenshotProgressLabel.ForeColor = Color.White;
            ScreenshotProgressLabel.Location = new Point(12, 846);
            ScreenshotProgressLabel.Name = "ScreenshotProgressLabel";
            ScreenshotProgressLabel.Size = new Size(0, 20);
            ScreenshotProgressLabel.TabIndex = 21;
            ScreenshotProgressLabel.Visible = false;
            // 
            // DownloadScreenshotBtn
            // 
            DownloadScreenshotBtn.Anchor = AnchorStyles.Bottom;
            DownloadScreenshotBtn.BackColor = Color.FromArgb(32, 34, 46);
            DownloadScreenshotBtn.BorderColor = Color.FromArgb(32, 34, 46);
            DownloadScreenshotBtn.BorderDownColor = Color.Empty;
            DownloadScreenshotBtn.BorderDownWidth = 0F;
            DownloadScreenshotBtn.BorderOverColor = Color.Empty;
            DownloadScreenshotBtn.BorderOverWidth = 0F;
            DownloadScreenshotBtn.BorderRadius = 50;
            DownloadScreenshotBtn.BorderWidth = 1.75F;
            DownloadScreenshotBtn.ForeColor = Color.White;
            DownloadScreenshotBtn.Location = new Point(219, 546);
            DownloadScreenshotBtn.Margin = new Padding(3, 4, 3, 4);
            DownloadScreenshotBtn.Name = "DownloadScreenshotBtn";
            DownloadScreenshotBtn.Size = new Size(423, 69);
            DownloadScreenshotBtn.TabIndex = 22;
            DownloadScreenshotBtn.Text = "Download Screenshot";
            DownloadScreenshotBtn.UseVisualStyleBackColor = false;
            DownloadScreenshotBtn.Visible = false;
            DownloadScreenshotBtn.Click += DownloadScreenshotBtn_Click;
            // 
            // DiscardScreenshotBtn
            // 
            DiscardScreenshotBtn.Anchor = AnchorStyles.Bottom;
            DiscardScreenshotBtn.BackColor = Color.FromArgb(32, 34, 46);
            DiscardScreenshotBtn.BorderColor = Color.FromArgb(32, 34, 46);
            DiscardScreenshotBtn.BorderDownColor = Color.Empty;
            DiscardScreenshotBtn.BorderDownWidth = 0F;
            DiscardScreenshotBtn.BorderOverColor = Color.Empty;
            DiscardScreenshotBtn.BorderOverWidth = 0F;
            DiscardScreenshotBtn.BorderRadius = 50;
            DiscardScreenshotBtn.BorderWidth = 1.75F;
            DiscardScreenshotBtn.ForeColor = Color.White;
            DiscardScreenshotBtn.Location = new Point(683, 546);
            DiscardScreenshotBtn.Margin = new Padding(3, 4, 3, 4);
            DiscardScreenshotBtn.Name = "DiscardScreenshotBtn";
            DiscardScreenshotBtn.Size = new Size(423, 69);
            DiscardScreenshotBtn.TabIndex = 23;
            DiscardScreenshotBtn.Text = "Discard";
            DiscardScreenshotBtn.UseVisualStyleBackColor = false;
            DiscardScreenshotBtn.Visible = false;
            DiscardScreenshotBtn.Click += DiscardScreenshotBtn_Click;
            // 
            // FrmRemoteCam
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(17, 17, 25);
            ClientSize = new Size(1257, 880);
            Controls.Add(DiscardScreenshotBtn);
            Controls.Add(DownloadScreenshotBtn);
            Controls.Add(ScreenshotProgressLabel);
            Controls.Add(TakePictureBtn);
            Controls.Add(InstructionToEscLabel);
            Controls.Add(TurnOnAudioIcon);
            Controls.Add(RecordingPathTextbox);
            Controls.Add(PathLabel);
            Controls.Add(StartRecordingButton);
            Controls.Add(FrameRateLabel);
            Controls.Add(StartWatchingButton);
            Controls.Add(CameraPictureBox);
            Controls.Add(CamerasBox);
            Controls.Add(AvailableCamsLabel);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 4, 3, 4);
            Name = "FrmRemoteCam";
            Text = "FrmRemoteCam";
            FormClosing += FrmRemoteCam_FormClosing;
            Load += FrmRemoteCam_Load;
            KeyDown += FrmRemoteCam_KeyDown;
            ((System.ComponentModel.ISupportInitialize)CameraPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)TurnOnAudioIcon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox CamerasBox;
        private Label AvailableCamsLabel;
        private PictureBox CameraPictureBox;
        private FormsAddons.RoundedButton StartWatchingButton;
        private Label FrameRateLabel;
        private FormsAddons.RoundedButton StartRecordingButton;
        private Label PathLabel;
        private TextBox RecordingPathTextbox;
        private PictureBox TurnOnAudioIcon;
        private Label InstructionToEscLabel;
        private FormsAddons.RoundedButton TakePictureBtn;
        private Label ScreenshotProgressLabel;
        private FormsAddons.RoundedButton DownloadScreenshotBtn;
        private FormsAddons.RoundedButton DiscardScreenshotBtn;
    }
}