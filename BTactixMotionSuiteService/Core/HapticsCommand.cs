using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTactixMotionSuiteService.Core
{
    public class HapticsCommand
    {
        public string Target { get; set; } = "left";
        public float Intensity { get; set; } = 1.0f;
        public int DurationMs { get; set; } = 100;
    }
}
