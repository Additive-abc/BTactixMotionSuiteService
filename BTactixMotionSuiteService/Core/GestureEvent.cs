using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTactixMotionSuiteService.Core
{
    public class GestureEvent
    {
        public string GestureId { get; set; }
        public double Confidence { get; set; }
        public long TimestampMs { get; set; }
    }
}
