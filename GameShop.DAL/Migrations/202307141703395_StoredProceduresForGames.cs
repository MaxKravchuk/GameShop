namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoredProceduresForGames : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                CREATE PROCEDURE GetGameByKey
                    @GameKey NVARCHAR(255)
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT g.*
                    FROM Games g
                    WHERE g.[Key] = @GameKey;
                END
            ");

            Sql(@"
                CREATE PROCEDURE GetGamesByGenreId
                    @GenreId INT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT g.*
                    FROM Games g
                    LEFT JOIN GameGenres gg ON g.Id = gg.Game_Id
                    LEFT JOIN Genres gn ON gg.Genre_Id = gn.Id
                    WHERE gn.Id = @GenreId;
                END
            ");

            Sql(@"
                CREATE PROCEDURE GetGamesByPlatformTypeId
                    @PlatformTypeId INT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT g.*
                    FROM Games g
                    LEFT JOIN GamePlatformTypes gpt ON g.Id = gpt.Game_Id
                    LEFT JOIN PlatformTypes pt ON gpt.PlatformType_Id = pt.Id
                    WHERE pt.Id = @PlatformTypeId;
                END
            ");
        }
        
        public override void Down()
        {
            Sql("DROP PROCEDURE IF EXISTS GetGameByKey");
            Sql("DROP PROCEDURE IF EXISTS GetGamesByGenreId");
            Sql("DROP PROCEDURE IF EXISTS GetGamesByPlatformTypeId");
        }
    }
}
