using System;
using System.Drawing;
using System.Windows.Forms;


namespace Resistenza.Server.FormsAddons
{
    public class SimpleTextContextMenu : ContextMenuStrip
    {
        private ToolStripMenuItem[] items;

        public SimpleTextContextMenu(string[] itemTexts)
        {
            InitializeComponents(itemTexts);
        }

        private void InitializeComponents(string[] itemTexts)
        {
            items = new ToolStripMenuItem[itemTexts.Length];

            for (int i = 0; i < itemTexts.Length; i++)
            {
                items[i] = new ToolStripMenuItem(itemTexts[i]);
                items[i].Click += Item_Click;
            }

            Items.AddRange(items);
        }

        private void Item_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem selectedItem = (ToolStripMenuItem)sender;

            // Esegui l'azione desiderata quando un elemento viene cliccato.
            // In questo esempio, stampiamo il testo dell'elemento selezionato.
            MessageBox.Show($"Hai cliccato su: {selectedItem.Text}", "Elemento selezionato", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
