using server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace client
{
    /// <summary>
    /// Interaction logic for ExitConfermation.xaml
    /// </summary>
    public partial class ExitConfermation : Window
    {
        public ExitConfermation()
        {
            InitializeComponent();
            
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public delegate void ConfermationGivenHandler();
        public event ConfermationGivenHandler? ConfermationGiven;

        /*BEGINNING OF CLASS METHODS*/

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ReleaseCapture();
            IntPtr FormHandle = new WindowInteropHelper(this).Handle;
            SendMessage(FormHandle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        } 

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool ExitConfirmed = (((Button)sender).Content == CancelButton.Content) ? false : true;
            if (ExitConfirmed)
            {
                ConfermationGiven?.Invoke();
            }
            else
            {
                this.Hide();
            }
        }
    }
}
