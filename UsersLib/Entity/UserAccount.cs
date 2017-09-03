using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersLib.Entity
{
    public class UserAccount
    {
        [Key]
        [ForeignKey("User")]
        public int UserId { get; set; }
        
        public string Login { get; set; }
        
        public string Password { get; set; }

        public bool NeedChangePassword { get; set; }

        public virtual User User { get; set; }
    }
}