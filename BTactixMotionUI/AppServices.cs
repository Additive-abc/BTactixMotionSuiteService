using BTactix.Common.Impelentations;
using BTactix.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTactixMotionUI
{
    public static class AppServices
    {
        public static readonly IAppLoggerFactory LoggerFactory;
        public static readonly IErrorHandler ErrorHandler;

        static AppServices()
        {
            // Initialize once for entire app
            LoggerFactory = new AppLoggerFactory("D:\\Logs\\BTactixMotionUI.log");
            ErrorHandler = new ErrorHandler();
            AppLogger = LoggerFactory.CreateLogger(typeof(App));
        }

        public static readonly log4net.ILog AppLogger;
    }
}
