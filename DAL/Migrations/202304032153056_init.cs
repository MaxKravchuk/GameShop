namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Coments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Body = c.String(nullable: false, maxLength: 255),
                        GameKey = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.GameKey)
                .Index(t => t.GameKey);
            
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Key = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 255),
                        Id = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.GameGenre",
                c => new
                    {
                        GameKey = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => new { t.GameKey, t.Name })
                .ForeignKey("dbo.Games", t => t.GameKey, cascadeDelete: true)
                .ForeignKey("dbo.Genres", t => t.Name, cascadeDelete: true)
                .Index(t => t.GameKey)
                .Index(t => t.Name);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 50),
                        ParentGenreName = c.String(maxLength: 50),
                        Id = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Name)
                .ForeignKey("dbo.Genres", t => t.ParentGenreName)
                .Index(t => t.ParentGenreName);
            
            CreateTable(
                "dbo.GamePlatformType",
                c => new
                    {
                        GameKey = c.String(nullable: false, maxLength: 128),
                        Type = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => new { t.GameKey, t.Type })
                .ForeignKey("dbo.Games", t => t.GameKey, cascadeDelete: true)
                .ForeignKey("dbo.PlatformTypes", t => t.Type, cascadeDelete: true)
                .Index(t => t.GameKey)
                .Index(t => t.Type);
            
            CreateTable(
                "dbo.PlatformTypes",
                c => new
                    {
                        Type = c.String(nullable: false, maxLength: 50),
                        Id = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Type);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Coments", "GameKey", "dbo.Games");
            DropForeignKey("dbo.GamePlatformType", "Type", "dbo.PlatformTypes");
            DropForeignKey("dbo.GamePlatformType", "GameKey", "dbo.Games");
            DropForeignKey("dbo.GameGenre", "Name", "dbo.Genres");
            DropForeignKey("dbo.Genres", "ParentGenreName", "dbo.Genres");
            DropForeignKey("dbo.GameGenre", "GameKey", "dbo.Games");
            DropIndex("dbo.GamePlatformType", new[] { "Type" });
            DropIndex("dbo.GamePlatformType", new[] { "GameKey" });
            DropIndex("dbo.Genres", new[] { "ParentGenreName" });
            DropIndex("dbo.GameGenre", new[] { "Name" });
            DropIndex("dbo.GameGenre", new[] { "GameKey" });
            DropIndex("dbo.Coments", new[] { "GameKey" });
            DropTable("dbo.PlatformTypes");
            DropTable("dbo.GamePlatformType");
            DropTable("dbo.Genres");
            DropTable("dbo.GameGenre");
            DropTable("dbo.Games");
            DropTable("dbo.Coments");
        }
    }
}
