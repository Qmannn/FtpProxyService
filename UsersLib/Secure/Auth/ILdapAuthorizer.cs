namespace UsersLib.Secure.Auth
{
    public interface ILdapAuthorizer
    {
        bool ValidateCredentials( string userName, string password, bool adminRequred = true );
    }
}