using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Microsoft.Owin.Security;
using System.Web;
using System.Diagnostics.CodeAnalysis;
using System.Web.Http;
using Proxynet.Service.Converters;
using UsersLib.DbControllers;
using UsersLib.Secure.ActiveDirectory;
using UsersLib.Secure.Auth;
using Proxynet.Service.Savers;
using Unity.WebApi;

namespace Proxynet
{
    [ExcludeFromCodeCoverage]
    public static class UnityConfig
    {
        public static void RegisterComponents(HttpConfiguration config)
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IAuthenticationManager>(
                new InjectionFactory(o => HttpContext.Current.GetOwinContext().Authentication));

            // Controllers for working with data base
            container.RegisterType<IDbUserController, DbUserController>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDbSiteController, DbSiteController>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDbGroupController, DbGroupController>(new ContainerControlledLifetimeManager());

            container.RegisterType<IUsersUpdater, UsersUpdater>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILdapAuthorizer, LdapAuthorizer>(new ContainerControlledLifetimeManager());

            container.RegisterType<IUserDtoConverter, UserDtoConverter>(new ContainerControlledLifetimeManager());
            container.RegisterType<IGroupDtoConverter, GroupDtoConverter>(
                new ContainerControlledLifetimeManager());
            container.RegisterType<ISiteDtoConverter, SiteDtoConverter>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISiteSaver, SiteSaver>(new ContainerControlledLifetimeManager());

            // Set resolvers
            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}