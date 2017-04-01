using UsersLib.Checkers;

namespace UsersLib.Factories
{
    public interface ICheckersFactory
    {
        IUserChecker CreateUserChecker();
    }
}