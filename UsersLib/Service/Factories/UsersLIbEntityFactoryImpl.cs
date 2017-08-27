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

        public ILdapAuthorizer CreateAuthorizer()
        {
            return DependencyResolver.Resolver.GetService<ILdapAuthorizer>();
        }
    }
}