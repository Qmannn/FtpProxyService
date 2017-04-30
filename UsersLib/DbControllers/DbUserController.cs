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

        public Dictionary<User, List<Group>> GetUsersByGroups()
        {
            using ( FtpProxyDbContext dbContext = new FtpProxyDbContext() )
            {
                return dbContext.Users.Include(user => user.UserGroups)
                    .ToDictionary(item => new User(item),
                        item => item.UserGroups.ToList().ConvertAll(group => new Group(group)));
            }
        }

        public List<Group> GetUserGroups( string userLogin )
        {
            DbUser dbUser;
            using ( FtpProxyDbContext dbContext = new FtpProxyDbContext() )
            {
                dbUser = dbContext.Users.FirstOrDefault( user => user.Login == userLogin );
            }
            if ( dbUser == null )
            {
                return new List<Group>();
            }
            return GetUserGroups( dbUser.UserId );
        }

        public List<Group> GetUserGroups( int userId )
        {
            using ( FtpProxyDbContext dbContext = new FtpProxyDbContext() )
            {
                DbUser dbUser = dbContext.Users.FirstOrDefault( item => item.UserId == userId );

                return dbUser?.UserGroups.Select( item => new Group( item ) ).ToList() ?? new List<Group>();
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
                List<DbGroup> groups = dbContext.Groups
                    .Where( item => userGroupIds.Contains( item.Id ) )
                    .ToList();
                if ( user != null )
                {
                    user.UserGroups.Clear();
                    foreach ( DbGroup userGroup in groups )
                    {
                        user.UserGroups.Add( userGroup );
                    }
                    dbContext.SaveChanges();
                }
            }
        }

        public List<Group> GetUserGroups()
        {
            using ( FtpProxyDbContext dbContext = new FtpProxyDbContext() )
            {
                List<DbGroup> dbUserGroups = dbContext.Groups.ToList();
                return dbUserGroups.ConvertAll( item => new Group( item ) );
            }
        }
    }
}