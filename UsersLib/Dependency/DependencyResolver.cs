using System;
using UsersLib.Dependency.Ninject;

namespace UsersLib.Dependency
{
    /// <summary>
    /// Предоставляет доступ к резолверу зависимостей
    /// </summary>
    internal static class DependencyResolver
    {
        private static readonly Lazy<NinjectDependencyResolver> ResolverLazy = new Lazy<NinjectDependencyResolver>(GetNinjectResolver);

        /// <summary>
        /// Предоставляет доступ к текущему резолверу зависимостей. Резолвер один на все потоки.
        /// </summary>
        /// <remarks>
        /// Доступ к свойству является потокобезопасным
        /// </remarks>
        internal static IDependencyResolver Resolver => ResolverLazy.Value;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
            Justification = "Resolver живет до конца жизни приложения, поэтому освобождать его не нужно, он освободится автоматически с помощью GC")]
        private static NinjectDependencyResolver GetNinjectResolver()
        {
            NinjectDependencyResolver ninjectDependencyResolver = new NinjectDependencyResolver(new NinjectKernelFactory().GetNinjectKernel());

            return ninjectDependencyResolver;
        }
    }
}