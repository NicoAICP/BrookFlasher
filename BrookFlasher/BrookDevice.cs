using HidLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BrookFlasher
{
    internal class BrookDevice
    {

            private IntPtr intPtr;
            private BrookDeviceStructs.SP_DEVICE_INTERFACE_DATA dia;
            private BrookDeviceStructs.SP_DEVINFO_DATA da;
            private BrookDeviceStructs.SP_DEVICE_INTERFACE_DETAIL_DATA didd;

            public BrookDevice()
            {
                this.dia = new BrookDeviceStructs.SP_DEVICE_INTERFACE_DATA();
                this.dia.cbSize = Marshal.SizeOf<BrookDeviceStructs.SP_DEVICE_INTERFACE_DATA>(this.dia);
                this.da = new BrookDeviceStructs.SP_DEVINFO_DATA();
                this.da.cbSize = Marshal.SizeOf<BrookDeviceStructs.SP_DEVINFO_DATA>(this.da);
                this.didd = new BrookDeviceStructs.SP_DEVICE_INTERFACE_DETAIL_DATA()
                {
                    cbSize = 4 + Marshal.SystemDefaultCharSize
                };
            }

            public bool WriteByte(byte[] buf)
            {
                uint lpNumberOfBytesWritten = 0;
                return DLLHook.WriteFile(this.intPtr, buf, (uint)buf.Length, out lpNumberOfBytesWritten, IntPtr.Zero);
            }

            public byte[] ReadByte()
            {
                uint lpNumberOfBytesRead = 0;
                byte[] lpBuffer = new byte[65];
                return !DLLHook.ReadFile(this.intPtr, lpBuffer, (uint)lpBuffer.Length, out lpNumberOfBytesRead, IntPtr.Zero) ? (byte[])null : lpBuffer;
            }

            public bool Open(string devicePath)
            {
                if (devicePath == null)
                    return false;
                this.intPtr = DLLHook.CreateFile(devicePath, 3221225472U, 3U, 0U, 2U, 128U, 0U);
                return !(this.intPtr == IntPtr.Zero);
            }

            public void Close()
            {
                if (!(this.intPtr != IntPtr.Zero))
                    return;
                DLLHook.CloseHandle(this.intPtr);
            }

            public string Get_USB_Device(int vid, int pid)
            {
                int[] products = new int[1];
                products[0] = pid;
                var dev = HidDevices.Enumerate(vid, products).FirstOrDefault();
                if (dev != null)
                {
                    return dev.DevicePath;
                }
                return (string)null;
            }
        }   
}
