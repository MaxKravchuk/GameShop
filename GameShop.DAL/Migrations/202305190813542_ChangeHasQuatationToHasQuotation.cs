namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ChangeHasQuatationToHasQuotation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "HasQuotation", c => c.Boolean(nullable: false));
            DropColumn("dbo.Comments", "HasQuatation");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "HasQuatation", c => c.Boolean(nullable: false));
            DropColumn("dbo.Comments", "HasQuotation");
        }
    }
}
