using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using Ninject.Parameters;

namespace UsersLib.Dependency.Ninject
{
    /// <summary>
    /// Реализует адаптер из Ninject-контейнера (IResolutionRoot) в IDependencyResolver
    /// </summary>
    internal sealed class NinjectDependencyResolver : IDependencyResolver, IDisposable
    {
        private bool _disposed;
        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        private object GetService(Type serviceType)
        {
            //Реализация подсмотрена здесь https://github.com/ninject/Ninject.Web.Mvc/blob/master/mvc3/src/Ninject.Web.Mvc/NinjectDependencyResolver.cs
            var request = _kernel.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return _kernel.Resolve(request).SingleOrDefault();
        }

        private IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType).ToList();
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        public IEnumerable<T> GetServices<T>()
        {
            return GetServices(typeof(T)).Cast<T>();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _kernel.Dispose();

            _disposed = true;
        }
    }
}
