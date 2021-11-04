namespace Ahlam.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class payment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Amount = c.Double(nullable: false),
                        Method = c.String(),
                        Currency = c.String(),
                        Status = c.String(),
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
            
            AddColumn("dbo.Dreams", "PaymentId", c => c.Int(nullable: false));
            CreateIndex("dbo.Dreams", "PaymentId");
            AddForeignKey("dbo.Dreams", "PaymentId", "dbo.Payments", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Dreams", "PaymentId", "dbo.Payments");
            DropForeignKey("dbo.Payments", "ModifierId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Payments", "CreatorId", "dbo.AspNetUsers");
            DropIndex("dbo.Payments", new[] { "ModifierId" });
            DropIndex("dbo.Payments", new[] { "CreatorId" });
            DropIndex("dbo.Dreams", new[] { "PaymentId" });
            DropColumn("dbo.Dreams", "PaymentId");
            DropTable("dbo.Payments");
        }
    }
}
