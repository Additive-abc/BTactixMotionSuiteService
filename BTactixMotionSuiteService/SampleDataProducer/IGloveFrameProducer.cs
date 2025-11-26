using BTactixMotionSuiteService.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTactixMotionSuiteService.SampleDataProducer
{
    public interface IGloveFrameProducer
    {
        event Action<GloveFrame> OnFrame;
        Task StartAsync(CancellationToken token);
    }
}
