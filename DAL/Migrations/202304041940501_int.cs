namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _int : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Games", new[] { "Key" });
            DropIndex("dbo.Genres", new[] { "Name" });
            DropIndex("dbo.PlatformTypes", new[] { "Type" });
            AlterColumn("dbo.Games", "Key", c => c.String(maxLength: 50));
            AlterColumn("dbo.Genres", "Name", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.PlatformTypes", "Type", c => c.String(nullable: false, maxLength: 50, unicode: false));
            CreateIndex("dbo.Genres", "Name", unique: true);
            CreateIndex("dbo.PlatformTypes", "Type", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.PlatformTypes", new[] { "Type" });
            DropIndex("dbo.Genres", new[] { "Name" });
            AlterColumn("dbo.PlatformTypes", "Type", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Genres", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Games", "Key", c => c.String());
            CreateIndex("dbo.PlatformTypes", "Type", unique: true);
            CreateIndex("dbo.Genres", "Name", unique: true);
            CreateIndex("dbo.Games", "Key", unique: true);
        }
    }
}
