using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Microsoft.Owin.Security;
using System.Web;
using System.Diagnostics.CodeAnalysis;
using System.Web.Http;
using Proxynet.Service.Converters;
using Proxynet.Service.Finders;
using Proxynet.Service.Removers;
using UsersLib.DbControllers;
using Proxynet.Service.Savers;
using Proxynet.Service.Validators;
using Unity.WebApi;
using UsersLib.Service.Auth;
using UsersLib.Service.Factories;

namespace Proxynet
{
    [ExcludeFromCodeCoverage]
    public static class UnityConfig
    {
        public static void RegisterComponents(HttpConfiguration config)
        {
            var container = new UnityContainer();
            
            // InjectionFactories
            container.RegisterType<IAuthenticationManager>(
                new InjectionFactory(o => HttpContext.Current.GetOwinContext().Authentication));
            container.RegisterType<UsersLib.Service.Savers.ISiteSaver>(
                new InjectionFactory(o => UsersLIbEntityFactory.Instance.CreateSiteSaver()));
            container.RegisterType<UsersLib.Service.Savers.IUserSaver>(
                new InjectionFactory(o => UsersLIbEntityFactory.Instance.CreateUserSaver()));
            container.RegisterType<IAuthorizer>(
                new InjectionFactory(o => UsersLIbEntityFactory.Instance.CreateAuthorizer()));
            container.RegisterType<IDbAuthController>(
                new InjectionFactory(o => UsersLIbEntityFactory.Instance.CreateDbAuthController()));

            // UsersLib
            //TODO �������� �������� �� ������� UsersLIbEntityFactory
            container.RegisterType<IDbUserController, DbUserController>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDbSiteController, DbSiteController>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDbGroupController, DbGroupController>(new ContainerControlledLifetimeManager());

            // Types
            container.RegisterType<IUserDtoConverter, UserDtoConverter>(new ContainerControlledLifetimeManager());
            container.RegisterType<IGroupDtoConverter, GroupDtoConverter>(
                new ContainerControlledLifetimeManager());
            container.RegisterType<ISiteDtoConverter, SiteDtoConverter>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISiteSaver, SiteSaver>(new ContainerControlledLifetimeManager());
            container.RegisterType<IGroupDataFinder, GroupDataFinder>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISiteDataFinder, SiteDataFinder>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDataRemover, DataRemover>(new ContainerControlledLifetimeManager());
            container.RegisterType<IGroupSaver, GroupSaver>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUserSaver, UserSaver>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUserDataFinder, UserDataFinder>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUserAccountDtoConverter, UserAccountDtoConverter>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUserValidator, UserValidator>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISiteValidator, SiteValidator>(new ContainerControlledLifetimeManager());
            
            // Set resolvers
            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}