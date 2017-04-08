using UsersLib.Checkers;
using UsersLib.DbControllers;

namespace UsersLib.Factories
{
    public class CheckersFactory : ICheckersFactory
    {
        public IUserChecker CreateUserChecker()
        {
            return new UserChecker( new UserCheckerImpl() );
        }

        public IUserChecker CreateDataBaseUserChecker()
        {
            return new UserChecker( new DataBaseUserChecker(
                new DbUserController(),
                new DbSiteController(),
                new SecureFindersFactory() ) );
        }
    }
}