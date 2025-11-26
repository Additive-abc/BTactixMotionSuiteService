using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTactixMotionSuiteService.Core
{
    public class SimpleEventBus :IEventBus
    {
        private readonly ConcurrentDictionary<Type, ConcurrentBag<Delegate>> _subs = new();

        public void Publish<T>(T item)
        {
            if(_subs.TryGetValue(typeof(T), out var bag))
            {
                foreach(var bg in bag)
                {
                    ((Action<T>)bg)?.Invoke(item);
                }
            }
        }

        public void Subscribe<T>(Action<T> handler)
        {
            var bag = _subs.GetOrAdd(typeof(T), _ => new ConcurrentBag<Delegate>());
            bag.Add(handler);
        }
    }
}
