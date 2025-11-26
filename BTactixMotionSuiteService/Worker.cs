using BTactix.Common.Interfaces;
using BTactixMotionSuiteService.Interfaces;
using log4net;

namespace BTactixMotionSuiteService
{
    public class CoreOrchestratorServiceWorker : BackgroundService
    {
        private readonly ILog logger;
        private readonly IIpcNamedPipeServer ipcNamedPipeServer;

        public CoreOrchestratorServiceWorker(IAppLoggerFactory loggerFactory, IIpcNamedPipeServer ipcNamedPipeServer)
        {
            logger = loggerFactory.CreateLogger(this.GetType());
            this.ipcNamedPipeServer = ipcNamedPipeServer;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    //_logger.Info($"Worker running at: {DateTimeOffset.Now}");

            //    await Task.Delay(1000, stoppingToken);
            //}

            ipcNamedPipeServer.StartServerAsync();

            ipcNamedPipeServer.OnMessageReceived += (msg) =>
            {
                logger.Info($"Received from UI: {msg}");
            };

            return Task.CompletedTask;
        }
    }
}
