namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoredProceduresForGenres : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                CREATE PROCEDURE GetGenreById
                    @GenreId INT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT g.*
                    FROM Genres g
                    WHERE g.Id = @GenreId;
                END
            ");

            Sql(@"
                CREATE PROCEDURE GetGenres
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT g.*
                    FROM Genres g
                END
            ");
        }
        
        public override void Down()
        {
            Sql("DROP PROCEDURE IF EXISTS GetGenreById");
            Sql("DROP PROCEDURE IF EXISTS GetGenres");
        }
    }
}
