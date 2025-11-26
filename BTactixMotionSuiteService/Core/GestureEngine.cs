using BTactix.Common.Impelentations;
using BTactix.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTactixMotionSuiteService.Core
{
    public interface IGestureEngine
    {
        void Start();
    }


    public class GestureEngine: BaseService, IGestureEngine
    {
        private readonly IEventBus _bus;

        public GestureEngine(IAppLoggerFactory appLoggerFactory, IErrorHandler errorHandler, IEventBus bus)
         : base(appLoggerFactory, errorHandler)
        {
            _bus = bus;
        }

        public void Start()
        {
            _bus.Subscribe<GloveFrame>(OnFrame);
        }

        private void OnFrame(GloveFrame f)
        {
            ErrorHandler.Execute(() =>
            {
                var leftIndex = f.LeftFingerFlex[1];
                if (leftIndex > 0.85f)
                {
                    var g = new GestureEvent { GestureId = "left_pinch", Confidence = leftIndex, TimestampMs = f.TimestampMs };
                    _bus.Publish(g);
                    Logger.Info($"Gesture: {g.GestureId} @ {g.Confidence:F2}");
                }
            }, Logger, nameof(OnFrame));
            // simple example: if index finger flex > 0.8 => "pinch"
            
        }
    }
}
