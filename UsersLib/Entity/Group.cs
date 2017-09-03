using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UsersLib.Entity
{
    public class Group
    {
        [Key]
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Site> Sites { get; set; }
    }
}