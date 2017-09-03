using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UsersLib.DbEntity;

namespace UsersLib.Entity
{
    public class User
    {
        [Key]
        public virtual int UserId { get; set; }

        public virtual string DisplayName { get; set; }

        public virtual ICollection<Group> Groups { get; set; }

        public virtual UserAccount UserAccount { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } 
    }
}