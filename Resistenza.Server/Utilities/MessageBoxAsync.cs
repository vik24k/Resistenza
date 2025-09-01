using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resistenza.Server.Utilities
{
    internal class MessageBoxAsync
    {
        public static async Task MessageBoxErrorAsync(string Title, string Body)
        {
            await Task.Run(() => MessageBox.Show(Body, Title, MessageBoxButtons.OK, MessageBoxIcon.Error));
        }
    }
}
