namespace UsersLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitDataBase : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserGroup", newName: "Group");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Group", newName: "UserGroup");
        }
    }
}
