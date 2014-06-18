using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

namespace RPS.Api
{
    public class StaticScope : IDependencyScope
    {
        private readonly Dictionary<Type, object> _d;

        public StaticScope()
        {
            _d = new Dictionary<Type, object>();
        }

        public StaticScope Add<T>(T service)
        {
            _d.Add(typeof(T), service);
            return this;
        }
        object IDependencyScope.GetService(Type serviceType)
        {
            object s;
            _d.TryGetValue(serviceType, out s);
            return s;
        }

        IEnumerable<object> IDependencyScope.GetServices(Type serviceType)
        {
            return _d
                .Where(x => x.Key == serviceType)
                .Select(s => s.Value)
                .ToList();
        }

        void IDisposable.Dispose()
        {
            _d.Clear();
        }
    }
}