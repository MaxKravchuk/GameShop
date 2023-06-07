namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserPublisherRelationship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "PublisherId", c => c.Int());
            AddColumn("dbo.Users", "Publisher_Id", c => c.Int());
            CreateIndex("dbo.Users", "Publisher_Id");
            AddForeignKey("dbo.Users", "Publisher_Id", "dbo.Publishers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "Publisher_Id", "dbo.Publishers");
            DropIndex("dbo.Users", new[] { "Publisher_Id" });
            DropColumn("dbo.Users", "Publisher_Id");
            DropColumn("dbo.Users", "PublisherId");
        }
    }
}
