using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace RPS.Api
{
    public class ServiceResolver : IDependencyResolver
    {
        private readonly IDependencyScope _scope;

        public ServiceResolver(IDependencyScope scope)
        {
            _scope = scope;
        }

        public void Dispose()
        {
            _scope.Dispose();
        }

        public object GetService(Type serviceType)
        {
            return _scope.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
           return _scope.GetServices(serviceType);
        }

        public IDependencyScope BeginScope()
        {
            return _scope;
        }
    }
}