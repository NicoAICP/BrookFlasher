using System.Runtime.InteropServices;
using System.Text;

namespace BrookFlasher
{
    public static class DLLHook
    {
        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr RegisterDeviceNotification(
          IntPtr recipient,
          IntPtr notificationFilter,
          int flags);

        [DllImport("hid.dll", SetLastError = true)]
        public static extern void HidD_GetHidGuid(out Guid Guid);

        [DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool HidD_GetAttributes(
          IntPtr HidDeviceObject,
          ref BrookDeviceStructs.HIDD_ATTRIBUTES Attributes);

        [DllImport("setupapi.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SetupDiGetClassDevs(
          ref Guid ClassGuid,
          int Enumerator,
          IntPtr hwndParent,
          int Flags);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool SetupDiEnumDeviceInterfaces(
          IntPtr DeviceInfoSet,
          IntPtr DeviceInfoData,
          ref Guid InterfaceClassGuid,
          int MemberIndex,
          ref BrookDeviceStructs.SP_DEVICE_INTERFACE_DATA DeviceInterfaceData);

        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool SetupDiGetDeviceInterfaceDetail(
          IntPtr DeviceInfoSet,
          ref BrookDeviceStructs.SP_DEVICE_INTERFACE_DATA DeviceInterfaceData,
          ref BrookDeviceStructs.SP_DEVICE_INTERFACE_DETAIL_DATA DeviceInterfaceDetailData,
          int DeviceInterfaceDetailDataSize,
          ref int RequiredSize,
          ref BrookDeviceStructs.SP_DEVINFO_DATA DeviceInfoData);

        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool SetupDiGetDeviceRegistryProperty(
          IntPtr DeviceInfoSet,
          ref BrookDeviceStructs.SP_DEVINFO_DATA DeviceInfoData,
          int iProperty,
          ref int PropertyRegDataType,
          IntPtr PropertyBuffer,
          int PropertyBufferSize,
          ref int RequiredSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr CreateFile(
          string lpFileName,
          uint dwDesiredAccess,
          uint dwShareMode,
          uint lpSecurityAttributes,
          uint dwCreationDisposition,
          uint dwFlagsAndAttributes,
          uint hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool DeviceIoControl(
          IntPtr hDevice,
          uint dwIoControlCode,
          uint[] lpInBuffer,
          int nInBufferSize,
          byte[] lpOutbuffer,
          int nOutBufferSize,
          ref int lpByteReturned,
          IntPtr lpOverlapped);

        [DllImport("kernel32.dll")]
        public static extern bool WriteFile(
          IntPtr hFile,
          byte[] lpBuffer,
          uint nNumberOfBytesToWrite,
          out uint lpNumberOfBytesWritten,
          IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadFile(
          IntPtr hFile,
          [Out] byte[] lpBuffer,
          uint nNumberOfBytesToRead,
          out uint lpNumberOfBytesRead,
          IntPtr lpOverlapped);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        [DllImport("kernel32.dll")]
        public static extern bool FlushFileBuffers(IntPtr hFile);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern uint GetPrivateProfileSectionNames(
          IntPtr lpszReturnBuffer,
          uint nSize,
          string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern uint GetPrivateProfileSection(
          string lpAppName,
          IntPtr lpReturnedString,
          uint nSize,
          string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern uint GetPrivateProfileString(
          string lpAppName,
          string lpKeyName,
          string lpDefault,
          [In, Out] char[] lpReturnedString,
          uint nSize,
          string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern uint GetPrivateProfileString(
          string lpAppName,
          string lpKeyName,
          string lpDefault,
          StringBuilder lpReturnedString,
          uint nSize,
          string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern uint GetPrivateProfileString(
          string lpAppName,
          string lpKeyName,
          string lpDefault,
          string lpReturnedString,
          uint nSize,
          string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WritePrivateProfileSection(
          string lpAppName,
          string lpString,
          string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WritePrivateProfileString(
          string lpAppName,
          string lpKeyName,
          string lpString,
          string lpFileName);
    }
}
