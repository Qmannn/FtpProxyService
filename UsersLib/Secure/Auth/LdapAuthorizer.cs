using System;
using System.DirectoryServices.AccountManagement;
using System.Security.Cryptography;
using System.Text;
using UsersLib.Configuration;
using UsersLib.DbControllers;
using UsersLib.Entity;

namespace UsersLib.Secure.Auth
{
    public sealed class LdapAuthorizer : ILdapAuthorizer
    {
        public bool ValidateCredentials( string userName, string password, bool adminRequred = true )
        {
            return true;
            IDbAuthController dbAuthController = new DbAuthController();
            UserRole userRole = dbAuthController.GetUserRole( userName );
            if ( adminRequred && userRole != UserRole.Admin )
            {
                return false;
            }

            string passwordHash = GetPasswordHashString( password );
            DateTime? accessTime = dbAuthController.GetAccessTime( userName, passwordHash );

            if ( accessTime.HasValue && accessTime.Value.AddHours( 1 ) > DateTime.Now )
            {
                return true;
            }

            try
            {
                using ( PrincipalContext context = new PrincipalContext(
                    ContextType.Domain,
                    Config.LdapDomain ) )
                {
                    bool isAuthorized = context.ValidateCredentials( userName, password );
                    if ( isAuthorized )
                    {
                        dbAuthController.SetAccessTime( userName, passwordHash );
                    }
                    return isAuthorized;
                }
            }
            catch
            {
                return false;
            }
        }

        private string GetPasswordHashString( string password )
        {
            SHA256 hasher = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes( password );
            string hash = Encoding.UTF8.GetString( hasher.ComputeHash( bytes ) );
            return hash;
        }
    }
}