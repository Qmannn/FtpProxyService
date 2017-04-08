using System.Collections.Generic;
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
                return dbContext.Users.Select( item => new User( item ) ).ToList();
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

                return dbUser != null
                    ? dbUser.UserGroups.Select( item => new UserGroup( item ) ).ToList()
                    : new List<UserGroup>();
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
    }
}