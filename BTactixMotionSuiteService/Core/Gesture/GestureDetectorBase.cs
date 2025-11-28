using BTactix.Common.Impelentations;
using BTactix.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTactixMotionSuiteService.Core.Gesture
{
    public enum GesturePhase { None, Started, Holding, Ended }
    public record GestureEvent(string GestureId, GesturePhase Phase, double Confidence, long TimestampMs);
    public record PoseEvent(string PoseId, double Confidence, long TimestampMs);

    public abstract class GestureDetectorBase : BaseService, IGestureDetector
    {
        public string GestureId { get; protected set; } = "";
        protected readonly GestureDefinition _def;
        protected GesturePhase _phase = GesturePhase.None;
        protected long _phaseStartMs = 0;
        private GestureDefinition def;

        protected GestureDetectorBase(IAppLoggerFactory loggerFactory, IErrorHandler errorHandler, GestureDefinition def) : base(loggerFactory, errorHandler)
        {
            _def = def;
            GestureId = def.Id;
        }

        protected bool PassedThresholds(Dictionary<string, float> fingerThresholds, float[] values)
        {
            bool flag = false;

            ErrorHandler.Execute(() =>
            {
                // finger order: thumb=0,index=1,middle=2,ring=3,pinky=4
                var map = new[] { "thumb", "index", "middle", "ring", "pinky" };
                foreach (var kv in fingerThresholds)
                {
                    var idx = System.Array.IndexOf(map, kv.Key.ToLower());
                    if (idx < 0) continue;
                    if (_def.Hysteresis <= 0)
                    {
                        if (values[idx] < kv.Value) ;
                    }
                    else
                    {
                        // use hysteresis: for start compare against (threshold + h), for end compare against (threshold - h)
                        var startThresh = kv.Value + _def.Hysteresis;
                        if (values[idx] < startThresh) ;
                    }
                }

            }, Logger, nameof(PassedThresholds));
           
            return flag;
        }

        public abstract void ProcessFrame(GloveFrame frame, float[] leftNormalized, float[] rightNormalized);
        public virtual void Reset()
        {
            _phase = GesturePhase.None;
            _phaseStartMs = 0;
        }
    }
}
