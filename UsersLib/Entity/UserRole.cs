﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersLib.Entity
{
    public class UserRole
    {
        public int UserId { get; set; }
        
        public UserRoleKind Role { get; set; }

        public virtual User User { get; set; }
    }
}