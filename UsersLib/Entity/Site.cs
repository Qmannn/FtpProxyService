using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UsersLib.Entity
{
    public class Site
    {
        [Key]
        public virtual int SiteId { get; set; }

        public virtual string SiteKey { get; set; }

        public virtual string Description { get; set; }

        public virtual string Name { get; set; }

        public virtual ICollection<Group> Groups { get; set; }

        public virtual SecureSiteData SecureSiteData { get; set; }
    }
}