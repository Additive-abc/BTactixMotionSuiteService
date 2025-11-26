using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTactixMotionSuiteService.Core
{
    public class GloveFrame
    {
        public long TimestampMs { get; set; }
        public float[] LeftFingerFlex { get; set; } = new float[5];
        public float[] RightFingerFlex { get; set; } = new float[5];
        public float LeftSplay { get; set; }
        public float RightSplay { get; set; }
        public float JoystickX { get; set; }
        public float JoystickY { get; set; }
        public int Buttons { get; set; } // bitmask
    }
}
