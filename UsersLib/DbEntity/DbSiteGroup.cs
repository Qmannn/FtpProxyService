using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersLib.DbEntity
{
    [Table( "SiteGroup" )]
    public class DbSiteGroup
    {
        [Key]
        public virtual int SiteGroupId { get; set; }

        public virtual string Name { get; set; }

        public virtual List<DbSite> Sites { get; set; }

        public virtual List<DbUserGroup> UserGroups { get; set; }
    }
}