using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using UsersLib.DbContextSettings;
using UsersLib.Entity;

namespace UsersLib.DbControllers
{
    public class DbUserController : IDbUserController
    {
        public List<User> GetUsers()
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                List<User> dbUsers = dbContext.Users.ToList();
                return dbUsers.ToList();
            }
        }

        public User GetUser(int userId)
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                return dbContext.Users.FirstOrDefault(item => item.UserId == userId);
            }
        }

        public Dictionary<User, List<Group>> GetUsersByGroups()
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                return dbContext.Users.Include(user => user.Groups)
                    .ToDictionary(item => item, item => item.Groups.ToList());
            }
        }

        public void SaveUser(User user)
        {
            if (user == null)
            {
                return;
            }

            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                dbContext.Users.AddOrUpdate(user);
                dbContext.SaveChanges();
            }
        }

        public void SaveUserGroups(int userId, List<int> userGroupIds)
        {
            if (userId == 0)
            {
                return;
            }

            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                User user = dbContext.Users.Find(userId);
                List<Group> groups = dbContext.Groups
                    .Where(item => userGroupIds.Contains(item.Id))
                    .ToList();
                if (user != null)
                {
                    user.Groups.Clear();
                    foreach (Group userGroup in groups)
                    {
                        user.Groups.Add(userGroup);
                    }
                    dbContext.SaveChanges();
                }
            }
        }

        //public int UpdateUsers(List<User> users)
        //{
        //    using (UsersLibDbContext dbContext = new UsersLibDbContext())
        //    {
        //        List<string> existingUsers = dbContext.Users.Select(item => item.Login).ToList();
        //        users.RemoveAll(user => existingUsers.Contains(user.Login));
        //        List<User> addedUsers = dbContext.Users.AddRange(users).ToList();
        //        dbContext.SaveChanges();
        //        return addedUsers.Count;
        //    }
        //}

        public List<Group> GetUserGroups(int userId)
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                return dbContext.Users
                    .Where(item => item.UserId == userId)
                    .SelectMany(item => item.Groups)
                    .ToList();
            }
        }

        public void DeleteUser(int userId)
        {
            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                IEnumerable<User> users = dbContext.Users.Where(item => item.UserId == userId);
                dbContext.Users.RemoveRange(users);
                dbContext.SaveChanges();
            }
        }
    }
}