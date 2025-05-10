using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BrookFlasher
{
    public class BrookDeviceStructs
    {
        public const int BUFFER_SIZE = 2048;
        public const int REG_SZ = 1;
        public const int FILE_SHARE_READ = 1;
        public const int FILE_SHARE_WRITE = 2;
        public const uint OPEN_EXISTING = 2;
        public const uint FILE_ATTRIBUTE_NORMAL = 128;
        public const uint FILE_FLAG_OVERLAPPED = 1073741824;
        public const uint FILE_DEVICE_UNKNOWN = 34;
        public const int WM_DEVICECHANGE = 537;
        public const int DBT_DEVICEARRIVAL = 32768;
        public const int DBT_DEVICEREMOVALCOMPLETE = 32772;
        public const int DBT_DEVTYPVOLUME = 2;
        public const int DBT_DEVTYP_DEVICEINTERFACE = 5;
        public const int DEVICE_NOTIFY_WINDOW_HANDLE = 0;
        public const int DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 4;
        public static IntPtr notificationHandle;

        public struct SP_DEVICE_INTERFACE_DATA
        {
            public int cbSize;
            public Guid InterfaceClassGuid;
            public int Flags;
            public IntPtr Reserved;
        }

        public struct SP_DEVINFO_DATA
        {
            public int cbSize;
            public Guid ClassGuid;
            public IntPtr DevInst;
            public IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public int cbSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2048)]
            public string DevicePath;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct HIDD_ATTRIBUTES
        {
            [MarshalAs(UnmanagedType.U4)]
            public uint Size;
            [MarshalAs(UnmanagedType.U2)]
            public ushort VendorID;
            [MarshalAs(UnmanagedType.U2)]
            public ushort ProductID;
            [MarshalAs(UnmanagedType.U2)]
            public ushort VersionNumber;
        }

        public struct DEV_BROADCAST_DEVICEINTERFACE
        {
            public int dbcc_size;
            public int dbcc_devicetype;
            public int dbcc_reserved;
            public Guid dbcc_classguid;
            public char dbcc_name;
            public static readonly int Size = Marshal.SizeOf(typeof(BrookDeviceStructs.DEV_BROADCAST_DEVICEINTERFACE));
        }
    }
}
