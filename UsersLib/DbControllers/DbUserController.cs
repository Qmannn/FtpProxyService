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
            using ( FtpProxyDbContext dbContext = new FtpProxyDbContext() )
            {
                List<DbUser> dbUsers = dbContext.Users.ToList();
                return dbUsers.ConvertAll( item => new User( item ) ).ToList();
            }
        }

        public User GetUser( int userId )
        {
            using ( FtpProxyDbContext dbContext = new FtpProxyDbContext() )
            {
                DbUser dbUser = dbContext.Users.FirstOrDefault( item => item.UserId == userId );

                return dbUser != null
                    ? new User( dbUser )
                    : null;
            }
        }

        public Dictionary<User, List<UserGroup>> GetUsersByGroups()
        {
            using ( FtpProxyDbContext dbContext = new FtpProxyDbContext() )
            {
                return dbContext.Users.Include(user => user.UserGroups)
                    .ToDictionary(item => new User(item),
                        item => item.UserGroups.ToList().ConvertAll(group => new UserGroup(group)));
            }
        }

        public List<UserGroup> GetUserGroups( string userLogin )
        {
            DbUser dbUser;
            using ( FtpProxyDbContext dbContext = new FtpProxyDbContext() )
            {
                dbUser = dbContext.Users.FirstOrDefault( user => user.Login == userLogin );
            }
            if ( dbUser == null )
            {
                return new List<UserGroup>();
            }
            return GetUserGroups( dbUser.UserId );
        }

        public List<UserGroup> GetUserGroups( int userId )
        {
            using ( FtpProxyDbContext dbContext = new FtpProxyDbContext() )
            {
                DbUser dbUser = dbContext.Users.FirstOrDefault( item => item.UserId == userId );

                return dbUser?.UserGroups.Select( item => new UserGroup( item ) ).ToList() ?? new List<UserGroup>();
            }
        }

        public User GetUser( string userLogin )
        {
            using ( FtpProxyDbContext dbContext = new FtpProxyDbContext() )
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

            using ( FtpProxyDbContext dbContext = new FtpProxyDbContext() )
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

            using (FtpProxyDbContext dbContext = new FtpProxyDbContext())
            {
                DbUser user = dbContext.Users.Find( userId );
                List<DbUserGroup> groups = dbContext.UserGroups
                    .Where( item => userGroupIds.Contains( item.UserGroupId ) )
                    .ToList();
                if ( user != null )
                {
                    user.UserGroups.Clear();
                    foreach ( DbUserGroup userGroup in groups )
                    {
                        user.UserGroups.Add( userGroup );
                    }
                    dbContext.SaveChanges();
                }
            }
        }

        public List<UserGroup> GetUserGroups()
        {
            using ( FtpProxyDbContext dbContext = new FtpProxyDbContext() )
            {
                List<DbUserGroup> dbUserGroups = dbContext.UserGroups.ToList();
                return dbUserGroups.ConvertAll( item => new UserGroup( item ) );
            }
        }
    }
}