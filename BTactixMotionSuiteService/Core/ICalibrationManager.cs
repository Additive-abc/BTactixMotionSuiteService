using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTactixMotionSuiteService.Core
{
    public interface ICalibrationManager
    {
        CalibrationProfile BuildFromSamples(int[][] stepOpen, int[][] stepClosed, string deviceId);
        float[] Normalize(int[] raw, CalibrationProfile profile);
        float[] Normalize(float[] raw, CalibrationProfile profile);
    }
}
