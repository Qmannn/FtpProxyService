using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Metadata.Edm;
using UsersLib.Entity;

namespace UsersLib.DbEntity
{
    [Table( "User" )]
    public class DbUser
    {
        [Key]
        public virtual int UserId { get; set; }

        public virtual string Login { get; set; }

        public virtual string DisplayName { get; set; }

        public virtual ICollection<DbUserGroup> UserGroups { get; set; }
        
        public void Fill( User user )
        {
            Login = user.Login;
            DisplayName = user.DisplayName;
        }
    }
}