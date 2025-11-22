using BTactix.Common.Interfaces;
using BTactixMotionSuiteService.Interfaces;
using log4net;

namespace BTactixMotionSuiteService
{
    public class CoreOrchestratorServiceWorker : BackgroundService
    {
        private readonly ILog _logger;

        public CoreOrchestratorServiceWorker(IAppLoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(this.GetType());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.Info($"Worker running at: {DateTimeOffset.Now}");

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
