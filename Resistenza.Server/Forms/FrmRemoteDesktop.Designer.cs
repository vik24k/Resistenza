namespace Resistenza.Server.Forms
{
    partial class FrmRemoteDesktop
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
            RecordingPathTextbox = new TextBox();
            PathLabel = new Label();
            DesktopPictureBox = new PictureBox();
            DisplaysBox = new ComboBox();
            AvailableCamsLabel = new Label();
            StartWatchingButton = new FormsAddons.RoundedButton();
            InteractionModeBtn = new FormsAddons.RoundedButton();
            FrameRateLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)DesktopPictureBox).BeginInit();
            SuspendLayout();
            // 
            // RecordingPathTextbox
            // 
            RecordingPathTextbox.BackColor = Color.FromArgb(32, 34, 46);
            RecordingPathTextbox.BorderStyle = BorderStyle.FixedSingle;
            RecordingPathTextbox.ForeColor = Color.White;
            RecordingPathTextbox.Location = new Point(181, 86);
            RecordingPathTextbox.Margin = new Padding(3, 4, 3, 4);
            RecordingPathTextbox.Name = "RecordingPathTextbox";
            RecordingPathTextbox.Size = new Size(241, 27);
            RecordingPathTextbox.TabIndex = 22;
            // 
            // PathLabel
            // 
            PathLabel.AutoSize = true;
            PathLabel.ForeColor = Color.White;
            PathLabel.Location = new Point(30, 88);
            PathLabel.Name = "PathLabel";
            PathLabel.Size = new Size(145, 20);
            PathLabel.TabIndex = 21;
            PathLabel.Text = "Recordings File Path:";
            // 
            // DesktopPictureBox
            // 
            DesktopPictureBox.BackColor = Color.Black;
            DesktopPictureBox.Location = new Point(189, 160);
            DesktopPictureBox.Margin = new Padding(3, 4, 3, 4);
            DesktopPictureBox.Name = "DesktopPictureBox";
            DesktopPictureBox.Size = new Size(914, 511);
            DesktopPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            DesktopPictureBox.TabIndex = 20;
            DesktopPictureBox.TabStop = false;
            DesktopPictureBox.MouseEnter += DesktopPictureBox_MouseEnter;
            DesktopPictureBox.MouseLeave += DesktopPictureBox_MouseLeave;
            // 
            // DisplaysBox
            // 
            DisplaysBox.BackColor = Color.FromArgb(17, 17, 25);
            DisplaysBox.DropDownStyle = ComboBoxStyle.DropDownList;
            DisplaysBox.FlatStyle = FlatStyle.System;
            DisplaysBox.ForeColor = Color.White;
            DisplaysBox.FormattingEnabled = true;
            DisplaysBox.Location = new Point(183, 39);
            DisplaysBox.Margin = new Padding(3, 4, 3, 4);
            DisplaysBox.MaxDropDownItems = 50;
            DisplaysBox.Name = "DisplaysBox";
            DisplaysBox.Size = new Size(241, 28);
            DisplaysBox.TabIndex = 19;
            // 
            // AvailableCamsLabel
            // 
            AvailableCamsLabel.AutoSize = true;
            AvailableCamsLabel.ForeColor = Color.White;
            AvailableCamsLabel.Location = new Point(42, 42);
            AvailableCamsLabel.Name = "AvailableCamsLabel";
            AvailableCamsLabel.Size = new Size(133, 20);
            AvailableCamsLabel.TabIndex = 18;
            AvailableCamsLabel.Text = "Available Displays:";
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
            StartWatchingButton.Location = new Point(433, 710);
            StartWatchingButton.Margin = new Padding(3, 4, 3, 4);
            StartWatchingButton.Name = "StartWatchingButton";
            StartWatchingButton.Size = new Size(423, 69);
            StartWatchingButton.TabIndex = 23;
            StartWatchingButton.Text = "Start Watching Live";
            StartWatchingButton.UseVisualStyleBackColor = false;
            StartWatchingButton.Click += StartWatchingButton_Click;
            // 
            // InteractionModeBtn
            // 
            InteractionModeBtn.Anchor = AnchorStyles.Bottom;
            InteractionModeBtn.BackColor = Color.FromArgb(32, 34, 46);
            InteractionModeBtn.BorderColor = Color.FromArgb(32, 34, 46);
            InteractionModeBtn.BorderDownColor = Color.Empty;
            InteractionModeBtn.BorderDownWidth = 0F;
            InteractionModeBtn.BorderOverColor = Color.Empty;
            InteractionModeBtn.BorderOverWidth = 0F;
            InteractionModeBtn.BorderRadius = 50;
            InteractionModeBtn.BorderWidth = 1.75F;
            InteractionModeBtn.ForeColor = Color.White;
            InteractionModeBtn.Location = new Point(862, 710);
            InteractionModeBtn.Margin = new Padding(3, 4, 3, 4);
            InteractionModeBtn.Name = "InteractionModeBtn";
            InteractionModeBtn.Size = new Size(423, 69);
            InteractionModeBtn.TabIndex = 24;
            InteractionModeBtn.Text = "Interaction Mode: Off";
            InteractionModeBtn.UseVisualStyleBackColor = false;
            InteractionModeBtn.Visible = false;
            InteractionModeBtn.Click += InteractionModeBtn_Click;
            // 
            // FrameRateLabel
            // 
            FrameRateLabel.AutoSize = true;
            FrameRateLabel.ForeColor = Color.White;
            FrameRateLabel.Location = new Point(1169, 804);
            FrameRateLabel.Name = "FrameRateLabel";
            FrameRateLabel.Size = new Size(35, 20);
            FrameRateLabel.TabIndex = 25;
            FrameRateLabel.Text = "FPS:";
            FrameRateLabel.Visible = false;
            // 
            // FrmRemoteDesktop
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(17, 17, 25);
            ClientSize = new Size(1239, 833);
            Controls.Add(FrameRateLabel);
            Controls.Add(InteractionModeBtn);
            Controls.Add(StartWatchingButton);
            Controls.Add(RecordingPathTextbox);
            Controls.Add(PathLabel);
            Controls.Add(DesktopPictureBox);
            Controls.Add(DisplaysBox);
            Controls.Add(AvailableCamsLabel);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmRemoteDesktop";
            Text = "FrmRemoteDesktop";
            Load += FrmRemoteDesktop_Load;
            ((System.ComponentModel.ISupportInitialize)DesktopPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox RecordingPathTextbox;
        private Label PathLabel;
        private PictureBox DesktopPictureBox;
        private ComboBox DisplaysBox;
        private Label AvailableCamsLabel;
        private FormsAddons.RoundedButton StartWatchingButton;
        private FormsAddons.RoundedButton InteractionModeBtn;
        private Label FrameRateLabel;
    }
}