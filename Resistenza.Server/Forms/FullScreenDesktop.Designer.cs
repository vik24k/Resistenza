namespace Resistenza.Server.Forms
{
    partial class FullScreenDesktop
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
            saveFileDialog1 = new SaveFileDialog();
            FramePictureBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)FramePictureBox).BeginInit();
            SuspendLayout();
            // 
            // FramePictureBox
            // 
            FramePictureBox.BackColor = SystemColors.ActiveCaptionText;
            FramePictureBox.Dock = DockStyle.Fill;
            FramePictureBox.Location = new Point(0, 0);
            FramePictureBox.Name = "FramePictureBox";
            FramePictureBox.Size = new Size(800, 450);
            FramePictureBox.TabIndex = 1;
            FramePictureBox.TabStop = false;
            // 
            // FullScreenDesktop
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(FramePictureBox);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Name = "FullScreenDesktop";
            Text = "FullScreenDesktop";
            ((System.ComponentModel.ISupportInitialize)FramePictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SaveFileDialog saveFileDialog1;
        private PictureBox FramePictureBox;
    }
}