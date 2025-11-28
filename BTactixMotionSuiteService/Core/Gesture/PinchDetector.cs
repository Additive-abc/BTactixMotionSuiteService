using BTactix.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTactixMotionSuiteService.Core.Gesture
{
    public class PinchDetector : GestureDetectorBase
    {
        private readonly IEventBus _bus;
        private readonly Func<long> _utcNowMs;

        public PinchDetector(IAppLoggerFactory loggerFactory,
                             IErrorHandler errorHandler,GestureDefinition def, IEventBus bus) : base(loggerFactory, errorHandler, def)
        {
            _bus = bus; _utcNowMs = () => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        public override void ProcessFrame(GloveFrame frame, float[] leftNormalized, float[] rightNormalized)
        {
            ErrorHandler.Execute(() =>
            {
                var hand = _def.Hand.ToLower();
                float[] values = hand == "left" ? leftNormalized : rightNormalized;
                if (values == null) return;

                // check thresholds: thumb + index must exceed
                var thumbs = _def.FingerThresholds;
                bool isNow = PassedThresholds(thumbs, values);

                var now = _utcNowMs();

                if (_phase == GesturePhase.None)
                {
                    if (isNow)
                    {
                        _phase = GesturePhase.Started;
                        _phaseStartMs = now;
                        _bus.Publish(new GestureEvent(GestureId, _phase, ComputeConfidence(values), now));
                    }
                }
                else if (_phase == GesturePhase.Started)
                {
                    if (isNow)
                    {
                        if (now - _phaseStartMs >= _def.MinHoldMs)
                        {
                            _phase = GesturePhase.Holding;
                            _bus.Publish(new GestureEvent(GestureId, _phase, ComputeConfidence(values), now));
                        }
                    }
                    else
                    {
                        // canceled before hold
                        _phase = GesturePhase.Ended;
                        _bus.Publish(new GestureEvent(GestureId, _phase, ComputeConfidence(values), now));
                        Reset();
                    }
                }
                else if (_phase == GesturePhase.Holding)
                {
                    if (!isNow)
                    {
                        _phase = GesturePhase.Ended;
                        _bus.Publish(new GestureEvent(GestureId, _phase, ComputeConfidence(values), now));
                        Reset();
                    }
                    else
                    {
                        // still holding -> optionally emit periodic hold events
                    }
                }
            }, Logger, nameof(PinchDetector));
        }

        private double ComputeConfidence(float[] values)
        {
            float confidence = 0.0f;

            ErrorHandler.Execute(() =>
            {
                // simple average of defined finger values vs thresholds
                var map = new[] { "thumb", "index", "middle", "ring", "pinky" };
                var matched = _def.FingerThresholds.Keys.Select(k => map.ToList().IndexOf(k.ToLower())).Where(i => i >= 0).ToList();
                if (!matched.Any()) ;
                confidence = matched.Average(i => values[i]);
            }, Logger, nameof(ComputeConfidence));

            return confidence;
        }
    }
}
