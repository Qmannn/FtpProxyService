using UsersLib.Entity;

namespace UsersLib.Service.Auth
{
    public interface ILdapAuthorizer
    {
        bool ValidateCredentials( string userName, string password, UserRole roleRequred = UserRole.Admin);
    }
}