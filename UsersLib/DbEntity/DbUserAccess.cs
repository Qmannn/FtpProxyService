using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersLib.DbEntity
{
    [Table("user_access")]
    public class DbUserAccess
    {
        [Key]
        public virtual string Login { get; set; }

        public virtual DateTime AccessTime { get; set; }

        public virtual string Password { get; set; }
    }
}