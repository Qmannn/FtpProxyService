using System.Collections.Generic;
using UsersLib.Entity;

namespace Proxynet.Models
{
    public class EditUserViewModel
    {
        public User User { get; set; }

        public List<UserGroup> UserGroups { get; set; }

        public List<UserGroup> Groups { get; set; }
    }
}