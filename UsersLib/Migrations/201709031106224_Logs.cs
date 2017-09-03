namespace UsersLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Logs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccessLogs",
                c => new
                    {
                        AccesslogId = c.Long(nullable: false, identity: true),
                        Login = c.String(),
                        AccessTime = c.DateTime(nullable: false),
                        Role = c.String(),
                        IsAutenticated = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AccesslogId);
            
            DropTable("dbo.UserAccess");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserAccess",
                c => new
                    {
                        Login = c.String(nullable: false, maxLength: 128),
                        AccessTime = c.DateTime(nullable: false),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Login);
            
            DropTable("dbo.AccessLogs");
        }
    }
}
