using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UsersLib.DbControllers;
using UsersLib.Entity;
using UsersLib.Service.Log;

namespace UsersLib.Service.Auth
{
    internal class UserAuthorizer : IAuthorizer
    {
        private readonly IDbAuthController _dbAuthController;
        private readonly IAccessLogger _accessLogger;

        public UserAuthorizer(IDbAuthController dbAuthController, IAccessLogger accessLogger)
        {
            _dbAuthController = dbAuthController;
            _accessLogger = accessLogger;
        }

        public bool ValidateCredentials(string userName, string password, UserRoleKind roleRequred = UserRoleKind.Admin)
        {
            string passwordHash = GetPasswordHashString(password);
            UserAccount userAccount = _dbAuthController.GetUserAccount(userName, passwordHash);
            if (userAccount == null)
            {
                LogAuth(userName, roleRequred, false);
                return false;
            }

            if (roleRequred != UserRoleKind.Unknown)
            {
                List<UserRoleKind> userRoles = _dbAuthController.GetUserRoles(userAccount.UserId);
                bool roleExists = userRoles.Contains(roleRequred);
                LogAuth(userName, roleRequred, roleExists);
                return roleExists;
            }
            LogAuth(userName, roleRequred, true);
            return true;
        }

        private void LogAuth(string login, UserRoleKind role, bool isAutenticated)
        {
            _accessLogger.LogAssess(login, isAutenticated, role.ToString(), role.ToString());
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