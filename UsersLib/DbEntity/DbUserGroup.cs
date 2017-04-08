﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersLib.DbEntity
{
    [Table( "UserGroup" )]
    public class DbUserGroup
    {
        [Key]
        public virtual int UserGroupId { get; set; }

        public virtual string Name { get; set; }

        public virtual List<DbUser> Users { get; set; }
        
        public virtual List<DbSiteGroup> SiteGroups { get; set; }
    }
}