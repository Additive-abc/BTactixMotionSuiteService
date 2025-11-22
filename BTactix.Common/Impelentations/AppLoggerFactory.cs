using BTactix.Common.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTactix.Common.Impelentations
{
    public class AppLoggerFactory :IAppLoggerFactory
    {
        private static bool _isConfigured = false;
        private readonly string _logPath;

        public AppLoggerFactory(string logPath = null)
        {
            _logPath = logPath ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

            if (!Directory.Exists(_logPath))
                Directory.CreateDirectory(_logPath);

            ConfigureLog4Net();
        }

        private void ConfigureLog4Net()
        {
            if (_isConfigured) return;

            // Set the global property for log4net
            log4net.GlobalContext.Properties["LogPath"] = _logPath;

            var configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");

            if (File.Exists(configFile))
            {
                log4net.Config.XmlConfigurator.Configure(new FileInfo(configFile));
                _isConfigured = true;
            }
        }

        public ILog CreateLogger(Type type)
        {
            return LogManager.GetLogger(type);
        }
    }
}
