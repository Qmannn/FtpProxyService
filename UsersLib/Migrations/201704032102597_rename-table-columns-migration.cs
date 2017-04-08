namespace UsersLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renametablecolumnsmigration : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DbSiteGroups", newName: "SiteGroup");
            RenameTable(name: "dbo.DbSites", newName: "Site");
            RenameTable(name: "dbo.DbUserGroups", newName: "UserGroup");
            RenameTable(name: "dbo.DbUsers", newName: "User");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.User", newName: "DbUsers");
            RenameTable(name: "dbo.UserGroup", newName: "DbUserGroups");
            RenameTable(name: "dbo.Site", newName: "DbSites");
            RenameTable(name: "dbo.SiteGroup", newName: "DbSiteGroups");
        }
    }
}
