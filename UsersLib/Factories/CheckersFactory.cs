using UsersLib.Checkers;

namespace UsersLib.Factories
{
    public class CheckersFactory : ICheckersFactory
    {
        public IUserChecker CreateUserChecker()
        {
            return new UserChecker( new UserCheckerImpl() );
        }
    }
}