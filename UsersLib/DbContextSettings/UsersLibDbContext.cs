using System.Data.Entity;
using UsersLib.DbEntity;
using UsersLib.Entity;

namespace UsersLib.DbContextSettings
{
    public class UsersLibDbContext : DbContext
    {
        public UsersLibDbContext()
            : base( "DBConnection" )
        {
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
            Database.SetInitializer<UsersLibDbContext>( null );
        }

        public DbSet<DbUser> Users { get; set; }

        public DbSet<Site> Sites { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<DbUserRole> UserRoles { get; set; }

        public DbSet<DbUserAccess> UserAccess { get; set; }

        public DbSet<SecureSiteData> SecureSiteData { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Site>()
                .HasRequired(item => item.SecureSiteData)
                .WithRequiredPrincipal(item => item.Site)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<SecureSiteData>()
                .Ignore(item => item.NeedToEncrypt);
        }
    }
}