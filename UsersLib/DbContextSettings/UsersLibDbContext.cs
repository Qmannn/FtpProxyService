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
            // ReSharper disable once UnusedVariable
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
            Database.SetInitializer<UsersLibDbContext>(new DbInitializer());
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Site> Sites { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<UserRole> UserRole { get; set; }

        public DbSet<DbUserAccess> UserAccess { get; set; }

        public DbSet<SecureSiteData> SecureSiteData { get; set; }

        public DbSet<UserAccount> UserAccount { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Site>()
                .HasRequired(item => item.SecureSiteData)
                .WithRequiredPrincipal(item => item.Site)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<SecureSiteData>()
                .Ignore(item => item.NeedToEncrypt);

            modelBuilder.Entity<User>()
                .HasRequired(item => item.UserAccount)
                .WithRequiredPrincipal(item => item.User)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<UserRole>()
                .HasKey(item => new {item.UserId, item.Role});

            modelBuilder.Entity<User>()
                .HasMany(item => item.UserRoles)
                .WithRequired(item => item.User)
                .WillCascadeOnDelete(true);
        }
    }
}