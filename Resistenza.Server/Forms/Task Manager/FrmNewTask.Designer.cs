namespace Resistenza.Server.Forms.Task_Manager
{
    partial class FrmNewTask
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmNewTask));
            pictureBox1 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            TaskTextbox = new TextBox();
            StartCommandButton = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.White;
            pictureBox1.Image = Properties.Resources.system_run_big;
            pictureBox1.Location = new Point(14, 37);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(39, 26);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(59, 33);
            label1.Name = "label1";
            label1.Size = new Size(271, 30);
            label1.TabIndex = 1;
            label1.Text = "Type the name of a program, folder, document, or\r\nInternet resource, and it will be open for you.\r\n";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 90);
            label2.Name = "label2";
            label2.Size = new Size(39, 15);
            label2.TabIndex = 2;
            label2.Text = "Open:";
            // 
            // TaskTextbox
            // 
            TaskTextbox.Location = new Point(59, 87);
            TaskTextbox.Name = "TaskTextbox";
            TaskTextbox.Size = new Size(299, 23);
            TaskTextbox.TabIndex = 3;
            // 
            // StartCommandButton
            // 
            StartCommandButton.Location = new Point(283, 130);
            StartCommandButton.Name = "StartCommandButton";
            StartCommandButton.Size = new Size(75, 23);
            StartCommandButton.TabIndex = 4;
            StartCommandButton.Text = "OK";
            StartCommandButton.UseVisualStyleBackColor = true;
            StartCommandButton.Click += StartCommandButton_Click;
            // 
            // FrmNewTask
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(370, 165);
            Controls.Add(StartCommandButton);
            Controls.Add(TaskTextbox);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmNewTask";
            Text = "Run";
            FormClosed += FrmNewTask_FormClosed;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label label1;
        private Label label2;
        private TextBox TaskTextbox;
        private Button StartCommandButton;
    }
}