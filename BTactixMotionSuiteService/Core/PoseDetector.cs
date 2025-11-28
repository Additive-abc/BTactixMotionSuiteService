using BTactix.Common.Impelentations;
using BTactix.Common.Interfaces;
using BTactixMotionSuiteService.Core.Gesture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTactixMotionSuiteService.Core
{
    public class PoseDetector : BaseService
    {
        private readonly IEventBus _bus;

        public PoseDetector(IAppLoggerFactory loggerFactory, IErrorHandler errorHandler, IEventBus bus) : base(loggerFactory, errorHandler)
        {
            _bus = bus;
        }

        public void Process(float[] left, float[] right, long ts)
        {
            ErrorHandler.Execute(() =>
            {
                // Example: left pointing
                if (left[1] < 0.25f && left[2] > 0.7f && left[3] > 0.7f && left[4] > 0.7f)
                {
                    _bus.Publish(new PoseEvent("left_point", 1.0, ts));
                    return;
                }

                // left fist
                if (left.Average() > 0.8f)
                {
                    _bus.Publish(new PoseEvent("left_fist", left.Average(), ts));
                    return;
                }

                // thumbs up
                if (left[0] > 0.75f && left[1] < 0.25f && left.Skip(2).Average() < 0.3f)
                {
                    _bus.Publish(new PoseEvent("left_thumbs_up", 1.0, ts));
                    return;
                }

                // else publish neutral or nothing

            }, Logger, nameof(Process));
        }
    }
}
