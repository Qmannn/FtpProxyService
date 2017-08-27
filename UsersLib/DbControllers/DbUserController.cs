using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using UsersLib.DbContextSettings;
using UsersLib.DbEntity;
using UsersLib.Entity;

namespace UsersLib.DbControllers
{
    public class DbUserController : IDbUserController
    {
        public List<User> GetUsers()
        {
            using ( UsersLibDbContext dbContext = new UsersLibDbContext() )
            {
                List<DbUser> dbUsers = dbContext.Users.ToList();
                return dbUsers.ConvertAll( item => new User( item ) ).ToList();
            }
        }

        public User GetUser( int userId )
        {
            using ( UsersLibDbContext dbContext = new UsersLibDbContext() )
            {
                DbUser dbUser = dbContext.Users.FirstOrDefault( item => item.UserId == userId );

                return dbUser != null
                    ? new User( dbUser )
                    : null;
            }
        }

        public Dictionary<User, List<Group>> GetUsersByGroups()
        {
            using ( UsersLibDbContext dbContext = new UsersLibDbContext() )
            {
                return dbContext.Users.Include(user => user.Groups)
                    .ToDictionary(item => new User(item),
                        item => item.Groups.ToList());
            }
        }

        public List<Group> GetUserGroups( string userLogin )
        {
            DbUser dbUser;
            using ( UsersLibDbContext dbContext = new UsersLibDbContext() )
            {
                dbUser = dbContext.Users.FirstOrDefault( user => user.Login == userLogin );
            }
            if ( dbUser == null )
            {
                return new List<Group>();
            }
            return GetUserGroups( dbUser.UserId );
        }

        public User GetUser( string userLogin )
        {
            using ( UsersLibDbContext dbContext = new UsersLibDbContext() )
            {
                DbUser dbUser = dbContext.Users.FirstOrDefault( item => item.Login == userLogin );

                return dbUser != null
                    ? new User( dbUser )
                    : null;
            }
        }

        public void SaveUser( User user )
        {
            if ( user == null )
            {
                return;
            }

            using ( UsersLibDbContext dbContext = new UsersLibDbContext() )
            {
                dbContext.Users.AddOrUpdate( user.ConvertToDbUser() );
                dbContext.SaveChanges();
            }
        }

        public void SaveUserGroups( int userId, List<int> userGroupIds )
        {
            if ( userId == 0 )
            {
                return;
            }

            using (UsersLibDbContext dbContext = new UsersLibDbContext())
            {
                DbUser user = dbContext.Users.Find( userId );
                List<Group> groups = dbContext.Groups
                    .Where( item => userGroupIds.Contains( item.Id ) )
                    .ToList();
                if ( user != null )
                {
                    user.Groups.Clear();
                    foreach ( Group userGroup in groups )
                    {
                        user.Groups.Add( userGroup );
                    }
                    dbContext.SaveChanges();
                }
            }
        }

        public int UpdateUsers( List<User> users )
        {
            using ( UsersLibDbContext dbContext = new UsersLibDbContext() )
            {
                List<string> existingUsers = dbContext.Users.Select( item => item.Login ).ToList();
                users.RemoveAll( user => existingUsers.Contains( user.Login ) );
                List<DbUser> dbUsers = users.ConvertAll( item => item.ConvertToDbUser() );
                List<DbUser> addedUsers = dbContext.Users.AddRange( dbUsers ).ToList();
                dbContext.SaveChanges();
                return addedUsers.Count;
            }
        }

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
    }
}