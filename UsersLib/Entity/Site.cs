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
        }

        public int Id { get; set; }

        public string SiteKey { get; set; }

        public string Description { get; set; }

        public DbSite ConvertToDbSite()
        {
            return new DbSite
            {
                SiteId = Id,
                Description = Description,
                SiteKey = SiteKey
            };
        }
    }
}