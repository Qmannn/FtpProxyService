using Accounts.Checkers;

namespace Accounts.Factories
{
    public interface ICheckersFactory
    {
        IUserChecker CreateUserChecker();
    }
}