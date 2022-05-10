namespace EgePakErp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HammaddeCinsi", "BirimId", "dbo.Birim");
            DropForeignKey("dbo.HammaddeHaraket", "MarkaId", "dbo.Marka");
            DropIndex("dbo.HammaddeHaraket", new[] { "MarkaId" });
            DropIndex("dbo.HammaddeCinsi", new[] { "BirimId" });
            RenameColumn(table: "dbo.HammaddeHaraket", name: "Tedarikci_CariId", newName: "TedarikciId_CariId");
            RenameIndex(table: "dbo.HammaddeHaraket", name: "IX_Tedarikci_CariId", newName: "IX_TedarikciId_CariId");
            CreateTable(
                "dbo.HammaddeBirimi",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Birimi = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.HammaddeHaraket", "KdvTutarı", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.HammaddeCinsi", "HammaddeBirimi_Id", c => c.Int());
            AlterColumn("dbo.HammaddeHaraket", "MarkaId", c => c.Int(nullable: false));
            AlterColumn("dbo.HammaddeCinsi", "BirimId", c => c.Int(nullable: false));
            CreateIndex("dbo.HammaddeHaraket", "MarkaId");
            CreateIndex("dbo.HammaddeCinsi", "HammaddeBirimi_Id");
            AddForeignKey("dbo.HammaddeCinsi", "HammaddeBirimi_Id", "dbo.HammaddeBirimi", "Id");
            AddForeignKey("dbo.HammaddeHaraket", "MarkaId", "dbo.Marka", "Id", cascadeDelete: true);
            DropColumn("dbo.HammaddeHaraket", "BirimFiyat");
            DropColumn("dbo.HammaddeHaraket", "TedarikciId");
            DropColumn("dbo.HammaddeHaraket", "KdvTutari");
            DropColumn("dbo.HammaddeHaraket", "BelgeNo");
            DropColumn("dbo.HammaddeHaraket", "SiraNo");
            DropColumn("dbo.HammaddeHaraket", "FaturaTarihi");
            DropColumn("dbo.HammaddeHaraket", "Aciklama");
            DropTable("dbo.Birim");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Birim",
                c => new
                    {
                        BirimId = c.Int(nullable: false, identity: true),
                        Adi = c.String(),
                    })
                .PrimaryKey(t => t.BirimId);
            
            AddColumn("dbo.HammaddeHaraket", "Aciklama", c => c.String());
            AddColumn("dbo.HammaddeHaraket", "FaturaTarihi", c => c.DateTime(nullable: false));
            AddColumn("dbo.HammaddeHaraket", "SiraNo", c => c.String());
            AddColumn("dbo.HammaddeHaraket", "BelgeNo", c => c.String());
            AddColumn("dbo.HammaddeHaraket", "KdvTutari", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.HammaddeHaraket", "TedarikciId", c => c.Int());
            AddColumn("dbo.HammaddeHaraket", "BirimFiyat", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropForeignKey("dbo.HammaddeHaraket", "MarkaId", "dbo.Marka");
            DropForeignKey("dbo.HammaddeCinsi", "HammaddeBirimi_Id", "dbo.HammaddeBirimi");
            DropIndex("dbo.HammaddeCinsi", new[] { "HammaddeBirimi_Id" });
            DropIndex("dbo.HammaddeHaraket", new[] { "MarkaId" });
            AlterColumn("dbo.HammaddeCinsi", "BirimId", c => c.Int());
            AlterColumn("dbo.HammaddeHaraket", "MarkaId", c => c.Int());
            DropColumn("dbo.HammaddeCinsi", "HammaddeBirimi_Id");
            DropColumn("dbo.HammaddeHaraket", "KdvTutarı");
            DropTable("dbo.HammaddeBirimi");
            RenameIndex(table: "dbo.HammaddeHaraket", name: "IX_TedarikciId_CariId", newName: "IX_Tedarikci_CariId");
            RenameColumn(table: "dbo.HammaddeHaraket", name: "TedarikciId_CariId", newName: "Tedarikci_CariId");
            CreateIndex("dbo.HammaddeCinsi", "BirimId");
            CreateIndex("dbo.HammaddeHaraket", "MarkaId");
            AddForeignKey("dbo.HammaddeHaraket", "MarkaId", "dbo.Marka", "Id");
            AddForeignKey("dbo.HammaddeCinsi", "BirimId", "dbo.Birim", "BirimId");
        }
    }
}
