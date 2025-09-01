namespace Resistenza.Server.Forms
{
    partial class FrmFileManager
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
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            LocalFilesTextBox = new TextBox();
            LocalFilesGrid = new DataGridView();
            LocalIconColumn = new DataGridViewImageColumn();
            fileNameColumn = new DataGridViewTextBoxColumn();
            fileSizeColumn = new DataGridViewTextBoxColumn();
            LastChangeColumn = new DataGridViewTextBoxColumn();
            RemoteFilePathTextbox = new TextBox();
            FileBeingProcessedLabel = new Label();
            RemoteFilesUndoIcon = new PictureBox();
            RefreshRemoteFilesIcon = new PictureBox();
            HardDriveIcon = new PictureBox();
            RemoteFilesystemIcon = new PictureBox();
            LocalFilesParentDirIcon = new PictureBox();
            LocalEmptyFolderLabel = new Label();
            CancelRemoteOperationIcon = new PictureBox();
            RemoteFilesGrid = new DataGridView();
            dataGridViewImageColumn1 = new DataGridViewImageColumn();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            RemoteEmptyFolderLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)LocalFilesGrid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RemoteFilesUndoIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RefreshRemoteFilesIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)HardDriveIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RemoteFilesystemIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)LocalFilesParentDirIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CancelRemoteOperationIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RemoteFilesGrid).BeginInit();
            SuspendLayout();
            // 
            // LocalFilesTextBox
            // 
            LocalFilesTextBox.BackColor = Color.FromArgb(32, 34, 46);
            LocalFilesTextBox.BorderStyle = BorderStyle.FixedSingle;
            LocalFilesTextBox.ForeColor = Color.White;
            LocalFilesTextBox.Location = new Point(186, 147);
            LocalFilesTextBox.Name = "LocalFilesTextBox";
            LocalFilesTextBox.Size = new Size(256, 27);
            LocalFilesTextBox.TabIndex = 0;
            LocalFilesTextBox.TextChanged += LocalFilesTextBox_TextChanged;
            LocalFilesTextBox.KeyDown += LocalFilesTextBox_KeyDown;
            LocalFilesTextBox.KeyPress += localPathTextbox_KeyPress;
            // 
            // LocalFilesGrid
            // 
            LocalFilesGrid.AllowDrop = true;
            LocalFilesGrid.AllowUserToAddRows = false;
            LocalFilesGrid.AllowUserToDeleteRows = false;
            LocalFilesGrid.AllowUserToResizeColumns = false;
            LocalFilesGrid.AllowUserToResizeRows = false;
            LocalFilesGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            LocalFilesGrid.BackgroundColor = Color.FromArgb(32, 34, 46);
            LocalFilesGrid.CellBorderStyle = DataGridViewCellBorderStyle.None;
            LocalFilesGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            LocalFilesGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.FromArgb(32, 34, 46);
            dataGridViewCellStyle6.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            dataGridViewCellStyle6.ForeColor = Color.White;
            dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(32, 34, 46);
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            LocalFilesGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            LocalFilesGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            LocalFilesGrid.Columns.AddRange(new DataGridViewColumn[] { LocalIconColumn, fileNameColumn, fileSizeColumn, LastChangeColumn });
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = Color.FromArgb(32, 34, 46);
            dataGridViewCellStyle7.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle7.ForeColor = Color.White;
            dataGridViewCellStyle7.SelectionBackColor = Color.FromArgb(35, 33, 75);
            dataGridViewCellStyle7.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.False;
            LocalFilesGrid.DefaultCellStyle = dataGridViewCellStyle7;
            LocalFilesGrid.EnableHeadersVisualStyles = false;
            LocalFilesGrid.GridColor = Color.White;
            LocalFilesGrid.Location = new Point(14, 205);
            LocalFilesGrid.Name = "LocalFilesGrid";
            LocalFilesGrid.ReadOnly = true;
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = SystemColors.Control;
            dataGridViewCellStyle8.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle8.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = Color.FromArgb(32, 34, 46);
            dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.True;
            LocalFilesGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            LocalFilesGrid.RowHeadersVisible = false;
            LocalFilesGrid.RowHeadersWidth = 51;
            LocalFilesGrid.RowTemplate.Height = 29;
            LocalFilesGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            LocalFilesGrid.Size = new Size(606, 533);
            LocalFilesGrid.TabIndex = 2;
            LocalFilesGrid.CellContentClick += localFilesGrid_CellContentClick_1;
            LocalFilesGrid.CellContentDoubleClick += localFilesGrid_CellContentDoubleClick;
            LocalFilesGrid.CellDoubleClick += localFilesGrid_CellDoubleClick;
            LocalFilesGrid.CellEndEdit += localFilesGrid_CellEndEdit;
            LocalFilesGrid.CellFormatting += LocalFilesGrid_CellFormatting;
            LocalFilesGrid.CellPainting += LocalFilesGrid_CellPainting;
            LocalFilesGrid.DragDrop += localFilesGrid_DragDrop;
            LocalFilesGrid.DragEnter += localFilesGrid_DragEnter;
            LocalFilesGrid.DragOver += localFilesGrid_DragOver;
            LocalFilesGrid.MouseClick += localFilesGrid_MouseClick;
            LocalFilesGrid.MouseDown += localFilesGrid_MouseDown;
            LocalFilesGrid.MouseMove += localFilesGrid_MouseMove;
            // 
            // LocalIconColumn
            // 
            LocalIconColumn.FillWeight = 16F;
            LocalIconColumn.HeaderText = "";
            LocalIconColumn.MinimumWidth = 6;
            LocalIconColumn.Name = "LocalIconColumn";
            LocalIconColumn.ReadOnly = true;
            // 
            // fileNameColumn
            // 
            fileNameColumn.HeaderText = "File Name:";
            fileNameColumn.MinimumWidth = 6;
            fileNameColumn.Name = "fileNameColumn";
            fileNameColumn.ReadOnly = true;
            // 
            // fileSizeColumn
            // 
            fileSizeColumn.HeaderText = "Size:";
            fileSizeColumn.MinimumWidth = 6;
            fileSizeColumn.Name = "fileSizeColumn";
            fileSizeColumn.ReadOnly = true;
            // 
            // LastChangeColumn
            // 
            LastChangeColumn.HeaderText = "Last change:";
            LastChangeColumn.MinimumWidth = 6;
            LastChangeColumn.Name = "LastChangeColumn";
            LastChangeColumn.ReadOnly = true;
            // 
            // RemoteFilePathTextbox
            // 
            RemoteFilePathTextbox.BackColor = Color.FromArgb(32, 34, 46);
            RemoteFilePathTextbox.BorderStyle = BorderStyle.FixedSingle;
            RemoteFilePathTextbox.ForeColor = Color.White;
            RemoteFilePathTextbox.Location = new Point(815, 147);
            RemoteFilePathTextbox.Name = "RemoteFilePathTextbox";
            RemoteFilePathTextbox.Size = new Size(256, 27);
            RemoteFilePathTextbox.TabIndex = 6;
            RemoteFilePathTextbox.KeyPress += RemoteFilePathTextbox_KeyPress;
            // 
            // FileBeingProcessedLabel
            // 
            FileBeingProcessedLabel.AutoSize = true;
            FileBeingProcessedLabel.ForeColor = Color.White;
            FileBeingProcessedLabel.Location = new Point(14, 753);
            FileBeingProcessedLabel.Name = "FileBeingProcessedLabel";
            FileBeingProcessedLabel.Size = new Size(0, 20);
            FileBeingProcessedLabel.TabIndex = 12;
            FileBeingProcessedLabel.Visible = false;
            // 
            // RemoteFilesUndoIcon
            // 
            RemoteFilesUndoIcon.Image = Properties.Resources.go_last_rtl;
            RemoteFilesUndoIcon.Location = new Point(1078, 147);
            RemoteFilesUndoIcon.Margin = new Padding(3, 4, 3, 4);
            RemoteFilesUndoIcon.Name = "RemoteFilesUndoIcon";
            RemoteFilesUndoIcon.Size = new Size(29, 33);
            RemoteFilesUndoIcon.TabIndex = 14;
            RemoteFilesUndoIcon.TabStop = false;
            RemoteFilesUndoIcon.Click += RemoteFilesUndoIcon_Click;
            // 
            // RefreshRemoteFilesIcon
            // 
            RefreshRemoteFilesIcon.Image = Properties.Resources.view_refresh;
            RefreshRemoteFilesIcon.Location = new Point(1113, 147);
            RefreshRemoteFilesIcon.Margin = new Padding(3, 4, 3, 4);
            RefreshRemoteFilesIcon.Name = "RefreshRemoteFilesIcon";
            RefreshRemoteFilesIcon.Size = new Size(30, 39);
            RefreshRemoteFilesIcon.TabIndex = 15;
            RefreshRemoteFilesIcon.TabStop = false;
            RefreshRemoteFilesIcon.Click += RefreshRemoteFilesIcon_Click;
            // 
            // HardDriveIcon
            // 
            HardDriveIcon.Image = Properties.Resources.fixed_drive;
            HardDriveIcon.Location = new Point(141, 132);
            HardDriveIcon.Margin = new Padding(3, 4, 3, 4);
            HardDriveIcon.Name = "HardDriveIcon";
            HardDriveIcon.Size = new Size(39, 47);
            HardDriveIcon.TabIndex = 16;
            HardDriveIcon.TabStop = false;
            HardDriveIcon.Click += HardDriveIcon_Click;
            // 
            // RemoteFilesystemIcon
            // 
            RemoteFilesystemIcon.Image = Properties.Resources.networkunit;
            RemoteFilesystemIcon.Location = new Point(769, 132);
            RemoteFilesystemIcon.Margin = new Padding(3, 4, 3, 4);
            RemoteFilesystemIcon.Name = "RemoteFilesystemIcon";
            RemoteFilesystemIcon.Size = new Size(39, 48);
            RemoteFilesystemIcon.TabIndex = 17;
            RemoteFilesystemIcon.TabStop = false;
            RemoteFilesystemIcon.Click += RemoteFilesystemIcon_Click;
            // 
            // LocalFilesParentDirIcon
            // 
            LocalFilesParentDirIcon.Image = Properties.Resources.go_last_rtl;
            LocalFilesParentDirIcon.Location = new Point(449, 147);
            LocalFilesParentDirIcon.Margin = new Padding(3, 4, 3, 4);
            LocalFilesParentDirIcon.Name = "LocalFilesParentDirIcon";
            LocalFilesParentDirIcon.Size = new Size(27, 31);
            LocalFilesParentDirIcon.TabIndex = 18;
            LocalFilesParentDirIcon.TabStop = false;
            LocalFilesParentDirIcon.Click += LocalFilesParentDirIcon_Click;
            // 
            // LocalEmptyFolderLabel
            // 
            LocalEmptyFolderLabel.AutoSize = true;
            LocalEmptyFolderLabel.BackColor = Color.FromArgb(32, 34, 46);
            LocalEmptyFolderLabel.ForeColor = Color.White;
            LocalEmptyFolderLabel.Location = new Point(251, 273);
            LocalEmptyFolderLabel.Name = "LocalEmptyFolderLabel";
            LocalEmptyFolderLabel.Size = new Size(137, 20);
            LocalEmptyFolderLabel.TabIndex = 20;
            LocalEmptyFolderLabel.Text = "The folder is empty";
            LocalEmptyFolderLabel.Visible = false;
            // 
            // CancelRemoteOperationIcon
            // 
            CancelRemoteOperationIcon.Image = Properties.Resources.process_stop;
            CancelRemoteOperationIcon.Location = new Point(1150, 147);
            CancelRemoteOperationIcon.Margin = new Padding(3, 4, 3, 4);
            CancelRemoteOperationIcon.Name = "CancelRemoteOperationIcon";
            CancelRemoteOperationIcon.Size = new Size(34, 39);
            CancelRemoteOperationIcon.TabIndex = 22;
            CancelRemoteOperationIcon.TabStop = false;
            CancelRemoteOperationIcon.Click += CancelRemoteOperationIcon_Click;
            // 
            // RemoteFilesGrid
            // 
            RemoteFilesGrid.AllowDrop = true;
            RemoteFilesGrid.AllowUserToAddRows = false;
            RemoteFilesGrid.AllowUserToDeleteRows = false;
            RemoteFilesGrid.AllowUserToResizeColumns = false;
            RemoteFilesGrid.AllowUserToResizeRows = false;
            RemoteFilesGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            RemoteFilesGrid.BackgroundColor = Color.FromArgb(32, 34, 46);
            RemoteFilesGrid.CellBorderStyle = DataGridViewCellBorderStyle.None;
            RemoteFilesGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            RemoteFilesGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.BottomLeft;
            dataGridViewCellStyle9.BackColor = Color.FromArgb(32, 34, 46);
            dataGridViewCellStyle9.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            dataGridViewCellStyle9.ForeColor = Color.White;
            dataGridViewCellStyle9.SelectionBackColor = Color.FromArgb(32, 34, 46);
            dataGridViewCellStyle9.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            RemoteFilesGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            RemoteFilesGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            RemoteFilesGrid.Columns.AddRange(new DataGridViewColumn[] { dataGridViewImageColumn1, dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3 });
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = Color.FromArgb(32, 34, 46);
            dataGridViewCellStyle10.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle10.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = Color.FromArgb(35, 33, 75);
            dataGridViewCellStyle10.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = DataGridViewTriState.False;
            RemoteFilesGrid.DefaultCellStyle = dataGridViewCellStyle10;
            RemoteFilesGrid.EnableHeadersVisualStyles = false;
            RemoteFilesGrid.GridColor = Color.White;
            RemoteFilesGrid.Location = new Point(638, 205);
            RemoteFilesGrid.Name = "RemoteFilesGrid";
            RemoteFilesGrid.ReadOnly = true;
            RemoteFilesGrid.RowHeadersVisible = false;
            RemoteFilesGrid.RowHeadersWidth = 51;
            RemoteFilesGrid.RowTemplate.Height = 29;
            RemoteFilesGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            RemoteFilesGrid.Size = new Size(606, 533);
            RemoteFilesGrid.TabIndex = 23;
            RemoteFilesGrid.CellContentClick += RemoteFilesGrid_CellContentClick_1;
            RemoteFilesGrid.CellContentDoubleClick += RemoteFilesGrid_CellContentDoubleClick;
            RemoteFilesGrid.CellDoubleClick += RemoteFilesGrid_CellDoubleClick;
            RemoteFilesGrid.CellEndEdit += RemoteFilesGrid_CellEndEdit;
            RemoteFilesGrid.CellFormatting += RemoteFilesGrid_CellFormatting;
            RemoteFilesGrid.SelectionChanged += RemoteFilesGrid_SelectionChanged;
            RemoteFilesGrid.DragDrop += RemoteFilesGrid_DragDrop;
            RemoteFilesGrid.DragEnter += RemoteFilesGrid_DragEnter;
            RemoteFilesGrid.DragOver += RemoteFilesGrid_DragOver;
            RemoteFilesGrid.MouseClick += RemoteFilesGrid_MouseClick;
            RemoteFilesGrid.MouseDown += RemoteFilesGrid_MouseDown;
            RemoteFilesGrid.MouseMove += RemoteFilesGrid_MouseMove;
            // 
            // dataGridViewImageColumn1
            // 
            dataGridViewImageColumn1.FillWeight = 16F;
            dataGridViewImageColumn1.HeaderText = "";
            dataGridViewImageColumn1.MinimumWidth = 6;
            dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            dataGridViewImageColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "File Name:";
            dataGridViewTextBoxColumn1.MinimumWidth = 6;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Size:";
            dataGridViewTextBoxColumn2.MinimumWidth = 6;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Last change:";
            dataGridViewTextBoxColumn3.MinimumWidth = 6;
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // RemoteEmptyFolderLabel
            // 
            RemoteEmptyFolderLabel.AutoSize = true;
            RemoteEmptyFolderLabel.BackColor = Color.FromArgb(32, 34, 46);
            RemoteEmptyFolderLabel.ForeColor = Color.White;
            RemoteEmptyFolderLabel.Location = new Point(882, 273);
            RemoteEmptyFolderLabel.Name = "RemoteEmptyFolderLabel";
            RemoteEmptyFolderLabel.Size = new Size(137, 20);
            RemoteEmptyFolderLabel.TabIndex = 24;
            RemoteEmptyFolderLabel.Text = "The folder is empty";
            RemoteEmptyFolderLabel.Visible = false;
            // 
            // FrmFileManager
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.FromArgb(17, 17, 25);
            ClientSize = new Size(1257, 880);
            Controls.Add(RemoteEmptyFolderLabel);
            Controls.Add(RemoteFilesGrid);
            Controls.Add(CancelRemoteOperationIcon);
            Controls.Add(LocalEmptyFolderLabel);
            Controls.Add(LocalFilesParentDirIcon);
            Controls.Add(RemoteFilesystemIcon);
            Controls.Add(HardDriveIcon);
            Controls.Add(RefreshRemoteFilesIcon);
            Controls.Add(RemoteFilesUndoIcon);
            Controls.Add(FileBeingProcessedLabel);
            Controls.Add(RemoteFilePathTextbox);
            Controls.Add(LocalFilesGrid);
            Controls.Add(LocalFilesTextBox);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmFileManager";
            Load += FrmFileManager_Load;
            ((System.ComponentModel.ISupportInitialize)LocalFilesGrid).EndInit();
            ((System.ComponentModel.ISupportInitialize)RemoteFilesUndoIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)RefreshRemoteFilesIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)HardDriveIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)RemoteFilesystemIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)LocalFilesParentDirIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)CancelRemoteOperationIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)RemoteFilesGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox LocalFilesTextBox;
        private DataGridView LocalFilesGrid;
        private DataGridViewImageColumn LocalIconColumn;
        private DataGridViewTextBoxColumn fileNameColumn;
        private DataGridViewTextBoxColumn fileSizeColumn;
        private DataGridViewTextBoxColumn LastChangeColumn;
        private TextBox RemoteFilePathTextbox;
        private Label FileBeingProcessedLabel;
        private PictureBox RemoteFilesUndoIcon;
        private PictureBox RefreshRemoteFilesIcon;
        private PictureBox HardDriveIcon;
        private PictureBox RemoteFilesystemIcon;
        private PictureBox LocalFilesParentDirIcon;
        private Label LocalEmptyFolderLabel;
        private PictureBox CancelRemoteOperationIcon;
        private DataGridView RemoteFilesGrid;
        private DataGridViewImageColumn dataGridViewImageColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private Label RemoteEmptyFolderLabel;
    }
}