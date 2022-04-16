namespace EgePakErp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HammaddeCinsiVeHammaddeHareket1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HammaddeCinsi", "HammaddeBirimi_Id", c => c.Int());
            CreateIndex("dbo.HammaddeCinsi", "HammaddeBirimi_Id");
            AddForeignKey("dbo.HammaddeCinsi", "HammaddeBirimi_Id", "dbo.HammaddeBirimi", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HammaddeCinsi", "HammaddeBirimi_Id", "dbo.HammaddeBirimi");
            DropIndex("dbo.HammaddeCinsi", new[] { "HammaddeBirimi_Id" });
            DropColumn("dbo.HammaddeCinsi", "HammaddeBirimi_Id");
        }
    }
}
