namespace UsersLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DownOfRename : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.User", "DbUserGroup_UserGroupId", "dbo.UserGroup");
            DropIndex("dbo.User", new[] { "DbUserGroup_UserGroupId" });
            CreateTable(
                "dbo.DbUserDbUserGroups",
                c => new
                    {
                        DbUser_UserId = c.Int(nullable: false),
                        DbUserGroup_UserGroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DbUser_UserId, t.DbUserGroup_UserGroupId })
                .ForeignKey("dbo.User", t => t.DbUser_UserId, cascadeDelete: true)
                .ForeignKey("dbo.UserGroup", t => t.DbUserGroup_UserGroupId, cascadeDelete: true)
                .Index(t => t.DbUser_UserId)
                .Index(t => t.DbUserGroup_UserGroupId);
            
            DropColumn("dbo.User", "DbUserGroup_UserGroupId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "DbUserGroup_UserGroupId", c => c.Int());
            DropForeignKey("dbo.DbUserDbUserGroups", "DbUserGroup_UserGroupId", "dbo.UserGroup");
            DropForeignKey("dbo.DbUserDbUserGroups", "DbUser_UserId", "dbo.User");
            DropIndex("dbo.DbUserDbUserGroups", new[] { "DbUserGroup_UserGroupId" });
            DropIndex("dbo.DbUserDbUserGroups", new[] { "DbUser_UserId" });
            DropTable("dbo.DbUserDbUserGroups");
            CreateIndex("dbo.User", "DbUserGroup_UserGroupId");
            AddForeignKey("dbo.User", "DbUserGroup_UserGroupId", "dbo.UserGroup", "UserGroupId");
        }
    }
}
