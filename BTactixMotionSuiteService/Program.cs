using BTactix.Common.Impelentations;
using BTactix.Common.Interfaces;
using BTactixMotionSuiteService;
using BTactixMotionSuiteService.Core;
using BTactixMotionSuiteService.Interfaces;
using BTactixMotionSuiteService.SampleDataProducer;
using BTactixMotionSuiteService.Services;
using BTactixMotionSuiteService.Utility;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        // Configure Windows Service hosting
        builder.Services.AddWindowsService(options =>
        {
            options.ServiceName = "BTactixMotionService";
        });

        var logPath = builder.Configuration["Logging:File:Path"] ?? "D:\\Logs\\B-Tactix";
        builder.Services.Configure<SampleDataOptions>(builder.Configuration.GetSection("SampleData"));
        // Register core services
        builder.Services.AddSingleton<IEventBus, SimpleEventBus>();
        builder.Services.AddSingleton<IGestureEngine, GestureEngine>();
        builder.Services.AddSingleton<IIpcNamedPipeServer, IpcNamedPipeServer>();
        builder.Services.AddSingleton<ICalibrationManager, CalibrationManager>();
        builder.Services.AddSingleton<IGloveFrameProducer, SampleDataProducer>();
        builder.Services.AddSingleton<IErrorHandler, ErrorHandler>();
        builder.Services.AddSingleton<IAppLoggerFactory>(_ => new AppLoggerFactory(logPath));
        builder.Services.AddSingleton<IDataExportService, DataExportService>();
        builder.Services.AddSingleton<IDeviceManagementService, DeviceManagementService>();
        builder.Services.AddSingleton<IErrorDiagnosticsService, ErrorDiagnosticsService>();
        builder.Services.AddSingleton<IGestureDetectionEngineService, GestureDetectionEngineService>();
        builder.Services.AddSingleton<IProfileLoadedService, ProfileLoadedService>();
        builder.Services.AddSingleton<ISystemTrayService, SystemTrayService>();
        builder.Services.AddHostedService<CoreOrchestratorServiceWorker>();

        var app = builder.Build();

        app.Run();
    }

}
