using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrookFlasher
{
    public static class BrookFlash
    {
        private static BrookDevice brookDevice = new BrookDevice();
        private static string devicePath = "";
        private static int fwSec;
        public static bool FinishFirmwareFlashing()
        {
            if (!brookDevice.Open(devicePath))
                return false;
            byte[] buf = new byte[65];
            buf[1] = (byte)171;
            buf[5] = (byte)(fwSec & (int)byte.MaxValue);
            buf[6] = (byte)(fwSec >> 8);
            int num = brookDevice.WriteByte(buf) ? 1 : 0;
            brookDevice.Close();
            return num != 0;
        }
        private static bool SyncFirmwareSection(int FirmwareSection)
        {
            if (brookDevice.Open(devicePath))
            {
                fwSec = FirmwareSection;
                byte[] buf = new byte[65];
                buf[1] = (byte)164;
                buf[5] = (byte)fwSec;
                buf[9] = (byte)fwSec;
                if (brookDevice.WriteByte(buf))
                {
                    ++fwSec;
                    if (CheckFirmwareSection(fwSec) != null)
                    {
                        ++fwSec;
                        brookDevice.Close();
                        return true;
                    }
                }
            }
            brookDevice.Close();
            return false;
        }
        private static byte[] CheckFirmwareSection(int FwSection)
        {
            int num = 0;
            do
            {
                byte[] numArray = brookDevice.ReadByte();
                if (numArray == null)
                {
                    ++num;
                }
                else
                {
                    if ((int)numArray[5] + ((int)numArray[6] << 8) + ((int)numArray[7] << 16) == FwSection)
                        return numArray;
                    ++num;
                }
            }
            while (num <= 500);
            return (byte[])null; //Error Verifying Firmware
        }
        private static bool ConfirmChecksum(byte[] sectionData, byte[] sectionChecksum)
        {
            int num1 = 0;
            foreach (byte num2 in sectionData)
                num1 += (int)num2;
            int num3 = (int)sectionChecksum[1] + ((int)sectionChecksum[2] << 8);
            return num1 == num3;
        }
        public static bool FirmwareFlashing(string fileName, bool checksumCheck)
        {
            SyncFirmwareSection(0);
            if (brookDevice.Open(devicePath))
            {
                byte[] firmware = ReadFirmware(fileName);
                int length = firmware.Length;
                int offset = 0;
                int process = 0;
                int procCheck = 0;
                byte[] SectionData = new byte[65];
                while (true)
                {
                    do
                    {
                        Array.Clear((Array)SectionData, 0, SectionData.Length);
                        if (offset == 0)
                        {
                            SectionData[1] = (byte)160;
                            SectionData[5] = (byte)(fwSec & (int)byte.MaxValue);
                            SectionData[6] = (byte)(fwSec >> 8);
                            SectionData[13] = (byte)(firmware.Length & (int)byte.MaxValue);
                            SectionData[14] = (byte)(firmware.Length >> 8);
                            SectionData[15] = (byte)(firmware.Length >> 16);
                            Array.Copy((Array)firmware.SubArray<byte>(0, 48), 0, (Array)SectionData, 17, 48);
                            length -= 48;
                            offset += 48;
                        }
                        else if (length < 56)
                        {
                            SectionData[5] = (byte)(fwSec & (int)byte.MaxValue);
                            SectionData[6] = (byte)(fwSec >> 8);
                            Array.Copy((Array)firmware.SubArray<byte>(offset, length), 0, (Array)SectionData, 9, length);
                            length -= 56;
                            offset += 56;
                        }
                        else
                        {
                            SectionData[5] = (byte)(fwSec & (int)byte.MaxValue);
                            SectionData[6] = (byte)(fwSec >> 8);
                            Array.Copy((Array)firmware.SubArray<byte>(offset, 56), 0, (Array)SectionData, 9, 56);
                            length -= 56;
                            offset += 56;
                        }
                        if (brookDevice.WriteByte(SectionData))
                        {

                            ++fwSec;
                            byte[] firmwareChecksum = CheckFirmwareSection(fwSec);
                            if (firmwareChecksum == null)
                                return false;
                            if (length <= 0)
                            {
                                int num2 = (int)firmwareChecksum[9] + ((int)firmwareChecksum[10] << 8);
                                ++fwSec;
                                brookDevice.Close();
                                return true;
                            }
                            if (checksumCheck)
                            {
                                if (!ConfirmChecksum(SectionData, firmwareChecksum))
                                {
                                    string error = "Checksum is wrong.";
                                    Console.WriteLine(error);
                                    Console.WriteLine("Press Enter to exit.");
                                    Console.ReadLine();
                                    Environment.Exit(0);
                                    return false;
                                }
                            }
                            ++fwSec;
                            process = offset * 20 / firmware.Length;
                        }
                        else
                            goto unexpected;
                    }
                    while (procCheck == process);
                    procCheck = process;

                }
            unexpected:
                brookDevice.Close();
                return false;
            }
            brookDevice.Close();
            return false;
        }
        public static bool Connect_To_DFU(int retries)
        {
            int tryNum = 0;
            while (true)
            {
                foreach (BrookDeviceInfo brookDFU in BrookDeviceInfo.WingManList)
                {
                    string usbDevice = brookDevice.Get_USB_Device(Convert.ToInt32(brookDFU.VID, 16), Convert.ToInt32(brookDFU.PID, 16));
                    if (usbDevice != null)
                    {
                        Console.WriteLine($"Found BrookWingman in DFU Mode @ \"{usbDevice}\"");
                        Thread.Sleep(500);
                        devicePath = usbDevice;
                        return true;
                    }
                }
                ++tryNum;
                if (tryNum <= retries)
                    Thread.Sleep(10);
                else
                    break;
            }
            return false;
        }

        public static byte[] ReadFirmware(string filePath)
        {
            IEnumerable<string> hexFirmware = File.ReadLines(filePath);
            byte[] binFirmware = new byte[0];
            hexFirmware.ToList<string>().ForEach((Action<string>)(x =>
            {
                if (x[7] != '0' || x[8] != '0')
                    return;
                string str = x.Remove(0, 9);
                binFirmware = ((IEnumerable<byte>)binFirmware).Concat<byte>((IEnumerable<byte>)str.Remove(str.Length - 2, 2).StringToByteArray()).ToArray<byte>();
            }));
            return binFirmware;
        }
       
    }
}
