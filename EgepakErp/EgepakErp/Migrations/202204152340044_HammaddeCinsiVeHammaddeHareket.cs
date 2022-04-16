namespace EgePakErp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HammaddeCinsiVeHammaddeHareket : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HammaddeBirimi",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Birimi = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HammaddeHaraket",
                c => new
                    {
                        HammaddeHaraketId = c.Int(nullable: false, identity: true),
                        KayitTarihi = c.DateTime(nullable: false),
                        HammaddeGirisTarihi = c.DateTime(nullable: false),
                        HammaddeCinsiId = c.Int(nullable: false),
                        Miktar = c.Decimal(nullable: false, precision: 18, scale: 2),
                        KdvTutarı = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ToplamTutar = c.Decimal(nullable: false, precision: 18, scale: 2),
                        KdvOranı = c.Int(nullable: false),
                        FaturaNo = c.String(),
                        MarkaId = c.Int(nullable: false),
                        DovizId = c.Int(nullable: false),
                        DolarKuru = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EuroKuru = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PoundKuru = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HammaddetipiId = c.Int(nullable: false),
                        TedarikciId_CariId = c.Int(),
                    })
                .PrimaryKey(t => t.HammaddeHaraketId)
                .ForeignKey("dbo.Doviz", t => t.DovizId, cascadeDelete: true)
                .ForeignKey("dbo.HammaddeCinsi", t => t.HammaddeCinsiId, cascadeDelete: true)
                .ForeignKey("dbo.HammaddeTipi", t => t.HammaddetipiId, cascadeDelete: true)
                .ForeignKey("dbo.Marka", t => t.MarkaId, cascadeDelete: true)
                .ForeignKey("dbo.Cari", t => t.TedarikciId_CariId)
                .Index(t => t.HammaddeCinsiId)
                .Index(t => t.MarkaId)
                .Index(t => t.DovizId)
                .Index(t => t.HammaddetipiId)
                .Index(t => t.TedarikciId_CariId);
            
            CreateTable(
                "dbo.HammaddeTipi",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tipi = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Marka",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Adi = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HammaddeHaraket", "TedarikciId_CariId", "dbo.Cari");
            DropForeignKey("dbo.HammaddeHaraket", "MarkaId", "dbo.Marka");
            DropForeignKey("dbo.HammaddeHaraket", "HammaddetipiId", "dbo.HammaddeTipi");
            DropForeignKey("dbo.HammaddeHaraket", "HammaddeCinsiId", "dbo.HammaddeCinsi");
            DropForeignKey("dbo.HammaddeHaraket", "DovizId", "dbo.Doviz");
            DropIndex("dbo.HammaddeHaraket", new[] { "TedarikciId_CariId" });
            DropIndex("dbo.HammaddeHaraket", new[] { "HammaddetipiId" });
            DropIndex("dbo.HammaddeHaraket", new[] { "DovizId" });
            DropIndex("dbo.HammaddeHaraket", new[] { "MarkaId" });
            DropIndex("dbo.HammaddeHaraket", new[] { "HammaddeCinsiId" });
            DropTable("dbo.Marka");
            DropTable("dbo.HammaddeTipi");
            DropTable("dbo.HammaddeHaraket");
            DropTable("dbo.HammaddeBirimi");
        }
    }
}
