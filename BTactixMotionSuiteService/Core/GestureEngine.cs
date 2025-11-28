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
    public interface IGestureEngine
    {
        void Start(string gestureProfilePath = "gestures.json");
        void Stop();
    }


    public class GestureEngine: BaseService, IGestureEngine
    {
        private readonly IEventBus _bus;
        private readonly ICalibrationManager _calibrationManager;
        private readonly List<IGestureDetector> _detectors = new();
        private CalibrationProfile? _profile; // per-device: extend for multi-device

        public GestureEngine(IAppLoggerFactory appLoggerFactory, IErrorHandler errorHandler, IEventBus bus, ICalibrationManager cal)
         : base(appLoggerFactory, errorHandler)
        {
            _bus = bus;
            _calibrationManager = cal;
        }

        public void Start(string gestureProfilePath = @"..\SampleData\gestures.json")
        {
            var profile = GestureProfileLoader.Load(gestureProfilePath);
            foreach (var def in profile.Gestures)
            {
                // instantiate detectors by id (simple factory)
                if (def.Id == "pinch") _detectors.Add(new PinchDetector(def, _bus));
                else if (def.Id == "fist") _detectors.Add(new FistDetector(def, _bus));
                else if (def.Id == "point") _detectors.Add(new PointDetector(def, _bus));
                else if (def.Id == "thumbs_up") _detectors.Add(new ThumbsUpDetector(def, _bus));
                // else add custom
            }

            _bus.Subscribe<GloveFrame>(OnFrame);
        }

        private void OnFrame(GloveFrame f)
        {
            ErrorHandler.Execute(() =>
            {
                // Normalize raw if needed. If frame already normalized skip this.
                float[] leftNorm = f.LeftFingerFlex.Select(x => (float)x).ToArray();
                float[] rightNorm = f.RightFingerFlex.Select(x => (float)x).ToArray();

                if (_profile != null)
                {
                    leftNorm = _calibrationManager.Normalize(leftNorm, _profile);
                    // For a real system you need per-device profile and different for each hand
                }

                foreach (var d in _detectors)
                {
                    d.ProcessFrame(f, leftNorm, rightNorm);
                }
            }, Logger, nameof(OnFrame));
            // simple example: if index finger flex > 0.8 => "pinch"
            
        }

        public void SetCalibrationProfile(CalibrationProfile p) => _profile = p;

        public void Stop()
        {
            foreach (var d in _detectors) d.Reset();
        }
    }
}
