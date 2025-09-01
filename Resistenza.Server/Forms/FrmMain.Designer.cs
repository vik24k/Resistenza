namespace Resistenza.Server
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            ServerStatusLabel = new Label();
            MainPanel = new Panel();
            BackgroundGridpanel = new Panel();
            StatusLabel = new Label();
            ClientsCountLabel = new Label();
            PaddingPanel = new Panel();
            clientsGrid = new DataGridView();
            blankSpace = new DataGridViewTextBoxColumn();
            ipAddress = new DataGridViewTextBoxColumn();
            operatingSystem = new DataGridViewTextBoxColumn();
            userName = new DataGridViewTextBoxColumn();
            administrator = new DataGridViewTextBoxColumn();
            antivirus = new DataGridViewTextBoxColumn();
            connectedSince = new DataGridViewTextBoxColumn();
            SurveillanceButton = new Button();
            TopPanel = new Panel();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            LookforProcessesTextbox = new TextBox();
            label1 = new Label();
            MainPanel.SuspendLayout();
            BackgroundGridpanel.SuspendLayout();
            PaddingPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)clientsGrid).BeginInit();
            TopPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // ServerStatusLabel
            // 
            ServerStatusLabel.AutoSize = true;
            ServerStatusLabel.ForeColor = SystemColors.Control;
            ServerStatusLabel.Location = new Point(14, 816);
            ServerStatusLabel.Name = "ServerStatusLabel";
            ServerStatusLabel.Size = new Size(101, 20);
            ServerStatusLabel.TabIndex = 3;
            ServerStatusLabel.Text = "Server Status: ";
            // 
            // MainPanel
            // 
            MainPanel.AutoSize = true;
            MainPanel.BackColor = Color.FromArgb(50, 53, 62);
            MainPanel.Controls.Add(BackgroundGridpanel);
            MainPanel.Controls.Add(SurveillanceButton);
            MainPanel.Dock = DockStyle.Fill;
            MainPanel.Location = new Point(0, 0);
            MainPanel.Margin = new Padding(3, 4, 3, 4);
            MainPanel.Name = "MainPanel";
            MainPanel.Size = new Size(1486, 900);
            MainPanel.TabIndex = 2;
            // 
            // BackgroundGridpanel
            // 
            BackgroundGridpanel.BackColor = Color.FromArgb(43, 42, 51);
            BackgroundGridpanel.Controls.Add(StatusLabel);
            BackgroundGridpanel.Controls.Add(ClientsCountLabel);
            BackgroundGridpanel.Controls.Add(PaddingPanel);
            BackgroundGridpanel.Controls.Add(ServerStatusLabel);
            BackgroundGridpanel.Location = new Point(0, 52);
            BackgroundGridpanel.Margin = new Padding(3, 4, 3, 4);
            BackgroundGridpanel.Name = "BackgroundGridpanel";
            BackgroundGridpanel.Size = new Size(1486, 848);
            BackgroundGridpanel.TabIndex = 9;
            // 
            // StatusLabel
            // 
            StatusLabel.AutoSize = true;
            StatusLabel.ForeColor = Color.FromArgb(192, 0, 0);
            StatusLabel.Location = new Point(111, 816);
            StatusLabel.Name = "StatusLabel";
            StatusLabel.Size = new Size(52, 20);
            StatusLabel.TabIndex = 10;
            StatusLabel.Text = "offline";
            // 
            // ClientsCountLabel
            // 
            ClientsCountLabel.AutoSize = true;
            ClientsCountLabel.ForeColor = SystemColors.Control;
            ClientsCountLabel.Location = new Point(1347, 816);
            ClientsCountLabel.Name = "ClientsCountLabel";
            ClientsCountLabel.Size = new Size(141, 20);
            ClientsCountLabel.TabIndex = 9;
            ClientsCountLabel.Text = "Clients connected: 0";
            // 
            // PaddingPanel
            // 
            PaddingPanel.Anchor = AnchorStyles.None;
            PaddingPanel.BackColor = Color.FromArgb(31, 33, 36);
            PaddingPanel.Controls.Add(clientsGrid);
            PaddingPanel.Location = new Point(18, 48);
            PaddingPanel.Margin = new Padding(3, 4, 3, 4);
            PaddingPanel.Name = "PaddingPanel";
            PaddingPanel.Padding = new Padding(3, 4, 3, 4);
            PaddingPanel.Size = new Size(1403, 749);
            PaddingPanel.TabIndex = 8;
            // 
            // clientsGrid
            // 
            clientsGrid.AllowDrop = true;
            clientsGrid.AllowUserToAddRows = false;
            clientsGrid.AllowUserToDeleteRows = false;
            clientsGrid.AllowUserToResizeColumns = false;
            clientsGrid.AllowUserToResizeRows = false;
            clientsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            clientsGrid.BackgroundColor = Color.FromArgb(43, 43, 43);
            clientsGrid.BorderStyle = BorderStyle.None;
            clientsGrid.CellBorderStyle = DataGridViewCellBorderStyle.None;
            clientsGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            clientsGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(43, 43, 43);
            dataGridViewCellStyle1.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(32, 34, 46);
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            clientsGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            clientsGrid.ColumnHeadersHeight = 25;
            clientsGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            clientsGrid.Columns.AddRange(new DataGridViewColumn[] { blankSpace, ipAddress, operatingSystem, userName, administrator, antivirus, connectedSince });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(32, 34, 46);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(35, 33, 75);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            clientsGrid.DefaultCellStyle = dataGridViewCellStyle2;
            clientsGrid.Dock = DockStyle.Fill;
            clientsGrid.EnableHeadersVisualStyles = false;
            clientsGrid.GridColor = Color.White;
            clientsGrid.Location = new Point(3, 4);
            clientsGrid.Name = "clientsGrid";
            clientsGrid.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(32, 34, 46);
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            clientsGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            clientsGrid.RowHeadersVisible = false;
            clientsGrid.RowHeadersWidth = 51;
            clientsGrid.RowTemplate.Height = 29;
            clientsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            clientsGrid.Size = new Size(1397, 741);
            clientsGrid.TabIndex = 7;
            clientsGrid.CellDoubleClick += clientsGrid_CellDoubleClick;
            // 
            // blankSpace
            // 
            blankSpace.FillWeight = 31.976347F;
            blankSpace.HeaderText = "";
            blankSpace.MinimumWidth = 6;
            blankSpace.Name = "blankSpace";
            blankSpace.ReadOnly = true;
            blankSpace.Resizable = DataGridViewTriState.False;
            blankSpace.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // ipAddress
            // 
            ipAddress.FillWeight = 99.6596146F;
            ipAddress.HeaderText = "Public Ip Address:";
            ipAddress.MinimumWidth = 6;
            ipAddress.Name = "ipAddress";
            ipAddress.ReadOnly = true;
            // 
            // operatingSystem
            // 
            operatingSystem.FillWeight = 99.6596146F;
            operatingSystem.HeaderText = "Operating System:";
            operatingSystem.MinimumWidth = 6;
            operatingSystem.Name = "operatingSystem";
            operatingSystem.ReadOnly = true;
            // 
            // userName
            // 
            userName.FillWeight = 99.6596146F;
            userName.HeaderText = "User / Computer:";
            userName.MinimumWidth = 6;
            userName.Name = "userName";
            userName.ReadOnly = true;
            // 
            // administrator
            // 
            administrator.FillWeight = 99.6596146F;
            administrator.HeaderText = "Administrator:";
            administrator.MinimumWidth = 6;
            administrator.Name = "administrator";
            administrator.ReadOnly = true;
            // 
            // antivirus
            // 
            antivirus.FillWeight = 99.6596146F;
            antivirus.HeaderText = "Antivirus:";
            antivirus.MinimumWidth = 6;
            antivirus.Name = "antivirus";
            antivirus.ReadOnly = true;
            // 
            // connectedSince
            // 
            connectedSince.FillWeight = 99.6596146F;
            connectedSince.HeaderText = "Connected Since:";
            connectedSince.MinimumWidth = 6;
            connectedSince.Name = "connectedSince";
            connectedSince.ReadOnly = true;
            // 
            // SurveillanceButton
            // 
            SurveillanceButton.Dock = DockStyle.Top;
            SurveillanceButton.FlatAppearance.BorderSize = 0;
            SurveillanceButton.FlatStyle = FlatStyle.Flat;
            SurveillanceButton.ForeColor = Color.WhiteSmoke;
            SurveillanceButton.Location = new Point(0, 0);
            SurveillanceButton.Margin = new Padding(3, 4, 3, 4);
            SurveillanceButton.Name = "SurveillanceButton";
            SurveillanceButton.Padding = new Padding(11, 0, 0, 0);
            SurveillanceButton.Size = new Size(1486, 67);
            SurveillanceButton.TabIndex = 6;
            SurveillanceButton.Text = "Surveillance";
            SurveillanceButton.TextAlign = ContentAlignment.MiddleLeft;
            SurveillanceButton.UseVisualStyleBackColor = true;
            // 
            // TopPanel
            // 
            TopPanel.BackColor = Color.FromArgb(32, 32, 32);
            TopPanel.Controls.Add(pictureBox1);
            TopPanel.Controls.Add(pictureBox2);
            TopPanel.Controls.Add(LookforProcessesTextbox);
            TopPanel.Controls.Add(label1);
            TopPanel.Dock = DockStyle.Top;
            TopPanel.Location = new Point(0, 0);
            TopPanel.Margin = new Padding(3, 4, 3, 4);
            TopPanel.Name = "TopPanel";
            TopPanel.Size = new Size(1486, 52);
            TopPanel.TabIndex = 6;
            TopPanel.Click += TopPanel_Click;
            TopPanel.MouseDown += panel2_MouseDown;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.FromArgb(30, 32, 37);
            pictureBox1.Image = Properties.Resources.settings_ios;
            pictureBox1.Location = new Point(1403, 11);
            pictureBox1.Margin = new Padding(3, 4, 3, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(31, 32);
            pictureBox1.TabIndex = 20;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.FromArgb(30, 32, 37);
            pictureBox2.Image = Properties.Resources.exit_main;
            pictureBox2.Location = new Point(1441, 11);
            pictureBox2.Margin = new Padding(3, 4, 3, 4);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(31, 32);
            pictureBox2.TabIndex = 19;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // LookforProcessesTextbox
            // 
            LookforProcessesTextbox.BackColor = Color.FromArgb(33, 37, 41);
            LookforProcessesTextbox.BorderStyle = BorderStyle.FixedSingle;
            LookforProcessesTextbox.ForeColor = Color.FromArgb(185, 185, 185);
            LookforProcessesTextbox.Location = new Point(1150, 12);
            LookforProcessesTextbox.Margin = new Padding(3, 4, 3, 4);
            LookforProcessesTextbox.Name = "LookforProcessesTextbox";
            LookforProcessesTextbox.Size = new Size(234, 27);
            LookforProcessesTextbox.TabIndex = 13;
            LookforProcessesTextbox.Text = "   Search";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.CausesValidation = false;
            label1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ButtonHighlight;
            label1.Location = new Point(30, 12);
            label1.Name = "label1";
            label1.Size = new Size(105, 28);
            label1.TabIndex = 0;
            label1.Text = "Clients list";
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(33, 37, 41);
            ClientSize = new Size(1486, 900);
            Controls.Add(TopPanel);
            Controls.Add(MainPanel);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 4, 3, 4);
            Name = "FrmMain";
            Text = "Resistenza - Connected: 0 ";
            FormClosing += FrmMain_FormClosing;
            MainPanel.ResumeLayout(false);
            BackgroundGridpanel.ResumeLayout(false);
            BackgroundGridpanel.PerformLayout();
            PaddingPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)clientsGrid).EndInit();
            TopPanel.ResumeLayout(false);
            TopPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private VScrollBar vScrollBar1;
        private Label ServerStatusLabel;
        private Panel MainPanel;
        private Panel TopPanel;
        private Label label1;
        private TextBox LookforProcessesTextbox;
        private Button SurveillanceButton;
        private PictureBox pictureBox2;
        private Panel PaddingPanel;
        private DataGridView clientsGrid;
        private Panel BackgroundGridpanel;
        private PictureBox pictureBox1;
        private Label ClientsCountLabel;
        private DataGridViewTextBoxColumn blankSpace;
        private DataGridViewTextBoxColumn ipAddress;
        private DataGridViewTextBoxColumn operatingSystem;
        private DataGridViewTextBoxColumn userName;
        private DataGridViewTextBoxColumn administrator;
        private DataGridViewTextBoxColumn antivirus;
        private DataGridViewTextBoxColumn connectedSince;
        private Label StatusLabel;
    }
}