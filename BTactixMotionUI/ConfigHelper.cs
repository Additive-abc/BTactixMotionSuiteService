using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTactixMotionUI
{
    public static class ConfigHelper
    {
        public static string LogFilePath =>
            ConfigurationManager.AppSettings["LogFilePath"] ?? "D:\\Logs\\Default";

        public static string HttpServerUrl =>
            ConfigurationManager.AppSettings["HttpServerUrl"] ?? "http://localhost:5000";
    }

}
