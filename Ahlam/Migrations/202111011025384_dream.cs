namespace Ahlam.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dream : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dreams", "PhoneNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Dreams", "PhoneNumber");
        }
    }
}
