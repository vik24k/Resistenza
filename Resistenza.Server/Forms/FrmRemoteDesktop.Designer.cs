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
            DesktopPictureBox = new PictureBox();
            DisplaysBox = new ComboBox();
            AvailableCamsLabel = new Label();
            StartWatchingButton = new FormsAddons.RoundedButton();
            FullScreenBtn = new FormsAddons.RoundedButton();
            FrameRateLabel = new Label();
            InteractionPictureBox = new PictureBox();
            GenericNotificationLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)DesktopPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)InteractionPictureBox).BeginInit();
            SuspendLayout();
            // 
            // DesktopPictureBox
            // 
            DesktopPictureBox.BackColor = Color.Black;
            DesktopPictureBox.Location = new Point(165, 120);
            DesktopPictureBox.Name = "DesktopPictureBox";
            DesktopPictureBox.Size = new Size(800, 383);
            DesktopPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            DesktopPictureBox.TabIndex = 20;
            DesktopPictureBox.TabStop = false;
            DesktopPictureBox.Click += DesktopPictureBox_Click;
            DesktopPictureBox.DoubleClick += DesktopPictureBox_DoubleClick;
           
            // 
            // DisplaysBox
            // 
            DisplaysBox.AccessibleName = "MonitorBox";
            DisplaysBox.BackColor = Color.FromArgb(17, 17, 25);
            DisplaysBox.DropDownStyle = ComboBoxStyle.DropDownList;
            DisplaysBox.FlatStyle = FlatStyle.System;
            DisplaysBox.ForeColor = Color.White;
            DisplaysBox.FormattingEnabled = true;
            DisplaysBox.Location = new Point(165, 43);
            DisplaysBox.MaxDropDownItems = 50;
            DisplaysBox.Name = "DisplaysBox";
            DisplaysBox.Size = new Size(211, 23);
            DisplaysBox.TabIndex = 19;
            // 
            // AvailableCamsLabel
            // 
            AvailableCamsLabel.AutoSize = true;
            AvailableCamsLabel.ForeColor = Color.White;
            AvailableCamsLabel.Location = new Point(42, 46);
            AvailableCamsLabel.Name = "AvailableCamsLabel";
            AvailableCamsLabel.Size = new Size(104, 15);
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
            StartWatchingButton.Location = new Point(387, 567);
            StartWatchingButton.Name = "StartWatchingButton";
            StartWatchingButton.Size = new Size(370, 52);
            StartWatchingButton.TabIndex = 23;
            StartWatchingButton.Text = "Start Watching Live";
            StartWatchingButton.UseVisualStyleBackColor = false;
            StartWatchingButton.Click += StartWatchingButton_Click;
            // 
            // FullScreenBtn
            // 
            FullScreenBtn.Anchor = AnchorStyles.Bottom;
            FullScreenBtn.BackColor = Color.FromArgb(32, 34, 46);
            FullScreenBtn.BorderColor = Color.FromArgb(32, 34, 46);
            FullScreenBtn.BorderDownColor = Color.Empty;
            FullScreenBtn.BorderDownWidth = 0F;
            FullScreenBtn.BorderOverColor = Color.Empty;
            FullScreenBtn.BorderOverWidth = 0F;
            FullScreenBtn.BorderRadius = 50;
            FullScreenBtn.BorderWidth = 1.75F;
            FullScreenBtn.ForeColor = Color.White;
            FullScreenBtn.Location = new Point(762, 567);
            FullScreenBtn.Name = "FullScreenBtn";
            FullScreenBtn.Size = new Size(370, 52);
            FullScreenBtn.TabIndex = 24;
            FullScreenBtn.Text = "Full Screen: Off";
            FullScreenBtn.UseVisualStyleBackColor = false;
            FullScreenBtn.Visible = false;
            FullScreenBtn.Click += FullScreenBtn_Click;
            // 
            // FrameRateLabel
            // 
            FrameRateLabel.AutoSize = true;
            FrameRateLabel.ForeColor = Color.White;
            FrameRateLabel.Location = new Point(1023, 603);
            FrameRateLabel.Name = "FrameRateLabel";
            FrameRateLabel.Size = new Size(29, 15);
            FrameRateLabel.TabIndex = 25;
            FrameRateLabel.Text = "FPS:";
            FrameRateLabel.Visible = false;
            // 
            // InteractionPictureBox
            // 
            InteractionPictureBox.BackColor = Color.Black;
            InteractionPictureBox.Image = Properties.Resources.mouse_clicker;
            InteractionPictureBox.Location = new Point(913, 263);
            InteractionPictureBox.Name = "InteractionPictureBox";
            InteractionPictureBox.Size = new Size(36, 36);
            InteractionPictureBox.TabIndex = 26;
            InteractionPictureBox.TabStop = false;
            InteractionPictureBox.Visible = false;
            InteractionPictureBox.Click += InteractionPictureBox_Click;
            // 
            // GenericNotificationLabel
            // 
            GenericNotificationLabel.AutoSize = true;
            GenericNotificationLabel.ForeColor = Color.White;
            GenericNotificationLabel.Location = new Point(12, 636);
            GenericNotificationLabel.Name = "GenericNotificationLabel";
            GenericNotificationLabel.Size = new Size(70, 15);
            GenericNotificationLabel.TabIndex = 27;
            GenericNotificationLabel.Text = "Notification";
            GenericNotificationLabel.Visible = false;
            // 
            // FrmRemoteDesktop
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(17, 17, 25);
            ClientSize = new Size(1100, 660);
            Controls.Add(GenericNotificationLabel);
            Controls.Add(InteractionPictureBox);
            Controls.Add(FrameRateLabel);
            Controls.Add(FullScreenBtn);
            Controls.Add(StartWatchingButton);
            Controls.Add(DesktopPictureBox);
            Controls.Add(DisplaysBox);
            Controls.Add(AvailableCamsLabel);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmRemoteDesktop";
            Text = "FrmRemoteDesktop";
            Load += FrmRemoteDesktop_Load;
            ((System.ComponentModel.ISupportInitialize)DesktopPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)InteractionPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private PictureBox DesktopPictureBox;
        private ComboBox DisplaysBox;
        private Label AvailableCamsLabel;
        private FormsAddons.RoundedButton StartWatchingButton;
        private FormsAddons.RoundedButton FullScreenBtn;
        private Label FrameRateLabel;
        private PictureBox InteractionPictureBox;
        private Label GenericNotificationLabel;
    }
}