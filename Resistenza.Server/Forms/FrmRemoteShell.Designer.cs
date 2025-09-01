namespace Resistenza.Server.Forms
{
    partial class FrmRemoteShell
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
            BackgroundPanel = new Panel();
            commandTextbox = new TextBox();
            clientUsernameLabel = new Label();
            BackgroundPanel.SuspendLayout();
            SuspendLayout();
            // 
            // BackgroundPanel
            // 
            BackgroundPanel.AutoScroll = true;
            BackgroundPanel.BackColor = SystemColors.ActiveCaptionText;
            BackgroundPanel.Controls.Add(commandTextbox);
            BackgroundPanel.Controls.Add(clientUsernameLabel);
            BackgroundPanel.Dock = DockStyle.Fill;
            BackgroundPanel.Location = new Point(0, 0);
            BackgroundPanel.Margin = new Padding(3, 2, 3, 2);
            BackgroundPanel.Name = "BackgroundPanel";
            BackgroundPanel.Size = new Size(1100, 660);
            BackgroundPanel.TabIndex = 0;
            BackgroundPanel.TabStop = true;
            // 
            // commandTextbox
            // 
            commandTextbox.BackColor = Color.Black;
            commandTextbox.BorderStyle = BorderStyle.None;
            commandTextbox.ForeColor = Color.Green;
            commandTextbox.HideSelection = false;
            commandTextbox.Location = new Point(228, 32);
            commandTextbox.Margin = new Padding(3, 2, 3, 2);
            commandTextbox.Name = "commandTextbox";
            commandTextbox.Size = new Size(84, 16);
            commandTextbox.TabIndex = 1;
            commandTextbox.TextChanged += commandTextbox_TextChanged;
            // 
            // clientUsernameLabel
            // 
            clientUsernameLabel.AutoSize = true;
            clientUsernameLabel.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold);
            clientUsernameLabel.ForeColor = Color.Green;
            clientUsernameLabel.Location = new Point(10, 20);
            clientUsernameLabel.Name = "clientUsernameLabel";
            clientUsernameLabel.Size = new Size(0, 15);
            clientUsernameLabel.TabIndex = 0;
            // 
            // FrmRemoteShell
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(1100, 660);
            Controls.Add(BackgroundPanel);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmRemoteShell";
            BackgroundPanel.ResumeLayout(false);
            BackgroundPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel BackgroundPanel;
        private Label clientUsernameLabel;
        private TextBox commandTextbox;
    }
}