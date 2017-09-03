using Ninject.Modules;
using UsersLib.DbControllers;
using UsersLib.Service.Auth;
using UsersLib.Service.Checkers;
using UsersLib.Service.Cryptography;
using UsersLib.Service.Log;
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
            Bind<IDbLogController>().To<DbLogController>();
            Bind<IAuthorizer>().To<UserAuthorizer>();
            Bind<ISecureSiteDataResolver>().To<SecureSiteDataResolver>();
            Bind<ICryptoService>().To<CryptoService>();
            Bind<IDbAuthController>().To<DbAuthController>();
            Bind<ISiteSaver>().To<SiteSaver>();
            Bind<IUserSaver>().To<UserSaver>();
            Bind<IAccessLogger>().To<AccessLogger>();
        }
    }
}