using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UsersLib.DbControllers;
using UsersLib.Entity;

namespace UsersLib.Service.Savers
{
    internal class UserSaver : IUserSaver
    {
        private readonly IDbUserController _dbUserController;
        private readonly IDbAuthController _dbAuthController;

        public UserSaver(IDbUserController dbUserController, IDbAuthController dbAuthController)
        {
            _dbUserController = dbUserController;
            _dbAuthController = dbAuthController;
        }

        public User SaveUser(User user, UserAccount userAccount)
        {
            if (user == null)
            {
                return null;
            }

            _dbUserController.SaveUser(user);
            if (userAccount == null)
            {
                return user;
            }
            userAccount.UserId = user.UserId;
            userAccount.Password = GetPasswordHashString(userAccount.Password);
            _dbAuthController.SaveUserAccount(userAccount);
            return user;
        }

        public User SaveUser(User user)
        {
            return SaveUser(user, null);
        }

        public void SaveUserGroups(int userId, List<int> grupIds )
        {
            _dbUserController.SaveUserGroups(userId, grupIds);
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