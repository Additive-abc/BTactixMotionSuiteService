using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTactixMotionSuiteService.Core.Gesture
{
    public interface IGestureDetector
    {
        string GestureId { get; }
        void ProcessFrame(GloveFrame frame, float[] leftNormalized, float[] rightNormalized);
        void Reset();
    }
}
