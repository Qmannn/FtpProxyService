namespace UsersLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSiteDto : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Site", newName: "Sites");
            RenameTable(name: "dbo.DbSiteDbGroups", newName: "SiteDbGroups");
            RenameColumn(table: "dbo.SiteDbGroups", name: "DbSite_SiteId", newName: "Site_SiteId");
            RenameIndex(table: "dbo.SiteDbGroups", name: "IX_DbSite_SiteId", newName: "IX_Site_SiteId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.SiteDbGroups", name: "IX_Site_SiteId", newName: "IX_DbSite_SiteId");
            RenameColumn(table: "dbo.SiteDbGroups", name: "Site_SiteId", newName: "DbSite_SiteId");
            RenameTable(name: "dbo.SiteDbGroups", newName: "DbSiteDbGroups");
            RenameTable(name: "dbo.Sites", newName: "Site");
        }
    }
}
