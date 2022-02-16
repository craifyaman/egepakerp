namespace EgePakErp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Urun", "UrunKodu");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Urun", "UrunKodu", c => c.String());
        }
    }
}
