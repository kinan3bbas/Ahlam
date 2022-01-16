namespace Ahlam.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class explanation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DreamExplanations",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Explanation = c.String(),
                        DreamId = c.Int(nullable: false),
                        InterpreterId = c.String(maxLength: 128),
                        Status = c.String(),
                        ExplanationDate = c.DateTime(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModificationDate = c.DateTime(nullable: false),
                        CreatorId = c.String(maxLength: 128),
                        ModifierId = c.String(maxLength: 128),
                        AttachmentId = c.Long(nullable: false),
                        CreatorName = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorId)
                .ForeignKey("dbo.Dreams", t => t.DreamId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.InterpreterId)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifierId)
                .Index(t => t.DreamId)
                .Index(t => t.InterpreterId)
                .Index(t => t.CreatorId)
                .Index(t => t.ModifierId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DreamExplanations", "ModifierId", "dbo.AspNetUsers");
            DropForeignKey("dbo.DreamExplanations", "InterpreterId", "dbo.AspNetUsers");
            DropForeignKey("dbo.DreamExplanations", "DreamId", "dbo.Dreams");
            DropForeignKey("dbo.DreamExplanations", "CreatorId", "dbo.AspNetUsers");
            DropIndex("dbo.DreamExplanations", new[] { "ModifierId" });
            DropIndex("dbo.DreamExplanations", new[] { "CreatorId" });
            DropIndex("dbo.DreamExplanations", new[] { "InterpreterId" });
            DropIndex("dbo.DreamExplanations", new[] { "DreamId" });
            DropTable("dbo.DreamExplanations");
        }
    }
}
