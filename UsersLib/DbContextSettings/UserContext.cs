using System.Data.Entity;
using UsersLib.DbEntity;
using UsersLib.Entity;

namespace UsersLib.DbContextSettings
{
    public class FtpProxyDbContext : DbContext
    {
        public FtpProxyDbContext()
            : base( "DBConnection" )
        {
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public DbSet<DbUser> Users { get; set; }

        public DbSet<DbSite> Sites { get; set; }

        public DbSet<DbUserGroup> UserGroups { get; set; }

        public DbSet<DbSiteGroup> SiteGroups { get; set; }
    }
}