using UsersLib.DbEntity;

namespace UsersLib.Entity
{
    public class SiteGroup
    {
        public SiteGroup( DbSiteGroup dbSiteGroup )
        {
            Id = dbSiteGroup.SiteGroupId;
            Name = dbSiteGroup.Name;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DbSiteGroup ConvertToDbSiteGroup()
        {
            return new DbSiteGroup
            {
                SiteGroupId = Id,
                Name = Name
            };
        }
    }
}