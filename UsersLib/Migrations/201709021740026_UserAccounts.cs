namespace UsersLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAccounts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sites",
                c => new
                    {
                        SiteId = c.Int(nullable: false, identity: true),
                        SiteKey = c.String(),
                        Description = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.SiteId);
            
            CreateTable(
                "dbo.SecureSiteDatas",
                c => new
                    {
                        SiteId = c.Int(nullable: false),
                        Url = c.String(),
                        Port = c.Int(nullable: false),
                        Login = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.SiteId)
                .ForeignKey("dbo.Sites", t => t.SiteId, cascadeDelete: true)
                .Index(t => t.SiteId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        DisplayName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.UserAccounts",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        Login = c.String(),
                        Password = c.String(),
                        NeedChangePassword = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        Role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.Role })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserAccess",
                c => new
                    {
                        Login = c.String(nullable: false, maxLength: 128),
                        AccessTime = c.DateTime(nullable: false),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Login);
            
            CreateTable(
                "dbo.SiteGroups",
                c => new
                    {
                        Site_SiteId = c.Int(nullable: false),
                        Group_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Site_SiteId, t.Group_Id })
                .ForeignKey("dbo.Sites", t => t.Site_SiteId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .Index(t => t.Site_SiteId)
                .Index(t => t.Group_Id);
            
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        User_UserId = c.Int(nullable: false),
                        Group_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_UserId, t.Group_Id })
                .ForeignKey("dbo.Users", t => t.User_UserId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .Index(t => t.User_UserId)
                .Index(t => t.Group_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserAccounts", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserGroups", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.UserGroups", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.SecureSiteDatas", "SiteId", "dbo.Sites");
            DropForeignKey("dbo.SiteGroups", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.SiteGroups", "Site_SiteId", "dbo.Sites");
            DropIndex("dbo.UserGroups", new[] { "Group_Id" });
            DropIndex("dbo.UserGroups", new[] { "User_UserId" });
            DropIndex("dbo.SiteGroups", new[] { "Group_Id" });
            DropIndex("dbo.SiteGroups", new[] { "Site_SiteId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserAccounts", new[] { "UserId" });
            DropIndex("dbo.SecureSiteDatas", new[] { "SiteId" });
            DropTable("dbo.UserGroups");
            DropTable("dbo.SiteGroups");
            DropTable("dbo.UserAccess");
            DropTable("dbo.UserRoles");
            DropTable("dbo.UserAccounts");
            DropTable("dbo.Users");
            DropTable("dbo.SecureSiteDatas");
            DropTable("dbo.Sites");
            DropTable("dbo.Groups");
        }
    }
}
