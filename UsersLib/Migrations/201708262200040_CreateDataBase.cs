namespace UsersLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDataBase : DbMigration
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
                "dbo.User",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                        DisplayName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
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
                "dbo.UserRole",
                c => new
                    {
                        Login = c.String(nullable: false, maxLength: 128),
                        Role = c.Int(nullable: false),
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
                "dbo.DbUserGroups",
                c => new
                    {
                        DbUser_UserId = c.Int(nullable: false),
                        Group_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DbUser_UserId, t.Group_Id })
                .ForeignKey("dbo.User", t => t.DbUser_UserId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .Index(t => t.DbUser_UserId)
                .Index(t => t.Group_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DbUserGroups", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.DbUserGroups", "DbUser_UserId", "dbo.User");
            DropForeignKey("dbo.SecureSiteDatas", "SiteId", "dbo.Sites");
            DropForeignKey("dbo.SiteGroups", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.SiteGroups", "Site_SiteId", "dbo.Sites");
            DropIndex("dbo.DbUserGroups", new[] { "Group_Id" });
            DropIndex("dbo.DbUserGroups", new[] { "DbUser_UserId" });
            DropIndex("dbo.SiteGroups", new[] { "Group_Id" });
            DropIndex("dbo.SiteGroups", new[] { "Site_SiteId" });
            DropIndex("dbo.SecureSiteDatas", new[] { "SiteId" });
            DropTable("dbo.DbUserGroups");
            DropTable("dbo.SiteGroups");
            DropTable("dbo.UserRole");
            DropTable("dbo.UserAccess");
            DropTable("dbo.User");
            DropTable("dbo.SecureSiteDatas");
            DropTable("dbo.Sites");
            DropTable("dbo.Groups");
        }
    }
}
