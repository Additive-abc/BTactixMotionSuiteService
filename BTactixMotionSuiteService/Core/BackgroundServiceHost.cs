using BTactix.Common.Impelentations;
using BTactix.Common.Interfaces;
using BTactixMotionSuiteService.Interfaces;
using BTactixMotionSuiteService.SampleDataProducer;
using BTactixMotionSuiteService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTactixMotionSuiteService.Core
{
    public class BackgroundServiceHost : BaseService
    {
        private readonly IIpcNamedPipeServer _ipc;
        private readonly IDeviceManagementService _deviceManagementService;
        private readonly IEventBus _bus;
        private readonly IDataExportService _dataExportService;
        private readonly IGestureEngine _gestureEngine;
        private readonly IGloveFrameProducer _gloveFrameProducer;

        public BackgroundServiceHost(IAppLoggerFactory loggerFactory, 
                                     IErrorHandler errorHandler, 
                                     IIpcNamedPipeServer ipc,
                                     IDeviceManagementService deviceManagementService,
                                     IEventBus bus,
                                     IDataExportService dataExportService,
                                     IGestureEngine gestureEngine,
                                     IGloveFrameProducer gloveFrameProducer,
                                     string pipeName) : base(loggerFactory, errorHandler)
        {
            this._bus = bus;
            this._deviceManagementService = deviceManagementService;
            this._ipc = ipc;
            this._dataExportService = dataExportService;
            this._gestureEngine = gestureEngine;
            this._gloveFrameProducer = gloveFrameProducer;
        }

        public async Task RunAsync(CancellationToken ct)
        {
            // subscribe to IPC commands
            _bus.Subscribe<IpcCommand>(cmd => {
                // basic command routing: implement actual command handlers
                Console.WriteLine($"Cmd from UI: {cmd.Type}");
            });

            // spin up modules
            var ipcTask = _ipc.RunAsync(ct);

            // Example: start a fake sensor producer
            _ = _gloveFrameProducer.StartAsync(ct);

            // gesture engine
            _gestureEngine.Start();

            // data exporter
            _dataExportService.Start();

            await Task.WhenAll(ipcTask);
        }
    }
}
