using BTactix.Common.Impelentations;
using BTactix.Common.Interfaces;
using BTactixMotionSuiteService.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BTactixMotionSuiteService.Utility;
using Microsoft.Extensions.Options;

namespace BTactixMotionSuiteService.SampleDataProducer
{
    public class SampleDataProducer : BaseService, IGloveFrameProducer
    {
        private readonly string _filePath;
        private string[] _lines;
        private int _index = 0;

        public event Action<GloveFrame>? OnFrame;

        public SampleDataProducer(IAppLoggerFactory appLoggerFactory, IErrorHandler errorHandler, IOptions<SampleDataOptions> options)
         : base(appLoggerFactory, errorHandler)
        {
            _filePath = options.Value.LeftCalibrationDataFilePath;
            _lines = File.ReadAllLines(_filePath);
        }

        public Task StartAsync(CancellationToken token)
        {
            ErrorHandler.ExecuteAsync(() =>
            {
                return Task.Run(async () =>
                {
                    foreach (var line in _lines)
                    {
                        if (token.IsCancellationRequested)
                            break;

                        var frame = ParseFrame(line);
                        OnFrame?.Invoke(frame);

                        await Task.Delay(10); // 100 Hz simulation
                    }
                }, token);

            },Logger, nameof(StartAsync));
           
            return Task.CompletedTask;
        }

        private GloveFrame ParseFrame(string line)
        {
            var frame = new GloveFrame();

            ErrorHandler.Execute(() =>
            {
                var parts = line.Split(';');

                foreach (var p in parts)
                {
                    if (p.StartsWith("L:"))
                    {
                        frame.LeftFingerFlex = p.Substring(2)
                            .Split(',')
                            .Select(float.Parse)
                            .ToArray();
                    }
                    else if (p.StartsWith("R:"))
                    {
                        frame.RightFingerFlex = p.Substring(2)
                            .Split(',')
                            .Select(float.Parse)
                            .ToArray();
                    }
                    else if (p.StartsWith("LS:"))
                        frame.LeftSplay = float.Parse(p.Substring(3));

                    else if (p.StartsWith("RS:"))
                        frame.RightSplay = float.Parse(p.Substring(3));

                    else if (p.StartsWith("JX:"))
                        frame.JoystickX = float.Parse(p.Substring(3));

                    else if (p.StartsWith("JY:"))
                        frame.JoystickY = float.Parse(p.Substring(3));

                    else if (p.StartsWith("BTN:"))
                        frame.Buttons = int.Parse(p.Substring(4));
                }

                frame.TimestampMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            },Logger, nameof(ParseFrame));

            return frame;

        }
    }

}
