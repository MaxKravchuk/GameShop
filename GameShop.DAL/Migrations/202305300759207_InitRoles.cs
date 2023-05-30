namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitRoles : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.Roles (Name, IsDeleted) VALUES ('Administrator', 0)");
            Sql("INSERT INTO dbo.Roles (Name, IsDeleted) VALUES ('Manager', 0)");
            Sql("INSERT INTO dbo.Roles (Name, IsDeleted) VALUES ('Moderator', 0)");
            Sql("INSERT INTO dbo.Roles (Name, IsDeleted) VALUES ('User', 0)");
        }
        
        public override void Down()
        {
        }
    }
}
