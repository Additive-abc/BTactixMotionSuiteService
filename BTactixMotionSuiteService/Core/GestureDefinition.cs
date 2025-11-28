using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTactixMotionSuiteService.Core
{
    public class GestureDefinition
    {
        public string Id { get; set; } = "";
        public string Hand { get; set; } = "left"; // left/right/both
        public Dictionary<string, float> FingerThresholds { get; set; } = new();
        public int MinHoldMs { get; set; } = 40;
        public float Hysteresis { get; set; } = 0.08f;
    }

    public class GestureProfile
    {
        public List<GestureDefinition> Gestures { get; set; } = new();
    }
}
