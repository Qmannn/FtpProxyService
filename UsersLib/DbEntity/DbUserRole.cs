using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UsersLib.Entity;

namespace UsersLib.DbEntity
{
    [Table("user_role")]
    public class DbUserRole
    {
        [Key]
        public virtual string Login { get; set; }

        public virtual UserRole Role { get; set; }
    }
}