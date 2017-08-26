using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UsersLib.Entity;

namespace UsersLib.DbEntity
{
    [Table( "Group" )]
    public class DbGroup
    {
        [Key]
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual ICollection<DbUser> Users { get; set; }

        public virtual ICollection<Site> Sites { get; set; }
    }
}