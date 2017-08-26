using System;
using System.Data.Entity.Migrations;
using System.Linq;
using UsersLib.DbContextSettings;
using UsersLib.DbEntity;
using UsersLib.Entity;

namespace UsersLib.DbControllers
{
    public class DbAuthController : IDbAuthController
    {
        public UserRole GetUserRole( string login )
        {
            using ( UsersLibDbContext dbContext = new UsersLibDbContext() )
            {
                DbUserRole userRole = dbContext.UserRoles.FirstOrDefault( item => item.Login == login );
                return userRole?.Role ?? UserRole.Unknown;
            }
        }

        public DateTime? GetAccessTime( string login, string password )
        {
            using ( UsersLibDbContext dbContext = new UsersLibDbContext() )
            {
                DbUserAccess userAccess = dbContext.UserAccess
                    .FirstOrDefault( item => item.Login == login && item.Password == password );

                return userAccess?.AccessTime;
            }
        }

        public void SetAccessTime( string login, string password )
        {
            using ( UsersLibDbContext dbContext = new UsersLibDbContext() )
            {
                DbUserAccess userAccess = new DbUserAccess
                {
                    AccessTime = DateTime.Now,
                    Password = password,
                    Login = login
                };

                dbContext.UserAccess.AddOrUpdate( userAccess );
                dbContext.SaveChanges();
            }
        }
    }
}