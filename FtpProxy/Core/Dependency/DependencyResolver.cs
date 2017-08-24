using System;
using Microsoft.Practices.Unity;

namespace FtpProxy.Core.Dependency
{
    public static class DependencyResolver
    {
        private static readonly Lazy<UnityContainer> ContainerLazy =
            new Lazy<UnityContainer>(GetUnityContainer);
        
        public static UnityContainer Container
        {
            get { return ContainerLazy.Value; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability",
            "CA2000:Dispose objects before losing scope",
            Justification =
                "Resolver живет до конца жизни приложения, поэтому освобождать его не нужно, он освободится автоматически с помощью GC"
            )]
        private static UnityContainer GetUnityContainer()
        {
            UnityContainer container = new UnityContainer();
            return DependencyRegistrator.Register(container);
        }
    }
}