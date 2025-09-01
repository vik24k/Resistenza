namespace Resistenza.Server.Forms
{
    partial class FrmTaskManager
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTaskManager));
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            ProcessesLabel = new Label();
            BorderPanel = new Panel();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            ProcessesGrid = new DataGridView();
            space = new DataGridViewTextBoxColumn();
            ArrowColumn = new DataGridViewImageColumn();
            LocalIconColumn = new DataGridViewImageColumn();
            ProcessNameColumn = new DataGridViewTextBoxColumn();
            PIDColumn = new DataGridViewTextBoxColumn();
            Memory = new DataGridViewTextBoxColumn();
            Cpu = new DataGridViewTextBoxColumn();
            ProcessNameLabel = new Label();
            PIDLabel = new Label();
            MemoryLabel = new Label();
            CpuLabel = new Label();
            EndTaskButton = new Button();
            RunNewTaskButton = new Button();
            LookforProcessesTextbox = new TextBox();
            pictureBox2 = new PictureBox();
            BorderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ProcessesGrid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // ProcessesLabel
            // 
            ProcessesLabel.AutoSize = true;
            ProcessesLabel.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ProcessesLabel.ForeColor = Color.White;
            ProcessesLabel.Location = new Point(12, 35);
            ProcessesLabel.Name = "ProcessesLabel";
            ProcessesLabel.Size = new Size(141, 37);
            ProcessesLabel.TabIndex = 0;
            ProcessesLabel.Text = "Processes";
            // 
            // BorderPanel
            // 
            BorderPanel.BackColor = Color.FromArgb(58, 58, 58);
            BorderPanel.Controls.Add(pictureBox1);
            BorderPanel.Controls.Add(label1);
            BorderPanel.Controls.Add(ProcessesGrid);
            BorderPanel.Location = new Point(0, 136);
            BorderPanel.Name = "BorderPanel";
            BorderPanel.Padding = new Padding(1);
            BorderPanel.Size = new Size(1100, 529);
            BorderPanel.TabIndex = 2;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.go_home;
            pictureBox1.Location = new Point(925, -54);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(98, 48);
            pictureBox1.TabIndex = 13;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(4, -16);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 4;
            label1.Text = "label1";
            // 
            // ProcessesGrid
            // 
            ProcessesGrid.AllowDrop = true;
            ProcessesGrid.AllowUserToAddRows = false;
            ProcessesGrid.AllowUserToDeleteRows = false;
            ProcessesGrid.AllowUserToResizeColumns = false;
            ProcessesGrid.AllowUserToResizeRows = false;
            ProcessesGrid.BackgroundColor = Color.FromArgb(25, 25, 25);
            ProcessesGrid.BorderStyle = BorderStyle.None;
            ProcessesGrid.CellBorderStyle = DataGridViewCellBorderStyle.None;
            ProcessesGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            ProcessesGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(25, 25, 25);
            dataGridViewCellStyle1.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.Gainsboro;
            dataGridViewCellStyle1.Padding = new Padding(0, 0, 0, 10);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(25, 25, 25);
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            ProcessesGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            ProcessesGrid.ColumnHeadersHeight = 4;
            ProcessesGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            ProcessesGrid.Columns.AddRange(new DataGridViewColumn[] { space, ArrowColumn, LocalIconColumn, ProcessNameColumn, PIDColumn, Memory, Cpu });
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(25, 25, 25);
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = Color.White;
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(39, 39, 39);
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            ProcessesGrid.DefaultCellStyle = dataGridViewCellStyle4;
            ProcessesGrid.Dock = DockStyle.Fill;
            ProcessesGrid.EnableHeadersVisualStyles = false;
            ProcessesGrid.GridColor = Color.White;
            ProcessesGrid.Location = new Point(1, 1);
            ProcessesGrid.Margin = new Padding(3, 2, 3, 2);
            ProcessesGrid.MultiSelect = false;
            ProcessesGrid.Name = "ProcessesGrid";
            ProcessesGrid.ReadOnly = true;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = SystemColors.Control;
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle5.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(32, 34, 46);
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            ProcessesGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            ProcessesGrid.RowHeadersVisible = false;
            ProcessesGrid.RowHeadersWidth = 51;
            ProcessesGrid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            ProcessesGrid.RowTemplate.Height = 29;
            ProcessesGrid.ScrollBars = ScrollBars.None;
            ProcessesGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ProcessesGrid.Size = new Size(1098, 527);
            ProcessesGrid.TabIndex = 3;
            ProcessesGrid.CellClick += ProcessesGrid_CellClick;
            // 
            // space
            // 
            space.HeaderText = "space";
            space.Name = "space";
            space.ReadOnly = true;
            space.Width = 10;
            // 
            // ArrowColumn
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.Transparent;
            dataGridViewCellStyle2.NullValue = resources.GetObject("dataGridViewCellStyle2.NullValue");
            ArrowColumn.DefaultCellStyle = dataGridViewCellStyle2;
            ArrowColumn.HeaderText = "ArrowIcon";
            ArrowColumn.Name = "ArrowColumn";
            ArrowColumn.ReadOnly = true;
            ArrowColumn.Resizable = DataGridViewTriState.False;
            ArrowColumn.Width = 20;
            // 
            // LocalIconColumn
            // 
            LocalIconColumn.FillWeight = 11.7766495F;
            LocalIconColumn.HeaderText = "";
            LocalIconColumn.MinimumWidth = 20;
            LocalIconColumn.Name = "LocalIconColumn";
            LocalIconColumn.ReadOnly = true;
            LocalIconColumn.Width = 40;
            // 
            // ProcessNameColumn
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            ProcessNameColumn.DefaultCellStyle = dataGridViewCellStyle3;
            ProcessNameColumn.FillWeight = 104.223351F;
            ProcessNameColumn.HeaderText = "Name";
            ProcessNameColumn.MinimumWidth = 6;
            ProcessNameColumn.Name = "ProcessNameColumn";
            ProcessNameColumn.ReadOnly = true;
            ProcessNameColumn.Width = 300;
            // 
            // PIDColumn
            // 
            PIDColumn.HeaderText = "PID";
            PIDColumn.Name = "PIDColumn";
            PIDColumn.ReadOnly = true;
            PIDColumn.Width = 200;
            // 
            // Memory
            // 
            Memory.HeaderText = "Memory";
            Memory.Name = "Memory";
            Memory.ReadOnly = true;
            Memory.Resizable = DataGridViewTriState.False;
            Memory.Width = 200;
            // 
            // Cpu
            // 
            Cpu.HeaderText = "Cpu";
            Cpu.Name = "Cpu";
            Cpu.ReadOnly = true;
            // 
            // ProcessNameLabel
            // 
            ProcessNameLabel.AutoSize = true;
            ProcessNameLabel.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ProcessNameLabel.Location = new Point(12, 113);
            ProcessNameLabel.Name = "ProcessNameLabel";
            ProcessNameLabel.Size = new Size(39, 15);
            ProcessNameLabel.TabIndex = 5;
            ProcessNameLabel.Text = "Name";
            // 
            // PIDLabel
            // 
            PIDLabel.AutoSize = true;
            PIDLabel.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            PIDLabel.Location = new Point(340, 113);
            PIDLabel.Name = "PIDLabel";
            PIDLabel.Size = new Size(30, 15);
            PIDLabel.TabIndex = 6;
            PIDLabel.Text = "PID ";
            // 
            // MemoryLabel
            // 
            MemoryLabel.AutoSize = true;
            MemoryLabel.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            MemoryLabel.Location = new Point(540, 113);
            MemoryLabel.Name = "MemoryLabel";
            MemoryLabel.Size = new Size(52, 15);
            MemoryLabel.TabIndex = 7;
            MemoryLabel.Text = "Memory";
            // 
            // CpuLabel
            // 
            CpuLabel.AutoSize = true;
            CpuLabel.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            CpuLabel.Location = new Point(740, 113);
            CpuLabel.Name = "CpuLabel";
            CpuLabel.Size = new Size(29, 15);
            CpuLabel.TabIndex = 8;
            CpuLabel.Text = "CPU";
            // 
            // EndTaskButton
            // 
            EndTaskButton.FlatAppearance.BorderSize = 0;
            EndTaskButton.FlatStyle = FlatStyle.Flat;
            EndTaskButton.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            EndTaskButton.ForeColor = Color.WhiteSmoke;
            EndTaskButton.Image = (Image)resources.GetObject("EndTaskButton.Image");
            EndTaskButton.ImageAlign = ContentAlignment.MiddleLeft;
            EndTaskButton.Location = new Point(606, 35);
            EndTaskButton.Name = "EndTaskButton";
            EndTaskButton.Padding = new Padding(10, 0, 0, 0);
            EndTaskButton.Size = new Size(135, 50);
            EndTaskButton.TabIndex = 10;
            EndTaskButton.Text = "End task";
            EndTaskButton.UseVisualStyleBackColor = true;
            EndTaskButton.Click += EndTaskButton_Click;
            // 
            // RunNewTaskButton
            // 
            RunNewTaskButton.FlatAppearance.BorderSize = 0;
            RunNewTaskButton.FlatStyle = FlatStyle.Flat;
            RunNewTaskButton.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            RunNewTaskButton.ForeColor = Color.WhiteSmoke;
            RunNewTaskButton.Image = Properties.Resources.run_new_task;
            RunNewTaskButton.ImageAlign = ContentAlignment.MiddleLeft;
            RunNewTaskButton.Location = new Point(376, 35);
            RunNewTaskButton.Name = "RunNewTaskButton";
            RunNewTaskButton.Padding = new Padding(10, 0, 0, 0);
            RunNewTaskButton.Size = new Size(168, 50);
            RunNewTaskButton.TabIndex = 11;
            RunNewTaskButton.Text = "Run new task";
            RunNewTaskButton.UseVisualStyleBackColor = true;
            RunNewTaskButton.Click += button1_Click;
            // 
            // LookforProcessesTextbox
            // 
            LookforProcessesTextbox.BackColor = Color.FromArgb(50, 50, 50);
            LookforProcessesTextbox.BorderStyle = BorderStyle.None;
            LookforProcessesTextbox.ForeColor = Color.FromArgb(185, 185, 185);
            LookforProcessesTextbox.Location = new Point(883, 114);
            LookforProcessesTextbox.Name = "LookforProcessesTextbox";
            LookforProcessesTextbox.Size = new Size(205, 16);
            LookforProcessesTextbox.TabIndex = 12;
            LookforProcessesTextbox.Text = "Type a name or PID for the search";
            LookforProcessesTextbox.TextAlign = HorizontalAlignment.Center;
            LookforProcessesTextbox.MouseClick += LookforProcessesTextbox_MouseClick;
            LookforProcessesTextbox.TextChanged += LookforProcessesTextbox_TextChanged;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.cross;
            pictureBox2.Location = new Point(859, 114);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(18, 20);
            pictureBox2.TabIndex = 13;
            pictureBox2.TabStop = false;
            pictureBox2.Visible = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // FrmTaskManager
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.FromArgb(25, 25, 25);
            ClientSize = new Size(1100, 660);
            Controls.Add(pictureBox2);
            Controls.Add(LookforProcessesTextbox);
            Controls.Add(RunNewTaskButton);
            Controls.Add(EndTaskButton);
            Controls.Add(CpuLabel);
            Controls.Add(MemoryLabel);
            Controls.Add(PIDLabel);
            Controls.Add(ProcessNameLabel);
            Controls.Add(BorderPanel);
            Controls.Add(ProcessesLabel);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmTaskManager";
            Text = "FrmTaskManager";
            Load += FrmTaskManager_Load;
            BorderPanel.ResumeLayout(false);
            BorderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)ProcessesGrid).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label ProcessesLabel;
        private Panel BorderPanel;
        private DataGridView ProcessesGrid;
        private Label label1;
        private Label ProcessNameLabel;
        private Label PIDLabel;
        private Label MemoryLabel;
        private Label CpuLabel;
        private Button EndTaskButton;
        private Button RunNewTaskButton;
        private TextBox LookforProcessesTextbox;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn space;
        private DataGridViewImageColumn ArrowColumn;
        private DataGridViewImageColumn LocalIconColumn;
        private DataGridViewTextBoxColumn ProcessNameColumn;
        private DataGridViewTextBoxColumn PIDColumn;
        private DataGridViewTextBoxColumn Memory;
        private DataGridViewTextBoxColumn Cpu;
    }
}