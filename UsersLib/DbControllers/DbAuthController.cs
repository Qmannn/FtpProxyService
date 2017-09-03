using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using UsersLib.DbContextSettings;
using UsersLib.Entity;

namespace UsersLib.DbControllers
{
    public class DbAuthController : IDbAuthController
    {
        public List<UserRoleKind> GetUserRoles(int userId)
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                return dbContext.UserRole.Where(item => item.UserId == userId)
                    .Select(item => item.Role).ToList();
            }
        }

        public UserAccount GetUserAccount(string login, string password)
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                return dbContext.UserAccount
                    .FirstOrDefault(item => item.Login == login && item.Password == password);
            }
        }

        public UserAccount GetUserAccount(int userId)
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                return dbContext.UserAccount
                    .FirstOrDefault(item => item.UserId == userId);
            }
        }

        public UserAccount SaveUserAccount(UserAccount userAccount)
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                dbContext.UserAccount.AddOrUpdate(userAccount);
                dbContext.SaveChanges();
                return userAccount;
            }
        }

        public int GetUserId(string login)
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                return dbContext.UserAccount.FirstOrDefault(item => item.Login == login)?.UserId ?? 0;
            }
        }
    }
}