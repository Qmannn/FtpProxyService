namespace UsersLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtendSite : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Site", "Name", c => c.String());
            AddColumn("dbo.Site", "Address", c => c.String());
            AddColumn("dbo.Site", "Port", c => c.Int(nullable: false));
            AddColumn("dbo.Site", "Login", c => c.String());
            AddColumn("dbo.Site", "Password", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Site", "Password");
            DropColumn("dbo.Site", "Login");
            DropColumn("dbo.Site", "Port");
            DropColumn("dbo.Site", "Address");
            DropColumn("dbo.Site", "Name");
        }
    }
}
