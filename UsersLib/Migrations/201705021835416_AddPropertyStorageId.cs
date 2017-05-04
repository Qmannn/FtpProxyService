namespace UsersLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPropertyStorageId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Site", "StorageId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Site", "StorageId");
        }
    }
}
