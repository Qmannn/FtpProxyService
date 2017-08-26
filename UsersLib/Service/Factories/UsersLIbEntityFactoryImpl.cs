using UsersLib.Dependency;
using UsersLib.Service.Checkers;

namespace UsersLib.Service.Factories
{
    internal class UsersLIbEntityFactoryImpl : IUsersLIbEntityFactory
    {
        public IUserChecker CreateUserChecker()
        {
            return DependencyResolver.Resolver.GetService<IUserChecker>();
        }
    }
}