﻿namespace DAL.Migrations
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
                        isDeleted = c.Boolean(nullable: false),
                        GameKey = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.GameKey)
                .Index(t => t.GameKey);
            
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Key = c.String(nullable: false, maxLength: 128),
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 255),
                        isDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 50),
                        Id = c.Int(nullable: false),
                        isDeleted = c.Boolean(nullable: false),
                        ParentGenreName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Name)
                .ForeignKey("dbo.Genres", t => t.ParentGenreName)
                .Index(t => t.ParentGenreName);
            
            CreateTable(
                "dbo.PlatformTypes",
                c => new
                    {
                        Type = c.String(nullable: false, maxLength: 50),
                        Id = c.Int(nullable: false),
                        isDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Type);
            
            CreateTable(
                "dbo.GameGenre",
                c => new
                    {
                        GameRefKey = c.String(nullable: false, maxLength: 128),
                        GenreRefName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => new { t.GameRefKey, t.GenreRefName })
                .ForeignKey("dbo.Games", t => t.GameRefKey, cascadeDelete: true)
                .ForeignKey("dbo.Genres", t => t.GenreRefName, cascadeDelete: true)
                .Index(t => t.GameRefKey)
                .Index(t => t.GenreRefName);
            
            CreateTable(
                "dbo.GamePlatformType",
                c => new
                    {
                        GameRefKey = c.String(nullable: false, maxLength: 128),
                        PlatformTypeRefType = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => new { t.GameRefKey, t.PlatformTypeRefType })
                .ForeignKey("dbo.Games", t => t.GameRefKey, cascadeDelete: true)
                .ForeignKey("dbo.PlatformTypes", t => t.PlatformTypeRefType, cascadeDelete: true)
                .Index(t => t.GameRefKey)
                .Index(t => t.PlatformTypeRefType);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Coments", "GameKey", "dbo.Games");
            DropForeignKey("dbo.GamePlatformType", "PlatformTypeRefType", "dbo.PlatformTypes");
            DropForeignKey("dbo.GamePlatformType", "GameRefKey", "dbo.Games");
            DropForeignKey("dbo.GameGenre", "GenreRefName", "dbo.Genres");
            DropForeignKey("dbo.GameGenre", "GameRefKey", "dbo.Games");
            DropForeignKey("dbo.Genres", "ParentGenreName", "dbo.Genres");
            DropIndex("dbo.GamePlatformType", new[] { "PlatformTypeRefType" });
            DropIndex("dbo.GamePlatformType", new[] { "GameRefKey" });
            DropIndex("dbo.GameGenre", new[] { "GenreRefName" });
            DropIndex("dbo.GameGenre", new[] { "GameRefKey" });
            DropIndex("dbo.Genres", new[] { "ParentGenreName" });
            DropIndex("dbo.Coments", new[] { "GameKey" });
            DropTable("dbo.GamePlatformType");
            DropTable("dbo.GameGenre");
            DropTable("dbo.PlatformTypes");
            DropTable("dbo.Genres");
            DropTable("dbo.Games");
            DropTable("dbo.Coments");
        }
    }
}
