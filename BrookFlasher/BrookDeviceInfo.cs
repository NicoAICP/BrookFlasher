using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrookFlasher
{
    public class BrookDeviceInfo
    {
        public string VID { get; set; }

        public string PID { get; set; }

        public BrookDeviceInfo()
        {
        }

        public BrookDeviceInfo(string Vid, string Pid)
        {
            this.VID = Vid;
            this.PID = Pid;
        }
        public static List<BrookDeviceInfo> WingManList
        {
            get
            {
                return new List<BrookDeviceInfo>()
                {
                    new BrookDeviceInfo() { VID = "0x0416", PID = "0x3F00" } //This is the DFU mode of atleast the XB2 and XB3 WingMan
                };
            }
        }
    }
}
