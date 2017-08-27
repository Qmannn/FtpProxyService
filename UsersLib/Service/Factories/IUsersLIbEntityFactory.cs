using UsersLib.Checkers;
using UsersLib.Service.Auth;
using UsersLib.Service.Checkers;
using UsersLib.Service.Savers;

namespace UsersLib.Service.Factories
{
    public interface IUsersLIbEntityFactory
    {
        IUserChecker CreateUserChecker();
        ISiteSaver CreateSiteSaver();
        ILdapAuthorizer CreateAuthorizer();
    }
}