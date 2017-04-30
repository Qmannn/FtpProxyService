using System;
using UsersLib.DbContextSettings;
using UsersLib.DbEntity;
using UsersLib.Entity;

namespace UsersLib.DbControllers
{
    public class DbGroupController : IDbGroupController
    {
        public int SaveGroup( string name )
        {
            using ( FtpProxyDbContext dbContext = new FtpProxyDbContext() )
            {
                DbGroup group = new DbGroup
                {
                    Name = name
                };

                group = dbContext.Groups.Add( group );
                dbContext.SaveChanges();
                return group.Id;
            }
        }
    }
}