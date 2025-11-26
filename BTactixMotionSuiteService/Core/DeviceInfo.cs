using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTactixMotionSuiteService.Core
{
    public class DeviceInfo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();    //This value can be either read once real device is connected
        public string Name { get; set; } = "";
        public string MacAddress { get; set; } = "";
        public int BatteryPercent { get; set; }
        public string FirmwareVersion { get; set; } = "";
        public bool Connected { get; set; } = false;
    }
}
