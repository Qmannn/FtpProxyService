using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UsersLib.DbEntity;

namespace UsersLib.Entity
{
    public class Site
    {
        [Key]
        public virtual int SiteId { get; set; }

        public virtual string SiteKey { get; set; }

        public virtual string Description { get; set; }

        public virtual string Name { get; set; }

        public virtual ICollection<DbGroup> Groups { get; set; }

        public virtual SecureSiteData SecureSiteData { get; set; }
    }
}