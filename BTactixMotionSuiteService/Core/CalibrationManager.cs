using BTactix.Common.Impelentations;
using BTactix.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTactixMotionSuiteService.Core
{
    public class CalibrationManager : BaseService, ICalibrationManager
    {
        public CalibrationManager(IAppLoggerFactory loggerFactory, IErrorHandler errorHandler) : base(loggerFactory, errorHandler)
        {
        }

        public CalibrationProfile BuildFromSamples(int[][] stepOpen, int[][] stepClosed, string deviceId)
        {
            CalibrationProfile? result = null;

            ErrorHandler.Execute(() =>
            {
                if ((stepOpen?.Length ?? 0) == 0 || (stepClosed?.Length ?? 0) == 0)
                    throw new ArgumentException("Need samples for open and closed steps");

                var openAvg = Enumerable.Range(0, 5).Select(i => stepOpen.Average(r => (double)r[i])).Select(d => (float)d).ToArray();
                var closedAvg = Enumerable.Range(0, 5).Select(i => stepClosed.Average(r => (double)r[i])).Select(d => (float)d).ToArray();

                result  =  new CalibrationProfile
                {
                    DeviceId = deviceId,
                    OpenAvg = openAvg,
                    ClosedAvg = closedAvg,
                    LastUpdatedUtc = DateTime.UtcNow
                };
            }, Logger, nameof(BuildFromSamples));

            return result;
        }

        public float[] Normalize(int[] raw, CalibrationProfile profile)
        {
            var outArr = new float[5];
            ErrorHandler.Execute(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    var denom = profile.ClosedAvg[i] - profile.OpenAvg[i];
                    if (Math.Abs(denom) < 1e-6f) outArr[i] = 0f;
                    else outArr[i] = (raw[i] - profile.OpenAvg[i]) / denom;
                    outArr[i] = Math.Clamp(outArr[i], 0f, 1f);
                }

            }, Logger, nameof(Normalize));

            return outArr;
        }

        // Overload to accept already float-ish data
        public float[] Normalize(float[] raw, CalibrationProfile profile)
        {
            // If raw already floats within sensor scale, treat similarly:
            var outArr = new float[5];

            ErrorHandler.Execute(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    var denom = profile.ClosedAvg[i] - profile.OpenAvg[i];
                    if (Math.Abs(denom) < 1e-6f) outArr[i] = 0f;
                    else outArr[i] = (raw[i] - profile.OpenAvg[i]) / denom;
                    outArr[i] = Math.Clamp(outArr[i], 0f, 1f);
                }

            }, Logger, nameof(Normalize));
            
            return outArr;
        }
    }
}
