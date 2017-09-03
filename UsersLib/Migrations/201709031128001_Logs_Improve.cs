namespace UsersLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Logs_Improve : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccessLogs", "AccessTarget", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccessLogs", "AccessTarget");
        }
    }
}
