using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Resistenza.Server.Forms.Task_Manager
{
    public partial class FrmNewTask : Form
    {
        public FrmNewTask(string IpAddress)
        {
            InitializeComponent();
            this.Text = $"Run ({IpAddress})";
        }

        public event EventHandler<string> TaskStarted;
        public event EventHandler TaskFormClosed;

        private void StartCommandButton_Click(object sender, EventArgs e)
        {
            if (TaskTextbox.Text != "")
            {
                TaskStarted.Invoke(this, TaskTextbox.Text);
            }

            this.Close();
        }

        private void FrmNewTask_FormClosed(object sender, FormClosedEventArgs e)
        {
            TaskFormClosed?.Invoke(this, e);
        }
    }
}
