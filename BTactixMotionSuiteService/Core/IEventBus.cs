using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTactixMotionSuiteService.Core
{
    public interface IEventBus
    {
        void Publish<T>(T item);
        void Subscribe<T>(Action<T> handler);
    }
}
