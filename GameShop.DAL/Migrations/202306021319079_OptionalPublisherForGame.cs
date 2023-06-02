namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OptionalPublisherForGame : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Games", "PublisherId", "dbo.Publishers");
            DropIndex("dbo.Games", new[] { "PublisherId" });
            AlterColumn("dbo.Games", "PublisherId", c => c.Int());
            CreateIndex("dbo.Games", "PublisherId");
            AddForeignKey("dbo.Games", "PublisherId", "dbo.Publishers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Games", "PublisherId", "dbo.Publishers");
            DropIndex("dbo.Games", new[] { "PublisherId" });
            AlterColumn("dbo.Games", "PublisherId", c => c.Int(nullable: false));
            CreateIndex("dbo.Games", "PublisherId");
            AddForeignKey("dbo.Games", "PublisherId", "dbo.Publishers", "Id", cascadeDelete: true);
        }
    }
}
