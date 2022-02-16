namespace EgePakErp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AuditLog", "Referance", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AuditLog", "Referance");
        }
    }
}
