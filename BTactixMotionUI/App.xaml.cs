using BTactixMotionUI;
using Prism.Ioc;
using Prism.Unity; // or Prism.DryIoc depending on your container
using System.Windows;

namespace BTactixMotionUI
{
    public partial class App : PrismApplication
    {
        public static HttpServiceClient HttpClient { get; private set; }
        private bool isRunning;

        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<HttpServiceClient>(() => HttpClient);
            containerRegistry.RegisterSingleton<Shell>();
        }

        protected override async void OnInitialized()
        {
            //HttpClient = new HttpServiceClient(ConfigHelper.HttpServerUrl);
            //bool isRunning = false;

            //try
            //{
            //    isRunning = await HttpClient.PingServiceAsync();
            //}
            //catch (Exception ex)
            //{
            //    AppServices.AppLogger.Error("Ping failed", ex);
            //    isRunning = false;
            //}

            //if (!isRunning)
            //{
            //    MessageBox.Show(
            //        "Service not reachable. Cannot start application.",
            //        "Startup Error",
            //        MessageBoxButton.OK,
            //        MessageBoxImage.Error);

            //    Application.Current.Shutdown();
            //    return;
            //}

            base.OnInitialized();

            var pipe = new IpcNamedPipeClient();
            bool isRunning = false;

            try
            {
                // Check if the background Windows Service is reachable
                isRunning = await pipe.PingAsync();
            }
            catch (Exception ex)
            {
                AppServices.AppLogger.Error("Pipe ping failed", ex);
                isRunning = false;
            }

            if (!isRunning)
            {
                MessageBox.Show(
                    "Background service not reachable. Cannot start application.",
                    "Startup Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                Application.Current.Shutdown();
                return;
            }

            try
            {
                // Example: ask the service to start the sensor
                await pipe.SendMessageAsync("StartSensor");
            }
            catch (Exception ex)
            {
                AppServices.AppLogger.Error("Failed to send StartSensor command via pipe", ex);

                MessageBox.Show(
                    "Unable to communicate with background service.",
                    "Communication Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                Application.Current.Shutdown();
                return;
            }
        }
    }
}
