namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoredProcedureForComments : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                CREATE PROCEDURE GetById
                    @CommentId INT
                AS
                BEGIN
                    SET NOCOUNT ON;
        
                    SELECT *
                    FROM Comments
                    WHERE Id = @CommentId;
                END
            ");
        }
        
        public override void Down()
        {
            Sql("DROP PROCEDURE IF EXISTS GetById");
        }
    }
}
