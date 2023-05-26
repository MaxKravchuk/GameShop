namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddIsPaidToOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "IsPaid", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "IsPaid");
        }
    }
}
