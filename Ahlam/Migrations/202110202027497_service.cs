namespace Ahlam.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class service : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Dreams",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        Description = c.String(),
                        ServiceProviderId = c.String(maxLength: 128),
                        ServicePathId = c.Int(nullable: false),
                        Explanation = c.String(),
                        ExplanationDate = c.DateTime(),
                        CreationDate = c.DateTime(nullable: false),
                        LastModificationDate = c.DateTime(nullable: false),
                        CreatorId = c.String(maxLength: 128),
                        ModifierId = c.String(maxLength: 128),
                        AttachmentId = c.Long(nullable: false),
                        CreatorName = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorId)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifierId)
                .ForeignKey("dbo.ServicePaths", t => t.ServicePathId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ServiceProviderId)
                .Index(t => t.ServiceProviderId)
                .Index(t => t.ServicePathId)
                .Index(t => t.CreatorId)
                .Index(t => t.ModifierId);
            
            CreateTable(
                "dbo.ServicePaths",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        NumberOfInterpreters = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModificationDate = c.DateTime(nullable: false),
                        CreatorId = c.String(maxLength: 128),
                        ModifierId = c.String(maxLength: 128),
                        AttachmentId = c.Long(nullable: false),
                        CreatorName = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorId)
                .ForeignKey("dbo.AspNetUsers", t => t.ModifierId)
                .Index(t => t.CreatorId)
                .Index(t => t.ModifierId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Dreams", "ServiceProviderId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Dreams", "ServicePathId", "dbo.ServicePaths");
            DropForeignKey("dbo.ServicePaths", "ModifierId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ServicePaths", "CreatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Dreams", "ModifierId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Dreams", "CreatorId", "dbo.AspNetUsers");
            DropIndex("dbo.ServicePaths", new[] { "ModifierId" });
            DropIndex("dbo.ServicePaths", new[] { "CreatorId" });
            DropIndex("dbo.Dreams", new[] { "ModifierId" });
            DropIndex("dbo.Dreams", new[] { "CreatorId" });
            DropIndex("dbo.Dreams", new[] { "ServicePathId" });
            DropIndex("dbo.Dreams", new[] { "ServiceProviderId" });
            DropTable("dbo.ServicePaths");
            DropTable("dbo.Dreams");
        }
    }
}
