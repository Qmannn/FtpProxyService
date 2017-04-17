using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using Microsoft.Owin.Security;
using System.Web;
using System.Diagnostics.CodeAnalysis;
using UsersLib.DbControllers;

namespace Proxynet
{
    [ExcludeFromCodeCoverage]
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            
            container.RegisterType<IAuthenticationManager>(
                new InjectionFactory( o => HttpContext.Current.GetOwinContext().Authentication ) );

            // Controllers for working with data base
            container.RegisterType<IDbUserController, DbUserController>( new ContainerControlledLifetimeManager() );
            container.RegisterType<IDbSiteController, DbSiteController>( new ContainerControlledLifetimeManager() );

            DependencyResolver.SetResolver( new UnityDependencyResolver( container ) );
        }
    }
}