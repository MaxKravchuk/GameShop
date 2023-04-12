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
                        GameId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.GameId)
                .Index(t => t.GameId);
            
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(nullable: false, maxLength: 255, unicode: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 255),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Key, unique: true, name: "Index");
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        ParentGenreId = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Genres", t => t.ParentGenreId)
                .Index(t => t.Name, unique: true)
                .Index(t => t.ParentGenreId);
            
            CreateTable(
                "dbo.PlatformTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false, maxLength: 50, unicode: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Type, unique: true);
            
            CreateTable(
                "dbo.GameGenres",
                c => new
                    {
                        Game_Id = c.Int(nullable: false),
                        Genre_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Game_Id, t.Genre_Id })
                .ForeignKey("dbo.Games", t => t.Game_Id, cascadeDelete: true)
                .ForeignKey("dbo.Genres", t => t.Genre_Id, cascadeDelete: true)
                .Index(t => t.Game_Id)
                .Index(t => t.Genre_Id);
            
            CreateTable(
                "dbo.GamePlatformTypes",
                c => new
                    {
                        Game_Id = c.Int(nullable: false),
                        PlatformType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Game_Id, t.PlatformType_Id })
                .ForeignKey("dbo.Games", t => t.Game_Id, cascadeDelete: true)
                .ForeignKey("dbo.PlatformTypes", t => t.PlatformType_Id, cascadeDelete: true)
                .Index(t => t.Game_Id)
                .Index(t => t.PlatformType_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Coments", "GameId", "dbo.Games");
            DropForeignKey("dbo.GamePlatformTypes", "PlatformType_Id", "dbo.PlatformTypes");
            DropForeignKey("dbo.GamePlatformTypes", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.GameGenres", "Genre_Id", "dbo.Genres");
            DropForeignKey("dbo.GameGenres", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Genres", "ParentGenreId", "dbo.Genres");
            DropIndex("dbo.GamePlatformTypes", new[] { "PlatformType_Id" });
            DropIndex("dbo.GamePlatformTypes", new[] { "Game_Id" });
            DropIndex("dbo.GameGenres", new[] { "Genre_Id" });
            DropIndex("dbo.GameGenres", new[] { "Game_Id" });
            DropIndex("dbo.PlatformTypes", new[] { "Type" });
            DropIndex("dbo.Genres", new[] { "ParentGenreId" });
            DropIndex("dbo.Genres", new[] { "Name" });
            DropIndex("dbo.Games", "Index");
            DropIndex("dbo.Coments", new[] { "GameId" });
            DropTable("dbo.GamePlatformTypes");
            DropTable("dbo.GameGenres");
            DropTable("dbo.PlatformTypes");
            DropTable("dbo.Genres");
            DropTable("dbo.Games");
            DropTable("dbo.Coments");
        }
    }
}
