namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserTokens : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserTokens",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        RefreshToken = c.String(),
                        UserId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id)
                .Index(t => t.Id);
            
            AddColumn("dbo.Users", "RefreshTokenId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserTokens", "Id", "dbo.Users");
            DropIndex("dbo.UserTokens", new[] { "Id" });
            DropColumn("dbo.Users", "RefreshTokenId");
            DropTable("dbo.UserTokens");
        }
    }
}
