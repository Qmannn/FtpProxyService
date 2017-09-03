using UsersLib.Checkers;
using UsersLib.DbControllers;
using UsersLib.Service.Auth;
using UsersLib.Service.Checkers;
using UsersLib.Service.Savers;

namespace UsersLib.Service.Factories
{
    public interface IUsersLIbEntityFactory
    {
        IUserChecker CreateUserChecker();
        ISiteSaver CreateSiteSaver();
        IUserSaver CreateUserSaver();
        IAuthorizer CreateAuthorizer();
        IDbAuthController CreateDbAuthController();
    }
}