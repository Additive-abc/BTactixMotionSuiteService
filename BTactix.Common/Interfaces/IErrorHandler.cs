using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTactix.Common.Interfaces
{
    public interface IErrorHandler
    {
        Task<T> ExecuteAsync<T>(Func<Task<T>> func, ILog logger, string context);
        //Task ExecuteAsync(Func<Task> action, ILog logger, string contextMessage = "");
        void Execute(Action action, ILog logger, string contextMessage = "");

        Task ExecuteAsync(Func<Task> func, ILog logger, string context);
    }
}
