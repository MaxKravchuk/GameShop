namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddViewsFieldForGame : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "Views", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "Views");
        }
    }
}
