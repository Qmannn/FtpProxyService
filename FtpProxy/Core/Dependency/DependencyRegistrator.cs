using FtpProxy.Core.Builders;
using FtpProxy.Core.DataConnection;
using FtpProxy.Core.Factory;
using FtpProxy.Core.Helpers;
using FtpProxy.Core.Resolvers;
using Microsoft.Practices.Unity;
using UsersLib.Service.Checkers;
using UsersLib.Service.Factories;

namespace FtpProxy.Core.Dependency
{
    public static class DependencyRegistrator
    {
        public static UnityContainer Register(UnityContainer container)
        {
            container.RegisterType<LocalCommandFactory, LocalCommandFactory>();
            container.RegisterType<IRemoteCommandFactory, RemoteCommandFactory>();
            container.RegisterType<IServerConnectionBuilder, ServerConnectionBuilder>();
            container.RegisterType<IConnectionFactory, ConnectionFactory>();
            container.RegisterType<ICommandArgsResolver, CommandArgsResolver>();
            container.RegisterType<IDataOperationExecutor, DataOperationExecutor>();
            container.RegisterType<ICommandExecutorHelper, CommandExecutorHelper>();

            container.RegisterType<IUserChecker>(
                new InjectionFactory(o => UsersLIbEntityFactory.Instance.CreateUserChecker()));

            return container;
        }
    }
}