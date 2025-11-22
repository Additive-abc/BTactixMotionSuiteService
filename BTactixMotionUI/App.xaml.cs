using System.Configuration;
using System.Data;
using System.Windows;

namespace BTactixMotionUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static HttpServiceClient HttpClient { get; private set; }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceUrl = "http://localhost:5000"; // or read from config
            HttpClient = new HttpServiceClient(serviceUrl);


            AppServices.ErrorHandler.Execute(async () =>
            {
                // Example: ping the service
                bool isRunning = await HttpClient.PingServiceAsync();

                if (isRunning)
                    AppServices.AppLogger.Info("Service is running!");
                else
                    AppServices.AppLogger.Warn("Service is NOT reachable.");

            }, AppServices.AppLogger, "OnStartup");
        }
    }

}
