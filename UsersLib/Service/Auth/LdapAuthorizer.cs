using System;
using System.DirectoryServices.AccountManagement;
using System.Security.Cryptography;
using System.Text;
using UsersLib.Configuration;
using UsersLib.DbControllers;
using UsersLib.Entity;

namespace UsersLib.Service.Auth
{
    public sealed class LdapAuthorizer : ILdapAuthorizer
    {
        private readonly IDbAuthController _dbAuthController;

        public LdapAuthorizer(IDbAuthController dbAuthController)
        {
            _dbAuthController = dbAuthController;
        }

        public bool ValidateCredentials(string userName, string password, UserRole roleRequred = UserRole.Admin)
        {
            UserRole userRole = _dbAuthController.GetUserRole(userName);
            if (roleRequred != UserRole.Unknown && roleRequred != userRole)
            {
                return false;
            }
            string passwordHash = GetPasswordHashString(password);
            DateTime? accessTime = _dbAuthController.GetAccessTime(userName, passwordHash);
            if (accessTime.HasValue && accessTime.Value.AddHours(1) > DateTime.Now)
            {
                return true;
            }
            return TryAuthorize(userName, password);
        }

        private string GetPasswordHashString(string password)
        {
            SHA256 hasher = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            string hash = Encoding.UTF8.GetString(hasher.ComputeHash(bytes));
            return hash;
        }

        private bool TryAuthorize(string userName, string password)
        {
            try
            {
                using (PrincipalContext context = new PrincipalContext(
                    ContextType.Domain,
                    Config.LdapDomain))
                {
                    bool isAuthorized = context.ValidateCredentials(userName, password);
                    if (isAuthorized)
                    {
                        _dbAuthController.SetAccessTime(userName, GetPasswordHashString(password));
                    }
                    return isAuthorized;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}