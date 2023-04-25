namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPublisherTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Publishers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false, maxLength: 40),
                        Description = c.String(nullable: false),
                        HomePage = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Games", "Price", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.Games", "UnitsInStock", c => c.Short(nullable: false));
            AddColumn("dbo.Games", "Discontinued", c => c.Boolean(nullable: false));
            AddColumn("dbo.Games", "PublisherId", c => c.Int(nullable: false));
            CreateIndex("dbo.Games", "PublisherId");
            AddForeignKey("dbo.Games", "PublisherId", "dbo.Publishers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Games", "PublisherId", "dbo.Publishers");
            DropIndex("dbo.Games", new[] { "PublisherId" });
            DropColumn("dbo.Games", "PublisherId");
            DropColumn("dbo.Games", "Discontinued");
            DropColumn("dbo.Games", "UnitsInStock");
            DropColumn("dbo.Games", "Price");
            DropTable("dbo.Publishers");
        }
    }
}
