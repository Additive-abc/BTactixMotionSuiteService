using BTactix.Common.Impelentations;
using BTactix.Common.Interfaces;
using BTactixMotionSuiteService.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTactixMotionSuiteService.Services
{
    public class DataExportService : BaseService, IDataExportService
    {
        public DataExportService(IAppLoggerFactory appLoggerFactory, IErrorHandler errorHandler)
         : base(appLoggerFactory, errorHandler)
        {

        }

        //private void ValidateExport()
        //{
        //    ErrorHandler.Execute(() =>
        //    {
        //        Logger.Info("Validating data...");
        //    }, Logger, "ValidateExport");
        //}
    }
}
