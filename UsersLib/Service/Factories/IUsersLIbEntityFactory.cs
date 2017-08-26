using UsersLib.Checkers;
using UsersLib.Service.Checkers;

namespace UsersLib.Service.Factories
{
    public interface IUsersLIbEntityFactory
    {
        IUserChecker CreateUserChecker();
    }
}