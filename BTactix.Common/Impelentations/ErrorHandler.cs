using BTactix.Common.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTactix.Common.Impelentations
{
    public class ErrorHandler : IErrorHandler
    {
        public void Execute(Action action, ILog logger, string contextMessage = "")
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                logger.Error($"Error in {contextMessage}: {ex.Message}", ex);
            }
        }

        public Task<T> ExecuteAsync<T>(Func<Task<T>> func, ILog logger, string context)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                logger.Error($"Error in {context}", ex);
                throw;
            }
        }

        public async Task ExecuteAsync(Func<Task> func, ILog logger, string context)
        {
            try
            {
                await func();
            }
            catch (Exception ex)
            {
                logger.Error($"Error in {context}", ex);
                throw;
            }
        }
    }
}
