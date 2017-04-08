namespace UsersLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DbSiteGroups",
                c => new
                    {
                        SiteGroupId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.SiteGroupId);
            
            CreateTable(
                "dbo.DbSites",
                c => new
                    {
                        SiteId = c.Int(nullable: false, identity: true),
                        SiteKey = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.SiteId);
            
            CreateTable(
                "dbo.DbUserGroups",
                c => new
                    {
                        UserGroupId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.UserGroupId);
            
            CreateTable(
                "dbo.DbUsers",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                        DisplayName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.DbSiteDbSiteGroups",
                c => new
                    {
                        DbSite_SiteId = c.Int(nullable: false),
                        DbSiteGroup_SiteGroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DbSite_SiteId, t.DbSiteGroup_SiteGroupId })
                .ForeignKey("dbo.DbSites", t => t.DbSite_SiteId, cascadeDelete: true)
                .ForeignKey("dbo.DbSiteGroups", t => t.DbSiteGroup_SiteGroupId, cascadeDelete: true)
                .Index(t => t.DbSite_SiteId)
                .Index(t => t.DbSiteGroup_SiteGroupId);
            
            CreateTable(
                "dbo.DbUserGroupDbSiteGroups",
                c => new
                    {
                        DbUserGroup_UserGroupId = c.Int(nullable: false),
                        DbSiteGroup_SiteGroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DbUserGroup_UserGroupId, t.DbSiteGroup_SiteGroupId })
                .ForeignKey("dbo.DbUserGroups", t => t.DbUserGroup_UserGroupId, cascadeDelete: true)
                .ForeignKey("dbo.DbSiteGroups", t => t.DbSiteGroup_SiteGroupId, cascadeDelete: true)
                .Index(t => t.DbUserGroup_UserGroupId)
                .Index(t => t.DbSiteGroup_SiteGroupId);
            
            CreateTable(
                "dbo.DbUserDbUserGroups",
                c => new
                    {
                        DbUser_UserId = c.Int(nullable: false),
                        DbUserGroup_UserGroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DbUser_UserId, t.DbUserGroup_UserGroupId })
                .ForeignKey("dbo.DbUsers", t => t.DbUser_UserId, cascadeDelete: true)
                .ForeignKey("dbo.DbUserGroups", t => t.DbUserGroup_UserGroupId, cascadeDelete: true)
                .Index(t => t.DbUser_UserId)
                .Index(t => t.DbUserGroup_UserGroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DbUserDbUserGroups", "DbUserGroup_UserGroupId", "dbo.DbUserGroups");
            DropForeignKey("dbo.DbUserDbUserGroups", "DbUser_UserId", "dbo.DbUsers");
            DropForeignKey("dbo.DbUserGroupDbSiteGroups", "DbSiteGroup_SiteGroupId", "dbo.DbSiteGroups");
            DropForeignKey("dbo.DbUserGroupDbSiteGroups", "DbUserGroup_UserGroupId", "dbo.DbUserGroups");
            DropForeignKey("dbo.DbSiteDbSiteGroups", "DbSiteGroup_SiteGroupId", "dbo.DbSiteGroups");
            DropForeignKey("dbo.DbSiteDbSiteGroups", "DbSite_SiteId", "dbo.DbSites");
            DropIndex("dbo.DbUserDbUserGroups", new[] { "DbUserGroup_UserGroupId" });
            DropIndex("dbo.DbUserDbUserGroups", new[] { "DbUser_UserId" });
            DropIndex("dbo.DbUserGroupDbSiteGroups", new[] { "DbSiteGroup_SiteGroupId" });
            DropIndex("dbo.DbUserGroupDbSiteGroups", new[] { "DbUserGroup_UserGroupId" });
            DropIndex("dbo.DbSiteDbSiteGroups", new[] { "DbSiteGroup_SiteGroupId" });
            DropIndex("dbo.DbSiteDbSiteGroups", new[] { "DbSite_SiteId" });
            DropTable("dbo.DbUserDbUserGroups");
            DropTable("dbo.DbUserGroupDbSiteGroups");
            DropTable("dbo.DbSiteDbSiteGroups");
            DropTable("dbo.DbUsers");
            DropTable("dbo.DbUserGroups");
            DropTable("dbo.DbSites");
            DropTable("dbo.DbSiteGroups");
        }
    }
}
