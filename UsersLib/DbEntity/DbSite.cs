using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersLib.DbEntity
{
    [Table( "Site" )]
    public class DbSite
    {
        [Key]
        public virtual int SiteId { get; set; }

        public virtual string SiteKey { get; set; }

        public virtual string StorageId { get; set; }

        public virtual string Description { get; set; }

        public virtual string Name { get; set; }

        public virtual string Address { get; set; }

        public virtual int Port { get; set; }

        public virtual string Login { get; set; }

        public virtual string Password { get; set; }

        public virtual ICollection<DbGroup> Groups { get; set; }
    }
}