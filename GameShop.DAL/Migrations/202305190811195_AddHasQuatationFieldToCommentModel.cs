namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddHasQuatationFieldToCommentModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "HasQuatation", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "HasQuatation");
        }
    }
}
