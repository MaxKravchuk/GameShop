namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddCreatedAtFieldForGame : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "CreatedAt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "CreatedAt");
        }
    }
}
