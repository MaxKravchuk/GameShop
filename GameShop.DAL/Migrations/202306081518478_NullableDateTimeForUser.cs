﻿namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableDateTimeForUser : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "BannedTo", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "BannedTo", c => c.DateTime(nullable: false));
        }
    }
}
