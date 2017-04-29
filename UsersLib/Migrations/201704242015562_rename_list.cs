namespace UsersLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rename_list : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DbUserDbUserGroups", "DbUser_UserId", "dbo.User");
            DropForeignKey("dbo.DbUserDbUserGroups", "DbUserGroup_UserGroupId", "dbo.UserGroup");
            DropIndex("dbo.DbUserDbUserGroups", new[] { "DbUser_UserId" });
            DropIndex("dbo.DbUserDbUserGroups", new[] { "DbUserGroup_UserGroupId" });
            AddColumn("dbo.User", "DbUserGroup_UserGroupId", c => c.Int());
            CreateIndex("dbo.User", "DbUserGroup_UserGroupId");
            AddForeignKey("dbo.User", "DbUserGroup_UserGroupId", "dbo.UserGroup", "UserGroupId");
            DropTable("dbo.DbUserDbUserGroups");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DbUserDbUserGroups",
                c => new
                    {
                        DbUser_UserId = c.Int(nullable: false),
                        DbUserGroup_UserGroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DbUser_UserId, t.DbUserGroup_UserGroupId });
            
            DropForeignKey("dbo.User", "DbUserGroup_UserGroupId", "dbo.UserGroup");
            DropIndex("dbo.User", new[] { "DbUserGroup_UserGroupId" });
            DropColumn("dbo.User", "DbUserGroup_UserGroupId");
            CreateIndex("dbo.DbUserDbUserGroups", "DbUserGroup_UserGroupId");
            CreateIndex("dbo.DbUserDbUserGroups", "DbUser_UserId");
            AddForeignKey("dbo.DbUserDbUserGroups", "DbUserGroup_UserGroupId", "dbo.UserGroup", "UserGroupId", cascadeDelete: true);
            AddForeignKey("dbo.DbUserDbUserGroups", "DbUser_UserId", "dbo.User", "UserId", cascadeDelete: true);
        }
    }
}
