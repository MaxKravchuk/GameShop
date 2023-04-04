namespace DAL.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class addIndexToGame : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Games", "Key", c => c.String(nullable: false, maxLength: 50, unicode: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "IX_Key",
                        new AnnotationValues(oldValue: null, newValue: "IndexAnnotation: { Name: IX_Key, IsUnique: True }")
                    },
                }));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Games", "Key", c => c.String(maxLength: 50,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "IX_Key",
                        new AnnotationValues(oldValue: "IndexAnnotation: { Name: IX_Key, IsUnique: True }", newValue: null)
                    },
                }));
        }
    }
}
