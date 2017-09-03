using System;
using System.ComponentModel.DataAnnotations;

namespace UsersLib.Entity
{
    public class AccessLog
    {
        [Key]
        public long AccesslogId { get; set; }

        public string Login { get; set; }
        
        public DateTime AccessTime { get; set; }

        public string Role { get; set; }

        public bool IsAutenticated { get; set; }

        public string AccessTarget { get; set; }

        public AccessLog()
        {
            AccessTime = DateTime.Now;
        }
    }
}