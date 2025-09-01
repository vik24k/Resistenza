namespace Resistenza.Server.Forms
{
    partial class FrmActions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmActions));
            MenuBackgroundPanel = new Panel();
            SystemSubpanel = new Panel();
            TaskManagerBtn = new Button();
            FilesManagerBtn = new Button();
            RemoteShellBtn = new Button();
            SystemInfoFormBtn = new Button();
            SystemBtn = new Button();
            SurveillanceSubpanel = new Panel();
            RemoteDesktopBtn = new Button();
            WebcamBtn = new Button();
            MicrophoneBtn = new Button();
            SurveillanceBtn = new Button();
            TitlePanel = new Panel();
            LogoPicturebox = new PictureBox();
            ClientAddressLabel = new Label();
            MenuLabel = new Label();
            topPanel = new Panel();
            CloseIcon = new PictureBox();
            ActionsPanel = new Panel();
            BackgroundPanel = new Panel();
            MenuBackgroundPanel.SuspendLayout();
            SystemSubpanel.SuspendLayout();
            SurveillanceSubpanel.SuspendLayout();
            TitlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)LogoPicturebox).BeginInit();
            topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)CloseIcon).BeginInit();
            BackgroundPanel.SuspendLayout();
            SuspendLayout();
            // 
            // MenuBackgroundPanel
            // 
            MenuBackgroundPanel.AutoScroll = true;
            MenuBackgroundPanel.BackColor = Color.FromArgb(8, 5, 12);
            MenuBackgroundPanel.Controls.Add(SystemSubpanel);
            MenuBackgroundPanel.Controls.Add(SystemBtn);
            MenuBackgroundPanel.Controls.Add(SurveillanceSubpanel);
            MenuBackgroundPanel.Controls.Add(SurveillanceBtn);
            MenuBackgroundPanel.Controls.Add(TitlePanel);
            MenuBackgroundPanel.Location = new Point(0, 0);
            MenuBackgroundPanel.Margin = new Padding(3, 4, 3, 4);
            MenuBackgroundPanel.Name = "MenuBackgroundPanel";
            MenuBackgroundPanel.Size = new Size(286, 933);
            MenuBackgroundPanel.TabIndex = 2;
            // 
            // SystemSubpanel
            // 
            SystemSubpanel.BackColor = Color.FromArgb(27, 25, 30);
            SystemSubpanel.Controls.Add(TaskManagerBtn);
            SystemSubpanel.Controls.Add(FilesManagerBtn);
            SystemSubpanel.Controls.Add(RemoteShellBtn);
            SystemSubpanel.Controls.Add(SystemInfoFormBtn);
            SystemSubpanel.Dock = DockStyle.Top;
            SystemSubpanel.Location = new Point(0, 452);
            SystemSubpanel.Margin = new Padding(3, 4, 3, 4);
            SystemSubpanel.Name = "SystemSubpanel";
            SystemSubpanel.Size = new Size(286, 220);
            SystemSubpanel.TabIndex = 7;
            SystemSubpanel.Visible = false;
            // 
            // TaskManagerBtn
            // 
            TaskManagerBtn.FlatAppearance.BorderSize = 0;
            TaskManagerBtn.FlatStyle = FlatStyle.Flat;
            TaskManagerBtn.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TaskManagerBtn.ForeColor = Color.WhiteSmoke;
            TaskManagerBtn.Location = new Point(0, 168);
            TaskManagerBtn.Margin = new Padding(3, 4, 3, 4);
            TaskManagerBtn.Name = "TaskManagerBtn";
            TaskManagerBtn.Padding = new Padding(40, 0, 0, 0);
            TaskManagerBtn.Size = new Size(286, 48);
            TaskManagerBtn.TabIndex = 9;
            TaskManagerBtn.Text = "Task Manager";
            TaskManagerBtn.TextAlign = ContentAlignment.MiddleLeft;
            TaskManagerBtn.UseVisualStyleBackColor = true;
            TaskManagerBtn.Click += TaskManagerBtn_Click;
            // 
            // FilesManagerBtn
            // 
            FilesManagerBtn.FlatAppearance.BorderSize = 0;
            FilesManagerBtn.FlatStyle = FlatStyle.Flat;
            FilesManagerBtn.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FilesManagerBtn.ForeColor = Color.WhiteSmoke;
            FilesManagerBtn.Location = new Point(-3, 112);
            FilesManagerBtn.Margin = new Padding(3, 4, 3, 4);
            FilesManagerBtn.Name = "FilesManagerBtn";
            FilesManagerBtn.Padding = new Padding(40, 0, 0, 0);
            FilesManagerBtn.Size = new Size(286, 48);
            FilesManagerBtn.TabIndex = 8;
            FilesManagerBtn.Text = "Files Manager";
            FilesManagerBtn.TextAlign = ContentAlignment.MiddleLeft;
            FilesManagerBtn.UseVisualStyleBackColor = true;
            FilesManagerBtn.Click += FilesManagerBtn_Click;
            // 
            // RemoteShellBtn
            // 
            RemoteShellBtn.FlatAppearance.BorderSize = 0;
            RemoteShellBtn.FlatStyle = FlatStyle.Flat;
            RemoteShellBtn.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            RemoteShellBtn.ForeColor = Color.WhiteSmoke;
            RemoteShellBtn.Location = new Point(0, 56);
            RemoteShellBtn.Margin = new Padding(3, 4, 3, 4);
            RemoteShellBtn.Name = "RemoteShellBtn";
            RemoteShellBtn.Padding = new Padding(40, 0, 0, 0);
            RemoteShellBtn.Size = new Size(286, 48);
            RemoteShellBtn.TabIndex = 7;
            RemoteShellBtn.Text = "Remote Shell";
            RemoteShellBtn.TextAlign = ContentAlignment.MiddleLeft;
            RemoteShellBtn.UseVisualStyleBackColor = true;
            RemoteShellBtn.Click += RemoteShellBtn_Click;
            // 
            // SystemInfoFormBtn
            // 
            SystemInfoFormBtn.FlatAppearance.BorderSize = 0;
            SystemInfoFormBtn.FlatStyle = FlatStyle.Flat;
            SystemInfoFormBtn.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            SystemInfoFormBtn.ForeColor = Color.WhiteSmoke;
            SystemInfoFormBtn.Location = new Point(-3, 0);
            SystemInfoFormBtn.Margin = new Padding(3, 4, 3, 4);
            SystemInfoFormBtn.Name = "SystemInfoFormBtn";
            SystemInfoFormBtn.Padding = new Padding(40, 0, 0, 0);
            SystemInfoFormBtn.Size = new Size(286, 48);
            SystemInfoFormBtn.TabIndex = 6;
            SystemInfoFormBtn.Text = "System Information";
            SystemInfoFormBtn.TextAlign = ContentAlignment.MiddleLeft;
            SystemInfoFormBtn.UseVisualStyleBackColor = true;
            SystemInfoFormBtn.Click += SystemInfoFormBtn_Click;
            // 
            // SystemBtn
            // 
            SystemBtn.Dock = DockStyle.Top;
            SystemBtn.FlatAppearance.BorderSize = 0;
            SystemBtn.FlatStyle = FlatStyle.Flat;
            SystemBtn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            SystemBtn.ForeColor = Color.WhiteSmoke;
            SystemBtn.Location = new Point(0, 385);
            SystemBtn.Margin = new Padding(3, 4, 3, 4);
            SystemBtn.Name = "SystemBtn";
            SystemBtn.Padding = new Padding(11, 0, 0, 0);
            SystemBtn.Size = new Size(286, 67);
            SystemBtn.TabIndex = 6;
            SystemBtn.Tag = "System";
            SystemBtn.Text = "System";
            SystemBtn.TextAlign = ContentAlignment.MiddleLeft;
            SystemBtn.UseVisualStyleBackColor = true;
            SystemBtn.Click += SystemBtn_Click;
            // 
            // SurveillanceSubpanel
            // 
            SurveillanceSubpanel.BackColor = Color.FromArgb(27, 25, 30);
            SurveillanceSubpanel.Controls.Add(RemoteDesktopBtn);
            SurveillanceSubpanel.Controls.Add(WebcamBtn);
            SurveillanceSubpanel.Controls.Add(MicrophoneBtn);
            SurveillanceSubpanel.Dock = DockStyle.Top;
            SurveillanceSubpanel.Location = new Point(0, 226);
            SurveillanceSubpanel.Margin = new Padding(3, 4, 3, 4);
            SurveillanceSubpanel.Name = "SurveillanceSubpanel";
            SurveillanceSubpanel.Size = new Size(286, 159);
            SurveillanceSubpanel.TabIndex = 5;
            SurveillanceSubpanel.Visible = false;
            // 
            // RemoteDesktopBtn
            // 
            RemoteDesktopBtn.FlatAppearance.BorderSize = 0;
            RemoteDesktopBtn.FlatStyle = FlatStyle.Flat;
            RemoteDesktopBtn.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            RemoteDesktopBtn.ForeColor = Color.WhiteSmoke;
            RemoteDesktopBtn.Location = new Point(-3, 112);
            RemoteDesktopBtn.Margin = new Padding(3, 4, 3, 4);
            RemoteDesktopBtn.Name = "RemoteDesktopBtn";
            RemoteDesktopBtn.Padding = new Padding(40, 0, 0, 0);
            RemoteDesktopBtn.Size = new Size(286, 48);
            RemoteDesktopBtn.TabIndex = 8;
            RemoteDesktopBtn.Text = "Remote Desktop";
            RemoteDesktopBtn.TextAlign = ContentAlignment.MiddleLeft;
            RemoteDesktopBtn.UseVisualStyleBackColor = true;
            RemoteDesktopBtn.Click += RemoteDesktopBtn_Click;
            // 
            // WebcamBtn
            // 
            WebcamBtn.FlatAppearance.BorderSize = 0;
            WebcamBtn.FlatStyle = FlatStyle.Flat;
            WebcamBtn.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            WebcamBtn.ForeColor = Color.WhiteSmoke;
            WebcamBtn.Location = new Point(0, 56);
            WebcamBtn.Margin = new Padding(3, 4, 3, 4);
            WebcamBtn.Name = "WebcamBtn";
            WebcamBtn.Padding = new Padding(40, 0, 0, 0);
            WebcamBtn.Size = new Size(286, 48);
            WebcamBtn.TabIndex = 7;
            WebcamBtn.Text = "Webcam";
            WebcamBtn.TextAlign = ContentAlignment.MiddleLeft;
            WebcamBtn.UseVisualStyleBackColor = true;
            WebcamBtn.Click += WebcamBtn_Click;
            // 
            // MicrophoneBtn
            // 
            MicrophoneBtn.FlatAppearance.BorderSize = 0;
            MicrophoneBtn.FlatStyle = FlatStyle.Flat;
            MicrophoneBtn.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MicrophoneBtn.ForeColor = Color.WhiteSmoke;
            MicrophoneBtn.Location = new Point(-3, 0);
            MicrophoneBtn.Margin = new Padding(3, 4, 3, 4);
            MicrophoneBtn.Name = "MicrophoneBtn";
            MicrophoneBtn.Padding = new Padding(40, 0, 0, 0);
            MicrophoneBtn.Size = new Size(286, 48);
            MicrophoneBtn.TabIndex = 6;
            MicrophoneBtn.Text = "Microphone";
            MicrophoneBtn.TextAlign = ContentAlignment.MiddleLeft;
            MicrophoneBtn.UseVisualStyleBackColor = true;
            MicrophoneBtn.Click += MicrophoneBtn_Click;
            // 
            // SurveillanceBtn
            // 
            SurveillanceBtn.Dock = DockStyle.Top;
            SurveillanceBtn.FlatAppearance.BorderSize = 0;
            SurveillanceBtn.FlatStyle = FlatStyle.Flat;
            SurveillanceBtn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            SurveillanceBtn.ForeColor = Color.WhiteSmoke;
            SurveillanceBtn.Location = new Point(0, 159);
            SurveillanceBtn.Margin = new Padding(3, 4, 3, 4);
            SurveillanceBtn.Name = "SurveillanceBtn";
            SurveillanceBtn.Padding = new Padding(11, 0, 0, 0);
            SurveillanceBtn.Size = new Size(286, 67);
            SurveillanceBtn.TabIndex = 4;
            SurveillanceBtn.Text = "Surveillance";
            SurveillanceBtn.TextAlign = ContentAlignment.MiddleLeft;
            SurveillanceBtn.UseVisualStyleBackColor = true;
            SurveillanceBtn.Click += SurveillanceBtn_Click;
            // 
            // TitlePanel
            // 
            TitlePanel.Controls.Add(LogoPicturebox);
            TitlePanel.Controls.Add(ClientAddressLabel);
            TitlePanel.Controls.Add(MenuLabel);
            TitlePanel.Dock = DockStyle.Top;
            TitlePanel.Location = new Point(0, 0);
            TitlePanel.Margin = new Padding(3, 4, 3, 4);
            TitlePanel.Name = "TitlePanel";
            TitlePanel.Size = new Size(286, 159);
            TitlePanel.TabIndex = 2;
            // 
            // LogoPicturebox
            // 
            LogoPicturebox.BackColor = Color.Transparent;
            LogoPicturebox.Image = Properties.Resources.resistenza_logo_transparent;
            LogoPicturebox.Location = new Point(14, 29);
            LogoPicturebox.Margin = new Padding(3, 4, 3, 4);
            LogoPicturebox.Name = "LogoPicturebox";
            LogoPicturebox.Size = new Size(90, 91);
            LogoPicturebox.TabIndex = 3;
            LogoPicturebox.TabStop = false;
            // 
            // ClientAddressLabel
            // 
            ClientAddressLabel.AutoSize = true;
            ClientAddressLabel.ForeColor = Color.White;
            ClientAddressLabel.Location = new Point(138, 75);
            ClientAddressLabel.Name = "ClientAddressLabel";
            ClientAddressLabel.Size = new Size(0, 20);
            ClientAddressLabel.TabIndex = 2;
            // 
            // MenuLabel
            // 
            MenuLabel.Anchor = AnchorStyles.None;
            MenuLabel.AutoSize = true;
            MenuLabel.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            MenuLabel.ForeColor = Color.White;
            MenuLabel.Location = new Point(111, 69);
            MenuLabel.Name = "MenuLabel";
            MenuLabel.Size = new Size(116, 32);
            MenuLabel.TabIndex = 0;
            MenuLabel.Text = "Manager";
            // 
            // topPanel
            // 
            topPanel.BackColor = Color.FromArgb(8, 5, 12);
            topPanel.Controls.Add(CloseIcon);
            topPanel.ForeColor = Color.Black;
            topPanel.Location = new Point(286, 0);
            topPanel.Margin = new Padding(3, 4, 3, 4);
            topPanel.Name = "topPanel";
            topPanel.Size = new Size(1257, 52);
            topPanel.TabIndex = 0;
            topPanel.MouseDown += topPanel_MouseDown;
            // 
            // CloseIcon
            // 
            CloseIcon.BackColor = Color.FromArgb(11, 7, 17);
            CloseIcon.Image = (Image)resources.GetObject("CloseIcon.Image");
            CloseIcon.Location = new Point(1213, 12);
            CloseIcon.Margin = new Padding(3, 4, 3, 4);
            CloseIcon.Name = "CloseIcon";
            CloseIcon.Size = new Size(31, 40);
            CloseIcon.TabIndex = 3;
            CloseIcon.TabStop = false;
            // 
            // ActionsPanel
            // 
            ActionsPanel.Location = new Point(286, 52);
            ActionsPanel.Margin = new Padding(3, 4, 3, 4);
            ActionsPanel.Name = "ActionsPanel";
            ActionsPanel.Size = new Size(1257, 880);
            ActionsPanel.TabIndex = 3;
            // 
            // BackgroundPanel
            // 
            BackgroundPanel.Controls.Add(MenuBackgroundPanel);
            BackgroundPanel.Controls.Add(ActionsPanel);
            BackgroundPanel.Controls.Add(topPanel);
            BackgroundPanel.Dock = DockStyle.Fill;
            BackgroundPanel.Location = new Point(0, 0);
            BackgroundPanel.Margin = new Padding(3, 4, 3, 4);
            BackgroundPanel.Name = "BackgroundPanel";
            BackgroundPanel.Size = new Size(1543, 933);
            BackgroundPanel.TabIndex = 4;
            // 
            // FrmActions
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.White;
            ClientSize = new Size(1543, 933);
            Controls.Add(BackgroundPanel);
            ForeColor = Color.Black;
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmActions";
            Load += FrmActions_Load;
            MenuBackgroundPanel.ResumeLayout(false);
            SystemSubpanel.ResumeLayout(false);
            SurveillanceSubpanel.ResumeLayout(false);
            TitlePanel.ResumeLayout(false);
            TitlePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)LogoPicturebox).EndInit();
            topPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)CloseIcon).EndInit();
            BackgroundPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel MenuBackgroundPanel;
        private Label MenuLabel;
        private Panel TitlePanel;
        private Panel topPanel;
        private PictureBox CloseIcon;
        private Panel ActionsPanel;
        private Panel BackgroundPanel;
        private Label ClientAddressLabel;
        private PictureBox LogoPicturebox;
        private Button SurveillanceBtn;
        private Panel SurveillanceSubpanel;
        private Button MicrophoneBtn;
        private Button RemoteDesktopBtn;
        private Button WebcamBtn;
        private Panel SystemSubpanel;
        private Button FilesManagerBtn;
        private Button RemoteShellBtn;
        private Button SystemInfoFormBtn;
        private Button SystemBtn;
        private Button TaskManagerBtn;
    }
}