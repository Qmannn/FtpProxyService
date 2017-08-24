using FtpProxy.Core.Factory;
using Microsoft.Practices.Unity;

namespace FtpProxy.Core.Dependency
{
    public static class DependencyRegistrator
    {
        public static UnityContainer Register(UnityContainer container)
        {
            container.RegisterType<ICommandDispatcher, CommandDispatcher>();
            container.RegisterType<LocalCommandFactory, LocalCommandFactory>();

            return container;
        }
    }
}