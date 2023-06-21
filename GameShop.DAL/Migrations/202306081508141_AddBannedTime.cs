namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBannedTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "BannedTo", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "BannedTo");
        }
    }
}
