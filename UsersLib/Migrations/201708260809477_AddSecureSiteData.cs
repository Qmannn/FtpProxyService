namespace UsersLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSecureSiteData : DbMigration
    {
        public override void Up()
        {
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
                .ForeignKey("dbo.Site", t => t.SiteId)
                .Index(t => t.SiteId);
            
            DropColumn("dbo.Site", "Address");
            DropColumn("dbo.Site", "Port");
            DropColumn("dbo.Site", "Login");
            DropColumn("dbo.Site", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Site", "Password", c => c.String());
            AddColumn("dbo.Site", "Login", c => c.String());
            AddColumn("dbo.Site", "Port", c => c.Int(nullable: false));
            AddColumn("dbo.Site", "Address", c => c.String());
            DropForeignKey("dbo.SecureSiteDatas", "SiteId", "dbo.Site");
            DropIndex("dbo.SecureSiteDatas", new[] { "SiteId" });
            DropTable("dbo.SecureSiteDatas");
        }
    }
}
