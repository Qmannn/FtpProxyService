using System.Collections.Generic;
using UsersLib.Entity;

namespace Proxynet.Models
{
    public class EditUserViewModel
    {
        public User User { get; set; }

        public List<Group> UserGroups { get; set; }

        public List<Group> Groups { get; set; }
    }
}