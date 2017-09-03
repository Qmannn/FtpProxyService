using UsersLib.DbControllers;
using UsersLib.Dependency;
using UsersLib.Service.Auth;
using UsersLib.Service.Checkers;
using UsersLib.Service.Savers;

namespace UsersLib.Service.Factories
{
    internal class UsersLIbEntityFactoryImpl : IUsersLIbEntityFactory
    {
        public IUserChecker CreateUserChecker()
        {
            return DependencyResolver.Resolver.GetService<IUserChecker>();
        }

        public ISiteSaver CreateSiteSaver()
        {
            return DependencyResolver.Resolver.GetService<ISiteSaver>();
        }

        public IUserSaver CreateUserSaver()
        {
            return DependencyResolver.Resolver.GetService<IUserSaver>();
        }

        public IAuthorizer CreateAuthorizer()
        {
            return DependencyResolver.Resolver.GetService<IAuthorizer>();
        }

        public IDbAuthController CreateDbAuthController()
        {
            return DependencyResolver.Resolver.GetService<IDbAuthController>();
        }
    }
}