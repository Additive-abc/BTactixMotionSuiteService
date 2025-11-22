using BTactix.Common.Interfaces;
using log4net;

namespace BTactix.Common.Impelentations
{
    public abstract class BaseService
    {
        protected ILog Logger { get; }
        protected IErrorHandler ErrorHandler { get; }

        protected BaseService(IAppLoggerFactory loggerFactory, IErrorHandler errorHandler)
        {
            Logger = loggerFactory.CreateLogger(this.GetType());
            ErrorHandler = errorHandler;
        }
    }
}
