using UsersLib.DbEntity;

namespace UsersLib.Entity
{
    public class Site
    {
        public Site()
        {
            
        }

        public Site( DbSite dbSite )
        {
            Id = dbSite.SiteId;
            SiteKey = dbSite.SiteKey;
            Description = dbSite.Description;
            StorageId = dbSite.StorageId;
            Name = dbSite.Name;
            Address = dbSite.Address;
            Port = dbSite.Port;
            Login = dbSite.Login;
            Password = dbSite.Password;
        }

        public int Id { get; set; }

        public string SiteKey { get; set; }

        public string StorageId { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public int Port { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public DbSite ConvertToDbSite()
        {
            return new DbSite
            {
                SiteId = Id,
                Description = Description,
                SiteKey = SiteKey,
                StorageId = StorageId,
                Name = Name,
                Address = Address,
                Port = Port,
                Login = Login,
                Password = Password
            };
        }
    }
}