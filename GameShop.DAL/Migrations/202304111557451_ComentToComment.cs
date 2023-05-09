namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ComentToComment : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Coments", newName: "Comments");
        }

        public override void Down()
        {
            RenameTable(name: "dbo.Comments", newName: "Coments");
        }
    }
}
