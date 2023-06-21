namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedUserTokens : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserTokens", "RefreshTokenExpiryTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserTokens", "RefreshTokenExpiryTime");
        }
    }
}
