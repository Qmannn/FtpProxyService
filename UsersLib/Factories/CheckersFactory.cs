using UsersLib.Checkers;
using UsersLib.DbControllers;
using UsersLib.Secure.Auth;

namespace UsersLib.Factories
{
    public class CheckersFactory : ICheckersFactory
    {
        public IUserChecker CreateUserChecker()
        {
            return new UserChecker( new DataBaseUserChecker(
                new DbUserController(),
                new DbSiteController(),
                new SecureFindersFactory(),
                new LdapAuthorizer() ) );
        }
    }
}