using UsersLib.Entity;

namespace UsersLib.Service.Auth
{
    public interface IAuthorizer
    {
        bool ValidateCredentials( string userName, string password, UserRoleKind roleRequred = UserRoleKind.Admin);
    }
}