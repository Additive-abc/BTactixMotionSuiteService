using BTactix.Common.Impelentations;
using BTactix.Common.Interfaces;
using BTactixMotionSuiteService;
using BTactixMotionSuiteService.Interfaces;
using BTactixMotionSuiteService.Services;
using Microsoft.Extensions.Hosting;

public class Program
{
    //    public static void Main(string[] args)
    //    {
    //        // Use WebApplicationBuilder for HTTP + Windows Service
    //        var builder = WebApplication.CreateBuilder(args);
    //        builder.Configuration.SetBasePath(AppContext.BaseDirectory);

    //        // Setup logging early
    //        builder.Logging.ClearProviders();
    //        builder.Logging.AddConsole();

    //        // Read HTTP endpoint from config
    //        var httpUrl = builder.Configuration["HttpServer:Url"] ?? "http://localhost:5000";
    //        var uri = new Uri(httpUrl);

    //        // Configure Kestrel
    //        builder.WebHost.ConfigureKestrel(options =>
    //        {
    //#if DEBUG
    //            // Localhost binding for debug testing
    //            options.ListenLocalhost(uri.Port);
    //#else
    //            // Bind to all interfaces in production
    //            options.ListenAnyIP(uri.Port);
    //#endif
    //        });

    //        // Minimal routing setup (no HTTPS redirection)
    //        builder.Services.AddRouting();

    //        // Configure Windows Service hosting
    //        builder.Services.AddWindowsService(options =>
    //        {
    //            options.ServiceName = "BTactixMotionService";
    //        });

    //        // Logging path from config
    //        var logPath = builder.Configuration["Logging:File:Path"] ?? "D:\\Logs\\B-Tactix";

    //        // Register core services
    //        builder.Services.AddSingleton<IErrorHandler, ErrorHandler>();
    //        builder.Services.AddSingleton<IAppLoggerFactory>(_ => new AppLoggerFactory(logPath));
    //        builder.Services.AddSingleton<IDataExportService, DataExportService>();
    //        builder.Services.AddSingleton<IDeviceManagementService, DeviceManagementService>();
    //        builder.Services.AddSingleton<IErrorDiagnosticsService, ErrorDiagnosticsService>();
    //        builder.Services.AddSingleton<IGestureDetectionEngineService, GestureDetectionEngineService>();
    //        builder.Services.AddSingleton<IProfileLoadedService, ProfileLoadedService>();
    //        builder.Services.AddSingleton<ISystemTrayService, SystemTrayService>();
    //        builder.Services.AddHostedService<CoreOrchestratorServiceWorker>();

    //        var app = builder.Build();

    //        // Lifecycle diagnostics
    //        app.Lifetime.ApplicationStarted.Register(() =>
    //        {
    //            Console.WriteLine("✅ Application started successfully.");
    //        });
    //        app.Lifetime.ApplicationStopped.Register(() =>
    //        {
    //            Console.WriteLine("⛔ Application stopped.");
    //        });

    //        // Minimal endpoint to check service status
    //        app.MapGet("/", () => "BTactixMotionService is running");

    //        Console.WriteLine($"Starting Kestrel on {httpUrl}");

    //#if DEBUG
    //        app.Run(); // runs as console app in debug
    //#else
    //        app.RunAsService(); // runs as Windows Service in production
    //#endif
    //    }


    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var httpUrl = builder.Configuration["HttpServer:Url"] ?? "http://localhost:5000";
        var uri = new Uri(httpUrl);

        //builder.WebHost.ConfigureKestrel(o => o.ListenLocalhost(5000));
        builder.WebHost.ConfigureKestrel(options =>
               {
                   options.ListenAnyIP(uri.Port);
               });

        // Configure Windows Service hosting
        builder.Services.AddWindowsService(options =>
        {
            options.ServiceName = "BTactixMotionService";
        });

        var logPath = builder.Configuration["Logging:File:Path"] ?? "D:\\Logs\\B-Tactix";
        // Register core services
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
        app.MapGet("/", () => "BTactixMotionService is running");

        #if DEBUG
                app.Run(); // runs as console app in debug
        #else
                app.RunAsService(); // runs as Windows Service in production
        #endif
    }

}
