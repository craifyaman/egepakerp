namespace EgePakErp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HammaddeCinsiUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HammaddeCinsi", "BirimId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HammaddeCinsi", "BirimId");
        }
    }
}
