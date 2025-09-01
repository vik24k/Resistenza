using Resistenza.Server.Networking;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Resistenza.Common.Packets;
using Resistenza.Common.Packets.Task_Manager;
using Resistenza.Server.Utilities;
using static System.ComponentModel.Design.ObjectSelectorEditor;

using Resistenza.Server.Forms.Task_Manager;
using Microsoft.CodeAnalysis;
using OpenCvSharp.Internal.Vectors;
using System.Management.Automation.Language;

namespace Resistenza.Server.Forms
{
    public partial class FrmTaskManager : Form
    {
        public FrmTaskManager(ConnectedClient Client)
        {
            InitializeComponent();
            _Client = Client;
            _Client.IncomingPacket += OnPacketReceived;

            _ProcessesToBeAdded = new List<ProcessInfo>();
            _RowsSnapshotBeforeFiltering = new List<DataGridViewRow>();
            _KilledProcessesNames = new List<string>();
            _GridLock = new object();
            _IsUserFiltering = false;
            _LastFilter = string.Empty;
            _IsRunDialogOpen = false;
            _NameWithSubprocesses = new Dictionary<string, List<DataGridViewRow>>();
            _CurrentExpandedRow = null;


            ProcessesGrid.MouseWheel += OnMouseWheel;

            LookforProcessesTextbox.Tag = LookforProcessesTextbox.Text;

            ((DataGridViewImageColumn)this.ProcessesGrid.Columns[1]).DefaultCellStyle.NullValue = null;
            ((DataGridViewImageColumn)this.ProcessesGrid.Columns[2]).DefaultCellStyle.NullValue = null;

        }

        private ConnectedClient _Client;

        private int _IndexSelectedBefore;
        private int _FirstIndexShownBefore;
        private DataGridViewRow? _CurrentExpandedRow;

        private List<ProcessInfo> _ProcessesToBeAdded;
        private List<DataGridViewRow> _RowsSnapshotBeforeFiltering;
        private List<string> _KilledProcessesNames;
        private Dictionary<string, List<DataGridViewRow>> _NameWithSubprocesses;

        private object _GridLock;

        private bool _IsUserFiltering;
        private bool _IsRunDialogOpen;

        private string _LastFilter;


        private void OnMouseWheel(object? sender, MouseEventArgs e)
        {
            int newScrollValue = ProcessesGrid.FirstDisplayedScrollingRowIndex - e.Delta / 120;

            // Assicurati che il nuovo valore sia all'interno dei limiti consentiti
            if (newScrollValue < 0)
                newScrollValue = 0;
            if (newScrollValue >= ProcessesGrid.RowCount)
                newScrollValue = ProcessesGrid.RowCount - 1;

            // Imposta la nuova posizione verticale della griglia
            ProcessesGrid.FirstDisplayedScrollingRowIndex = newScrollValue;

        }


        private async void OnPacketReceived(object PacketReceived)
        {

            switch (PacketReceived)
            {
                case RunningProcessesResponse:

                    RunningProcessesResponse runningProcessesResponse = (RunningProcessesResponse)PacketReceived;
                    if (runningProcessesResponse.Entry != null)
                    {
                        AddProcessToGrid(runningProcessesResponse.Entry, runningProcessesResponse.IsFirst, runningProcessesResponse.IsLast);

                    }
                    return;
                case ActionOnProcessResponse:

                    ActionOnProcessResponse actionResult = (ActionOnProcessResponse)PacketReceived;
                    if (actionResult.Error != null && actionResult.Error != "")
                    {
                        await MessageBoxAsync.MessageBoxErrorAsync("Error from client", actionResult.Error);
                        return;
                    }
                    switch (actionResult.PossibleActionsOnProcess)
                    {
                        case PossibleActionsOnProcess.END_PROCESS:

                            //il processo è stato terminato con successo, rimuoviamo dalla grid la row in questione

                            lock (_GridLock)
                            {
                                foreach (DataGridViewRow Row in ProcessesGrid.Rows)
                                {
                                    string procNameWithNumber = (string)Row.Cells[1].Value;
                                    string procName = procNameWithNumber.Substring(0, procNameWithNumber.IndexOf(" "));

                                    _KilledProcessesNames.Add(procName);

                                    if (procName == actionResult.Name)
                                    {
                                        ProcessesGrid.Rows.RemoveAt(Row.Index);
                                        break;
                                    }
                                }

                            }

                            return;
                        case PossibleActionsOnProcess.CREATE_PROCESS:

                            //il processo è stato avviato con successo, lo aggiungiamo alla grid in index 0

                            lock (_GridLock)
                            {

                                DataGridViewRow Row = (DataGridViewRow)ProcessesGrid.Rows[0].Clone();
                                Row.Cells[0].Value = Properties.Resources.application_x_executable;
                                Row.Cells[1].Value = actionResult.Name;
                                Row.Cells[2].Value = "-";
                                Row.Cells[3].Value = "0";
                                Row.Cells[4].Value = "0";
                                ProcessesGrid.Rows.Insert(0, Row);

                            }

                            return;


                    }
                    return;

            }
        }

        public void AddProcessToGrid(ProcessInfo Info, bool IsFirst, bool IsLast)
        {
            lock (_GridLock)
            {
                if (IsFirst)
                {
                    _IndexSelectedBefore = ProcessesGrid.Rows.Count != 0 ? ProcessesGrid.SelectedRows[0].Index : 0;
                    _FirstIndexShownBefore = ProcessesGrid.FirstDisplayedScrollingRowIndex != -1 ? ProcessesGrid.FirstDisplayedScrollingRowIndex : 0;
                }


                _ProcessesToBeAdded.Add(Info);
                if (IsLast)
                {
                    foreach (ProcessInfo ProcessInfo in _ProcessesToBeAdded)
                    {
                        if (ProcessInfo == _ProcessesToBeAdded.First() && !_IsUserFiltering)
                        {
                            ProcessesGrid.Rows.Clear();
                        }

                        //potrebbe essere che il processo sia stato killato ma che nel frattempo i dati con il processo ancora inserito siano stati inviati al server
                        //cosi per la prima volta che arriva un pacchetto dati dopo aver mandato il comando di kill della task, il processo in questione viene ignorato 

                        if (_KilledProcessesNames.Count > 0)
                        {
                            foreach (string KilledProcess in _KilledProcessesNames)
                            {
                                if (KilledProcess.StartsWith(ProcessInfo.Name))
                                {
                                    _KilledProcessesNames.Remove(KilledProcess);
                                    break;
                                }
                            }
                            continue;

                        }

                        if (_IsUserFiltering)
                        {

                            foreach (DataGridViewRow FilteredRow in ProcessesGrid.Rows)
                            {
                                string procNameWithNumber = (string)FilteredRow.Cells[1].Value;
                                string procName = procNameWithNumber.Substring(0, procNameWithNumber.IndexOf(" "));

                                if (procName == ProcessInfo.Name)
                                {
                                    FilteredRow.Cells[4].Value = ProcessInfo.PID;
                                    FilteredRow.Cells[5].Value = ProcessInfo.MemoryUsedInMegabytes + " MB";
                                    FilteredRow.Cells[6].Value = ProcessInfo.CpuUsedInPercentage + "%";
                                }
                            }
                        }

                        else
                        {
                            int AlreadyPresentIndex = IsNamePresentInGrid(ProcessInfo.Name);
                            if (AlreadyPresentIndex != -1)
                            {
                                ProcessesGrid.Rows[AlreadyPresentIndex].Cells[1].Value = Properties.Resources.arrow_task_manager;
                                DataGridViewRow SubRow = (DataGridViewRow)ProcessesGrid.Rows[ProcessesGrid.RowCount - 1].Clone();
                                //SubRow.Cells[2].Value = Properties.Resources.application_x_executable;
                                SubRow.Cells[3].Value = ProcessInfo.Name;
                                SubRow.Cells[4].Value = ProcessInfo.PID;
                                SubRow.Cells[5].Value = ProcessInfo.MemoryUsedInMegabytes + " MB";
                                SubRow.Cells[6].Value = ProcessInfo.CpuUsedInPercentage + "%";

                                List<DataGridViewRow> subRowsAssociatedWithname;
                                if (!_NameWithSubprocesses.TryGetValue(ProcessInfo.Name, out subRowsAssociatedWithname))
                                {
                                    List<DataGridViewRow> newList = new List<DataGridViewRow>();
                                    newList.Add(SubRow);
                                    newList.Add(CloneRowValues(ProcessesGrid.Rows[AlreadyPresentIndex]));
                                    _NameWithSubprocesses.Add(ProcessInfo.Name, newList);
                                    //ProcessesGrid.Rows[AlreadyPresentIndex].Cells[3].Value = (string)ProcessesGrid.Rows[AlreadyPresentIndex].Cells[3].Value + " (2)";

                                }
                                else
                                {

                                    //ProcessesGrid.Rows[AlreadyPresentIndex].Cells[3].Value = (string)ProcessesGrid.Rows[AlreadyPresentIndex].Cells[3].Value + $" ({subRowsAssociatedWithname.Count})";
                                    subRowsAssociatedWithname.Add(SubRow);
                                }

                                _NameWithSubprocesses.TryGetValue(ProcessInfo.Name, out subRowsAssociatedWithname);

                                int TotalCpuPercentageForName = 0;
                                double TotalRamMbForName = 0;

                                foreach (DataGridViewRow newSub in subRowsAssociatedWithname)
                                {
                                    int ThisProcessCpuUsage = int.Parse(((string)newSub.Cells[6].Value).Substring(0, ((string)newSub.Cells[6].Value).IndexOf("%")));
                                    double ThisRamMb = double.Parse(((string)newSub.Cells[5].Value).Substring(0, ((string)newSub.Cells[5].Value).IndexOf(" ")));
                                    TotalCpuPercentageForName += ThisProcessCpuUsage;
                                    TotalRamMbForName += Math.Round(ThisRamMb, 1);
                                }

                                ProcessesGrid.Rows[AlreadyPresentIndex].Cells[4].Value = "";
                                ProcessesGrid.Rows[AlreadyPresentIndex].Cells[5].Value = TotalRamMbForName.ToString() + " MB";
                                ProcessesGrid.Rows[AlreadyPresentIndex].Cells[6].Value = TotalCpuPercentageForName.ToString() + " %";





                                continue;
                            }

                            //il processo è l'unico fino a quel momento col quel nome


                            ProcessesGrid.Rows.Add(1);
                            DataGridViewRow Row = ProcessesGrid.Rows[ProcessesGrid.RowCount - 1];

                            Row.Cells[1].Value = null;
                            Row.Cells[2].Value = Properties.Resources.application_x_executable;
                            Row.Cells[3].Value = ProcessInfo.Name;
                            Row.Cells[4].Value = ProcessInfo.PID;
                            Row.Cells[5].Value = ProcessInfo.MemoryUsedInMegabytes + " MB";
                            Row.Cells[6].Value = ProcessInfo.CpuUsedInPercentage + "%";



                            if (ProcessInfo == _ProcessesToBeAdded.Last())
                            {

                                ProcessesGrid.Rows[_IndexSelectedBefore].Selected = true;
                                ProcessesGrid.FirstDisplayedScrollingRowIndex = _FirstIndexShownBefore;
                                _ProcessesToBeAdded = new List<ProcessInfo>();

                            }
                        }


                    }
                }


            }

        }

        private DataGridViewRow CloneRowValues(DataGridViewRow Src)
        {
            DataGridViewRow originalRow = Src;
            DataGridViewRow Dest = new DataGridViewRow();

            for (int i = 0; i < originalRow.Cells.Count; i++)
            {
                DataGridViewCell originalCell = originalRow.Cells[i];
                DataGridViewCell newCell = (DataGridViewCell)originalCell.Clone();
                newCell.Value = originalCell.Value;
                Dest.Cells.Add(newCell);
            }

            return Dest;

        }

        private int IsNamePresentInGrid(string Name)
        {


            foreach (DataGridViewRow Row in ProcessesGrid.Rows)
            {
                string NameOfRow = (string)Row.Cells[3].Value;

                if (NameOfRow == Name)
                {
                    return Row.Index;
                }

            }

            return -1;

        }





        private async void EndTaskButton_Click(object sender, EventArgs e)
        {
            if (ProcessesGrid.SelectedRows.Count == 1)
            {
                DataGridViewRow Selected = ProcessesGrid.SelectedRows[0];
                string procNameWithNumber = (string)Selected.Cells[1].Value;
                string procName = procNameWithNumber.Substring(0, procNameWithNumber.IndexOf(" "));


                ActionOnProcessRequest EndProcess = new ActionOnProcessRequest();
                EndProcess.Action = PossibleActionsOnProcess.END_PROCESS;
                EndProcess.Name = procName;


                await _Client.CustomStream.SendPacketAsync(EndProcess);

            }
        }

        private async void OnNewTaskStarted(object sender, string TaskCommand)
        {
            ActionOnProcessRequest StartProcess = new ActionOnProcessRequest();
            StartProcess.Action = PossibleActionsOnProcess.CREATE_PROCESS;
            StartProcess.Name = TaskCommand;
            await _Client.CustomStream.SendPacketAsync(StartProcess);

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (!_IsRunDialogOpen)
            {
                FrmNewTask RunForm = new FrmNewTask(_Client.IpAddress);
                RunForm.TaskStarted += OnNewTaskStarted;
                RunForm.FormClosed += RunForm_FormClosed;
                Task.Run(() => RunForm.ShowDialog());
                _IsRunDialogOpen = true;
            }
        }

        private void RunForm_FormClosed(object? sender, EventArgs e)
        {
            _IsRunDialogOpen = false;
        }

        private void LookforProcessesTextbox_TextChanged(object sender, EventArgs e)
        {
            string ThisFilter = LookforProcessesTextbox.Text;

            if (LookforProcessesTextbox.Text == string.Empty)
            {
                if (_LastFilter != string.Empty && ThisFilter == _LastFilter.Substring(0, _LastFilter.Length - 1))
                {
                    ResetFilter(true);
                    return;
                }

                foreach (DataGridViewRow RowBeforeFiltering in ProcessesGrid.Rows)
                {
                    _RowsSnapshotBeforeFiltering.Add(RowBeforeFiltering);
                }
                return;
            }


            dynamic ListToIterThrough;


            if (_LastFilter == string.Empty || ThisFilter != _LastFilter.Substring(0, _LastFilter.Length - 1))
            {

                ListToIterThrough = ProcessesGrid.Rows;

            }
            else
            {
                //è stato premuto backspace
                ListToIterThrough = _RowsSnapshotBeforeFiltering;
            }

            _IsUserFiltering = true;
            pictureBox2.Visible = true;


            List<DataGridViewRow> rowsShownNext = new List<DataGridViewRow>();
            foreach (DataGridViewRow Row in ListToIterThrough)
            {
                string NameOfProcess = (string)Row.Cells[1].Value;
                string FilterCapitalised = LookforProcessesTextbox.Text.First().ToString().ToUpper() + LookforProcessesTextbox.Text.Substring(1);
                if (NameOfProcess.StartsWith(LookforProcessesTextbox.Text) || NameOfProcess.StartsWith(FilterCapitalised))
                {
                    rowsShownNext.Add(Row);
                }
            }

            lock (_GridLock)
            {
                ProcessesGrid.Rows.Clear();
                ProcessesGrid.Rows.AddRange(rowsShownNext.ToArray());
            }

            _LastFilter = ThisFilter;

        }

        private void LookforProcessesTextbox_MouseClick(object sender, MouseEventArgs e)
        {
            if (LookforProcessesTextbox.Text == LookforProcessesTextbox.Tag.ToString())
            {
                LookforProcessesTextbox.Text = string.Empty;
                LookforProcessesTextbox.TextAlign = HorizontalAlignment.Left;

            }
        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {

            ResetFilter(true);

        }

        private void ResetFilter(bool RemoveFocus)
        {
            LookforProcessesTextbox.TextAlign = HorizontalAlignment.Center;
            LookforProcessesTextbox.Text = (string)LookforProcessesTextbox.Tag;

            if (RemoveFocus)
            {
                LookforProcessesTextbox.Enabled = false;
                LookforProcessesTextbox.Enabled = true;
            }

            pictureBox2.Visible = false;

            _IsUserFiltering = false;

            lock (_GridLock)
            {
                ProcessesGrid.Rows.Clear();
                ProcessesGrid.Rows.AddRange(_RowsSnapshotBeforeFiltering.ToArray());
                _RowsSnapshotBeforeFiltering.Clear();
            }


        }

        private void ProcessesGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                if (ProcessesGrid.Rows[e.RowIndex].Cells[1].Value == null) // la riga non è espandibile
                {
                    return;
                }

                Image RowIcon = (Image)ProcessesGrid.Rows[e.RowIndex].Cells[1].Value;

                if (AreImagesEqual(RowIcon, Properties.Resources.arrow_task_manager)) //è effettivamente una row espandibile ossia l'immagine non è null
                {

                    if (_CurrentExpandedRow != null)
                    {
                        RemoveExpandedRows(_CurrentExpandedRow.Index);
                    }

                    ProcessesGrid.Rows[e.RowIndex].Cells[1].Value = Properties.Resources.down_arrow_task_manager;
                    List<DataGridViewRow> otherProcesses;
                    string processName = (string)ProcessesGrid.Rows[e.RowIndex].Cells[3].Value; //prendo gli altri processi associati con quel nome
                    _NameWithSubprocesses.TryGetValue(processName, out otherProcesses);

                    int StartIndex = e.RowIndex + 1;
                    // ProcessesGrid.MultiSelect = false;

                    foreach (DataGridViewRow Row in otherProcesses)
                    {
                        if (ProcessesGrid.Rows.Contains(Row))
                        {

                            DataGridViewRow newRow = (DataGridViewRow)Row.Clone(); // Clona la riga esistente

                            for (int i = 0; i < Row.Cells.Count; i++)
                            {
                                newRow.Cells[i].Value = Row.Cells[i].Value; // Copia i valori delle celle

                            }
                            newRow.Cells[1].Value = null; //arrow icon
                            newRow.Cells[2].Value = null; //executable icon 
                            newRow.Cells[3].Value = String.Concat(Enumerable.Repeat(" ", 5)) + newRow.Cells[3].Value; //name 


                            lock (_GridLock)
                            {
                                ProcessesGrid.Rows.Insert(StartIndex, newRow);
                                //ProcessesGrid.Rows[StartIndex].Selected = true;
                            }
                        }

                        else
                        {
                            Row.Cells[1].Value = null; //arrow icon
                            Row.Cells[2].Value = null; //executable icon 
                            Row.Cells[3].Value = String.Concat(Enumerable.Repeat(" ", 5)) + Row.Cells[3].Value; //name 


                            lock (_GridLock)
                            {

                                ProcessesGrid.Rows.Insert(StartIndex, Row);
                            }
                        }

                        //ProcessesGrid.MultiSelect = true;
                        //ProcessesGrid.Rows[StartIndex].Selected = true;    
                        StartIndex++;


                    }

                    _CurrentExpandedRow = ProcessesGrid.Rows[e.RowIndex];
                }

                if (AreImagesEqual(RowIcon, Properties.Resources.down_arrow_task_manager))
                {

                    RemoveExpandedRows(_CurrentExpandedRow.Index);
                }


            }
        }

        private void RemoveExpandedRows(int initialRowIndex)
        {

            string FirstRowProcessName = (string)ProcessesGrid.Rows[initialRowIndex].Cells[3].Value;
            List<int> IndexesToRemove = new List<int>();

            for (int i = initialRowIndex + 1; i < ProcessesGrid.Rows.Count; i++)
            {

                string name = ((string)ProcessesGrid.Rows[i].Cells[3].Value).Trim();
                if (name == FirstRowProcessName)
                {
                    lock (_GridLock)
                    {
                        IndexesToRemove.Add(i);
                    }

                }
                else
                {
                    break;
                }

            }
            IndexesToRemove.Reverse();
            foreach (int Index in IndexesToRemove)
            {
                ProcessesGrid.Rows.RemoveAt(Index);
            }

            ProcessesGrid.Rows[initialRowIndex].Cells[1].Value = Properties.Resources.arrow_task_manager;
        }

        private bool AreImagesEqual(Image img1, Image img2)
        {
            if (img1.Size != img2.Size)
            {
                return false;
            }

            // Converti le immagini in array di byte
            byte[] imgBytes1, imgBytes2;
            using (MemoryStream ms1 = new MemoryStream())
            {
                img1.Save(ms1, img1.RawFormat);
                imgBytes1 = ms1.ToArray();
            }
            using (MemoryStream ms2 = new MemoryStream())
            {
                img2.Save(ms2, img2.RawFormat);
                imgBytes2 = ms2.ToArray();
            }

            // Confronta gli array di byte
            if (imgBytes1.Length != imgBytes2.Length)
            {
                return false;
            }

            for (int i = 0; i < imgBytes1.Length; i++)
            {
                if (imgBytes1[i] != imgBytes2[i])
                {
                    return false;
                }
            }

            return true;
        }

        private async void FrmTaskManager_Load(object sender, EventArgs e)
        {
            await _Client.CustomStream.SendPacketAsync(new RunningProcessesRequest());
        }
    }
}
