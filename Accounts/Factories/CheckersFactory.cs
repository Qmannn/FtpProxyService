using Accounts.Checkers;

namespace Accounts.Factories
{
    public class CheckersFactory : ICheckersFactory
    {
        public IUserChecker CreateUserChecker()
        {
            return new UserChecker( new UserCheckerImpl() );
        }
    }
}