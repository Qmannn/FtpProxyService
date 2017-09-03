using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UsersLib.DbControllers;
using UsersLib.Entity;

namespace UsersLib.Service.Auth
{
    internal class UserAuthorizer : IAuthorizer
    {
        private readonly IDbAuthController _dbAuthController;

        public UserAuthorizer(IDbAuthController dbAuthController)
        {
            _dbAuthController = dbAuthController;
        }

        public bool ValidateCredentials(string userName, string password, UserRoleKind roleRequred = UserRoleKind.Admin)
        {
            string passwordHash = GetPasswordHashString(password);
            UserAccount userAccount = _dbAuthController.GetUserAccount(userName, passwordHash);
            if (userAccount == null)
            {
                return false;
            }

            if (roleRequred != UserRoleKind.Unknown)
            {
                List<UserRoleKind> userRoles = _dbAuthController.GetUserRoles(userAccount.UserId);
                return userRoles.Contains(roleRequred);
            }
            return true;
        }

        private string GetPasswordHashString(string password)
        {
            SHA256 hasher = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            string hash = Encoding.UTF8.GetString(hasher.ComputeHash(bytes));
            return hash;
        }
    }
}