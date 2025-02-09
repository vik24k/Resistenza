using client;
using client.socket;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Defining positions (-4 for the purple border, margins set in XAML)

            this.Width = SystemParameters.PrimaryScreenWidth * 0.75;
            this.Height = SystemParameters.PrimaryScreenHeight * 0.75;

            Titlebar.Height = this.Height / 13 - 4;
            Titlebar.Width = this.Width - 4;
           
            MainCanva.Width = this.Width - 4;
            MainCanva.Height = this.Height - Titlebar.Height - 4;

            Title.FontSize = Titlebar.Height / 2.5;

            ExitIcon.Height = 25;
            SettingsIcon.Height = 25;

            //Constructing attributes

            Socket = new Connection();

            ExitWindow = null;
            
         
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        /*BEGINNING OF ACTUAL CLASS ATTRIBUTES*/

        readonly Connection Socket;
        ExitConfermation? ExitWindow;


        /* BEGINNING OF ACTUAL CLASS METHODS */ 

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Canvas.SetTop(Title, (Titlebar.ActualHeight - Title.ActualHeight) / 2);
            Canvas.SetLeft(Title, 30);

            Canvas.SetTop(ExitIcon, (Titlebar.ActualHeight - ExitIcon.ActualHeight) / 2);
            Canvas.SetRight(ExitIcon, 20);

            Canvas.SetTop(SettingsIcon, (Titlebar.ActualHeight - ExitIcon.ActualHeight) / 2);
            Canvas.SetRight(SettingsIcon, 60);

            //ExitIcon.UpdateLayout();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            /*If the ExitConfermation window is open, we make the main window uninteractable until
             the exit decision is confirmed or cancelled, adding windows ping color animation?? */

            if (ExitWindow != null && ExitWindow.IsVisible) 
            {
                ExitWindow.Activate(); 

                WindowsUtils.CenterWindow(this, ExitWindow);
                e.Handled = true;
            }
        }

        private void TerminateMainForm()
        {
            this.Close();
            System.Windows.Application.Current.Shutdown();
        }

        private void Titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            /*Little hack to be able to drag around windows without titlebar:
             * 1) We release the mouse capture, giving back the mouse control to OS
             * 2) We use SendMessage to signal to the window the left button is down
             */
            
            IntPtr FormHandle = new WindowInteropHelper(this).Handle; 
            ReleaseCapture();
            SendMessage(FormHandle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);

            
        }

        private void ExitIcon_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            if (Socket.IsConnected || true) 
            {
                ExitWindow = new ExitConfermation();
                ExitWindow.ConfermationGiven += TerminateMainForm;
                ExitWindow.Show();

                WindowsUtils.CenterWindow(this, ExitWindow);

            }
            else
            {
                TerminateMainForm();
            }
        }

        
    }
}