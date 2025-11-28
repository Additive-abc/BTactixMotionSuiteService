using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTactixMotionSuiteService.Core
{
    public class CalibrationProfile
    {
        public string DeviceId { get; set; } = "";
        public float[] OpenAvg { get; set; } = new float[5];
        public float[] ClosedAvg { get; set; } = new float[5];
        public DateTime LastUpdatedUtc { get; set; } = DateTime.UtcNow;
    }
}
