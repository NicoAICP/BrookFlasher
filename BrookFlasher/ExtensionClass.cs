using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrookFlasher
{
    internal static class ExtensionClass
    {
        public static T[] SubArray<T>(this T[] array, int offset, int length)
        {
            T[] destinationArray = new T[length];
            Array.Copy((Array)array, offset, (Array)destinationArray, 0, length);
            return destinationArray;
        }

        public static bool EqualArray(this byte[] array, byte[] arr1)
        {
            if (array.Length != arr1.Length)
                return false;
            for (int index = 0; index < array.Length; ++index)
            {
                if ((int)array[index] != (int)arr1[index])
                    return false;
            }
            return true;
        }

        public static byte[] StringToByteArray(this string hex)
        {
            byte[] byteArray = hex.Length % 2 != 1 ? new byte[hex.Length >> 1] : throw new Exception("The binary key cannot have an odd number of digits");
            for (int index = 0; index < hex.Length >> 1; ++index)
                byteArray[index] = (byte)((ExtensionClass.GetHexVal(hex[index << 1]) << 4) + ExtensionClass.GetHexVal(hex[(index << 1) + 1]));
            return byteArray;
        }

        private static int GetHexVal(char hex)
        {
            int num = (int)hex;
            return num - (num < 58 ? 48 : (num < 97 ? 55 : 87));
        }
    }
}
