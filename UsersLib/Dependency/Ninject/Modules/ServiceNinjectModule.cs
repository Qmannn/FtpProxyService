using Ninject.Modules;
using UsersLib.DbControllers;
using UsersLib.Service.Auth;
using UsersLib.Service.Checkers;
using UsersLib.Service.Cryptography;
using UsersLib.Service.Resolvers;
using UsersLib.Service.Savers;

namespace UsersLib.Dependency.Ninject.Modules
{
    internal class ServiceNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUserChecker>().To<UserChecker>();
            Bind<IDbUserController>().To<DbUserController>();
            Bind<IDbSiteController>().To<DbSiteController>();
            Bind<ILdapAuthorizer>().To<LdapAuthorizer>();
            Bind<ISecureSiteDataResolver>().To<SecureSiteDataResolver>();
            Bind<ICryptoService>().To<CryptoService>();
            Bind<IDbAuthController>().To<DbAuthController>();
            Bind<ISiteSaver>().To<SiteSaver>();
        }
    }
}