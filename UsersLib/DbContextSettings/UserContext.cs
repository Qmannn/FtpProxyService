using System.Data.Entity;
using UsersLib.DbEntity;

namespace UsersLib.DbContextSettings
{
    public class FtpProxyDbContext : DbContext
    {
        public FtpProxyDbContext()
            : base( "DBConnection" )
        {
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
            Database.SetInitializer<FtpProxyDbContext>( null );
        }

        public DbSet<DbUser> Users { get; set; }

        public DbSet<DbSite> Sites { get; set; }

        public DbSet<DbGroup> Groups { get; set; }

        public DbSet<DbUserRole> UserRoles { get; set; }

        public DbSet<DbUserAccess> UserAccess { get; set; }
    }
}