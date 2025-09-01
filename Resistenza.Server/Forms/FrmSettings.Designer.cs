namespace Resistenza.Server.Forms
{
    partial class FrmSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSettings));
            PortLabel = new Label();
            PortNumericUpDown = new NumericUpDown();
            UsernameTextBox = new TextBox();
            UsernameLabel = new Label();
            PasswordLabel = new Label();
            PasswordTextBox = new TextBox();
            AdditionalOptionsLabel = new Label();
            SoundNotificationCheckbox = new CheckBox();
            AuthFailureCheckbox = new CheckBox();
            StartAtBootCheckbox = new CheckBox();
            EnableLoggingCheckbox = new CheckBox();
            LogFilePathTextbox = new TextBox();
            LogFilePathLabel = new Label();
            InvalidLogPathLabel = new Label();
            InvalidUsernameOrPasswordLabel = new Label();
            pictureBox2 = new PictureBox();
            StartListeningBtn = new FormsAddons.RoundedButton();
            StopListeningBtn = new FormsAddons.RoundedButton();
            mainPanel = new Panel();
            ((System.ComponentModel.ISupportInitialize)PortNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            mainPanel.SuspendLayout();
            SuspendLayout();
            // 
            // PortLabel
            // 
            PortLabel.AutoSize = true;
            PortLabel.ForeColor = Color.WhiteSmoke;
            PortLabel.Location = new Point(76, 56);
            PortLabel.Name = "PortLabel";
            PortLabel.Size = new Size(94, 15);
            PortLabel.TabIndex = 0;
            PortLabel.Text = "Port to listen on:";
            // 
            // PortNumericUpDown
            // 
            PortNumericUpDown.Location = new Point(190, 53);
            PortNumericUpDown.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            PortNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            PortNumericUpDown.Name = "PortNumericUpDown";
            PortNumericUpDown.Size = new Size(120, 23);
            PortNumericUpDown.TabIndex = 1;
            PortNumericUpDown.Value = new decimal(new int[] { 2424, 0, 0, 0 });
            // 
            // UsernameTextBox
            // 
            UsernameTextBox.Location = new Point(188, 89);
            UsernameTextBox.Name = "UsernameTextBox";
            UsernameTextBox.Size = new Size(120, 23);
            UsernameTextBox.TabIndex = 2;
            // 
            // UsernameLabel
            // 
            UsernameLabel.AutoSize = true;
            UsernameLabel.ForeColor = Color.WhiteSmoke;
            UsernameLabel.Location = new Point(92, 92);
            UsernameLabel.Name = "UsernameLabel";
            UsernameLabel.Size = new Size(60, 15);
            UsernameLabel.TabIndex = 3;
            UsernameLabel.Text = "Username";
            // 
            // PasswordLabel
            // 
            PasswordLabel.AutoSize = true;
            PasswordLabel.ForeColor = Color.WhiteSmoke;
            PasswordLabel.Location = new Point(92, 126);
            PasswordLabel.Name = "PasswordLabel";
            PasswordLabel.Size = new Size(57, 15);
            PasswordLabel.TabIndex = 5;
            PasswordLabel.Text = "Password";
            // 
            // PasswordTextBox
            // 
            PasswordTextBox.Location = new Point(188, 123);
            PasswordTextBox.Name = "PasswordTextBox";
            PasswordTextBox.Size = new Size(120, 23);
            PasswordTextBox.TabIndex = 4;
            // 
            // AdditionalOptionsLabel
            // 
            AdditionalOptionsLabel.AutoSize = true;
            AdditionalOptionsLabel.ForeColor = Color.WhiteSmoke;
            AdditionalOptionsLabel.Location = new Point(41, 180);
            AdditionalOptionsLabel.Name = "AdditionalOptionsLabel";
            AdditionalOptionsLabel.Size = new Size(108, 15);
            AdditionalOptionsLabel.TabIndex = 6;
            AdditionalOptionsLabel.Text = "Additional options:";
            // 
            // SoundNotificationCheckbox
            // 
            SoundNotificationCheckbox.AutoSize = true;
            SoundNotificationCheckbox.ForeColor = Color.WhiteSmoke;
            SoundNotificationCheckbox.Location = new Point(47, 206);
            SoundNotificationCheckbox.Name = "SoundNotificationCheckbox";
            SoundNotificationCheckbox.Size = new Size(236, 19);
            SoundNotificationCheckbox.TabIndex = 8;
            SoundNotificationCheckbox.Text = "Sound notification on client connection";
            SoundNotificationCheckbox.UseVisualStyleBackColor = true;
            // 
            // AuthFailureCheckbox
            // 
            AuthFailureCheckbox.AutoSize = true;
            AuthFailureCheckbox.ForeColor = Color.WhiteSmoke;
            AuthFailureCheckbox.Location = new Point(47, 230);
            AuthFailureCheckbox.Name = "AuthFailureCheckbox";
            AuthFailureCheckbox.Size = new Size(236, 19);
            AuthFailureCheckbox.TabIndex = 9;
            AuthFailureCheckbox.Text = "Notify when a client fails authentication";
            AuthFailureCheckbox.UseVisualStyleBackColor = true;
            // 
            // StartAtBootCheckbox
            // 
            StartAtBootCheckbox.AutoSize = true;
            StartAtBootCheckbox.ForeColor = Color.WhiteSmoke;
            StartAtBootCheckbox.Location = new Point(47, 257);
            StartAtBootCheckbox.Name = "StartAtBootCheckbox";
            StartAtBootCheckbox.Size = new Size(221, 19);
            StartAtBootCheckbox.TabIndex = 10;
            StartAtBootCheckbox.Text = "Automatically start the server at boot";
            StartAtBootCheckbox.UseVisualStyleBackColor = true;
            // 
            // EnableLoggingCheckbox
            // 
            EnableLoggingCheckbox.AutoSize = true;
            EnableLoggingCheckbox.ForeColor = Color.WhiteSmoke;
            EnableLoggingCheckbox.Location = new Point(47, 281);
            EnableLoggingCheckbox.Name = "EnableLoggingCheckbox";
            EnableLoggingCheckbox.Size = new Size(105, 19);
            EnableLoggingCheckbox.TabIndex = 11;
            EnableLoggingCheckbox.Text = "Enable logging";
            EnableLoggingCheckbox.UseVisualStyleBackColor = true;
            EnableLoggingCheckbox.CheckedChanged += EnableLogging_CheckChanged;
            // 
            // LogFilePathTextbox
            // 
            LogFilePathTextbox.Enabled = false;
            LogFilePathTextbox.Location = new Point(144, 308);
            LogFilePathTextbox.Name = "LogFilePathTextbox";
            LogFilePathTextbox.Size = new Size(221, 23);
            LogFilePathTextbox.TabIndex = 12;
            // 
            // LogFilePathLabel
            // 
            LogFilePathLabel.AutoSize = true;
            LogFilePathLabel.Enabled = false;
            LogFilePathLabel.ForeColor = Color.WhiteSmoke;
            LogFilePathLabel.Location = new Point(62, 311);
            LogFilePathLabel.Name = "LogFilePathLabel";
            LogFilePathLabel.Size = new Size(76, 15);
            LogFilePathLabel.TabIndex = 13;
            LogFilePathLabel.Text = "Log file path:";
            // 
            // InvalidLogPathLabel
            // 
            InvalidLogPathLabel.AutoSize = true;
            InvalidLogPathLabel.ForeColor = Color.FromArgb(192, 0, 0);
            InvalidLogPathLabel.Location = new Point(170, 334);
            InvalidLogPathLabel.Name = "InvalidLogPathLabel";
            InvalidLogPathLabel.Size = new Size(0, 15);
            InvalidLogPathLabel.TabIndex = 15;
            InvalidLogPathLabel.Visible = false;
            // 
            // InvalidUsernameOrPasswordLabel
            // 
            InvalidUsernameOrPasswordLabel.AutoSize = true;
            InvalidUsernameOrPasswordLabel.ForeColor = Color.FromArgb(192, 0, 0);
            InvalidUsernameOrPasswordLabel.Location = new Point(190, 164);
            InvalidUsernameOrPasswordLabel.Name = "InvalidUsernameOrPasswordLabel";
            InvalidUsernameOrPasswordLabel.Size = new Size(0, 15);
            InvalidUsernameOrPasswordLabel.TabIndex = 16;
            InvalidUsernameOrPasswordLabel.Visible = false;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.FromArgb(30, 32, 37);
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(359, 12);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(27, 24);
            pictureBox2.TabIndex = 18;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // StartListeningBtn
            // 
            StartListeningBtn.Anchor = AnchorStyles.Bottom;
            StartListeningBtn.BackColor = Color.FromArgb(35, 40, 50);
            StartListeningBtn.BorderColor = Color.FromArgb(35, 40, 50);
            StartListeningBtn.BorderDownColor = Color.Empty;
            StartListeningBtn.BorderDownWidth = 0F;
            StartListeningBtn.BorderOverColor = Color.Empty;
            StartListeningBtn.BorderOverWidth = 0F;
            StartListeningBtn.BorderRadius = 50;
            StartListeningBtn.BorderWidth = 1.75F;
            StartListeningBtn.ForeColor = Color.White;
            StartListeningBtn.Location = new Point(74, 352);
            StartListeningBtn.Name = "StartListeningBtn";
            StartListeningBtn.Size = new Size(234, 38);
            StartListeningBtn.TabIndex = 19;
            StartListeningBtn.Text = "Start Listening ";
            StartListeningBtn.UseVisualStyleBackColor = false;
            StartListeningBtn.Click += StartListeningBtn_Click;
            // 
            // StopListeningBtn
            // 
            StopListeningBtn.Anchor = AnchorStyles.Bottom;
            StopListeningBtn.BackColor = Color.FromArgb(35, 40, 50);
            StopListeningBtn.BorderColor = Color.FromArgb(35, 40, 50);
            StopListeningBtn.BorderDownColor = Color.Empty;
            StopListeningBtn.BorderDownWidth = 0F;
            StopListeningBtn.BorderOverColor = Color.Empty;
            StopListeningBtn.BorderOverWidth = 0F;
            StopListeningBtn.BorderRadius = 50;
            StopListeningBtn.BorderWidth = 1.75F;
            StopListeningBtn.ForeColor = Color.White;
            StopListeningBtn.Location = new Point(76, 352);
            StopListeningBtn.Name = "StopListeningBtn";
            StopListeningBtn.Size = new Size(234, 38);
            StopListeningBtn.TabIndex = 20;
            StopListeningBtn.Text = "Stop Listening";
            StopListeningBtn.UseVisualStyleBackColor = false;
            StopListeningBtn.Visible = false;
            StopListeningBtn.Click += StopListeningBtn_Click;
            // 
            // mainPanel
            // 
            mainPanel.Controls.Add(PortLabel);
            mainPanel.Controls.Add(pictureBox2);
            mainPanel.Controls.Add(StopListeningBtn);
            mainPanel.Controls.Add(PortNumericUpDown);
            mainPanel.Controls.Add(StartListeningBtn);
            mainPanel.Controls.Add(UsernameTextBox);
            mainPanel.Controls.Add(UsernameLabel);
            mainPanel.Controls.Add(InvalidUsernameOrPasswordLabel);
            mainPanel.Controls.Add(PasswordTextBox);
            mainPanel.Controls.Add(InvalidLogPathLabel);
            mainPanel.Controls.Add(PasswordLabel);
            mainPanel.Controls.Add(LogFilePathLabel);
            mainPanel.Controls.Add(AdditionalOptionsLabel);
            mainPanel.Controls.Add(LogFilePathTextbox);
            mainPanel.Controls.Add(SoundNotificationCheckbox);
            mainPanel.Controls.Add(EnableLoggingCheckbox);
            mainPanel.Controls.Add(AuthFailureCheckbox);
            mainPanel.Controls.Add(StartAtBootCheckbox);
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(398, 423);
            mainPanel.TabIndex = 21;
            mainPanel.MouseDown += mainPanel_MouseDown;
            // 
            // FrmSettings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 32, 37);
            ClientSize = new Size(398, 423);
            Controls.Add(mainPanel);
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmSettings";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Settings";
            FormClosing += FrmSettings_Closing;
            Load += FrmSettings_Load;
            MouseDown += FrmSettings_MouseDown;
            ((System.ComponentModel.ISupportInitialize)PortNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            mainPanel.ResumeLayout(false);
            mainPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion


        private Label PortLabel;
        private NumericUpDown PortNumericUpDown;
        private TextBox UsernameTextBox;
        private Label UsernameLabel;
        private Label PasswordLabel;
        private TextBox PasswordTextBox;
        private Label AdditionalOptionsLabel;
        private CheckBox SoundNotificationCheckbox;
        private CheckBox AuthFailureCheckbox;
        private CheckBox StartAtBootCheckbox;
        private CheckBox EnableLoggingCheckbox;
        private TextBox LogFilePathTextbox;
        private Label LogFilePathLabel;
        private Label InvalidLogPathLabel;
        private Label InvalidUsernameOrPasswordLabel;
        private PictureBox pictureBox2;
        private FormsAddons.RoundedButton StartListeningBtn;
        private FormsAddons.RoundedButton StopListeningBtn;
        private Panel mainPanel;
    }
}