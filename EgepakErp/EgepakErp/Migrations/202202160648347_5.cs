namespace EgePakErp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HammaddeCinsi",
                c => new
                    {
                        HammaddeCinsiId = c.Int(nullable: false, identity: true),
                        Kisaltmasi = c.String(),
                        Adi = c.String(),
                        Aciklamasi = c.String(),
                    })
                .PrimaryKey(t => t.HammaddeCinsiId);
            
            CreateTable(
                "dbo.Kalip",
                c => new
                    {
                        KalipId = c.Int(nullable: false, identity: true),
                        KalipNo = c.String(),
                        KalipOzellik = c.String(),
                        Adi = c.String(),
                        UretimTeminSekliId = c.Int(nullable: false),
                        ParcaAgirlik = c.Decimal(nullable: false, precision: 18, scale: 2),
                        KalipGozSayisi = c.Int(nullable: false),
                        UretimZamani = c.Int(nullable: false),
                        Aciklama = c.String(),
                        HammaddeCinsi_HammaddeCinsiId = c.Int(),
                    })
                .PrimaryKey(t => t.KalipId)
                .ForeignKey("dbo.UretimTeminSekli", t => t.UretimTeminSekliId, cascadeDelete: true)
                .ForeignKey("dbo.HammaddeCinsi", t => t.HammaddeCinsi_HammaddeCinsiId)
                .Index(t => t.UretimTeminSekliId)
                .Index(t => t.HammaddeCinsi_HammaddeCinsiId);
            
            CreateTable(
                "dbo.KalipHammaddeRelation",
                c => new
                    {
                        KalipHammaddeRelationId = c.Int(nullable: false, identity: true),
                        KalipId = c.Int(nullable: false),
                        HammaddeCinsiId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.KalipHammaddeRelationId)
                .ForeignKey("dbo.HammaddeCinsi", t => t.HammaddeCinsiId, cascadeDelete: true)
                .ForeignKey("dbo.Kalip", t => t.KalipId, cascadeDelete: true)
                .Index(t => t.KalipId)
                .Index(t => t.HammaddeCinsiId);
            
            CreateTable(
                "dbo.UretimTeminSekli",
                c => new
                    {
                        UretimTeminSekliId = c.Int(nullable: false, identity: true),
                        Kisaltmasi = c.String(),
                        Adi = c.String(),
                        Aciklamasi = c.String(),
                    })
                .PrimaryKey(t => t.UretimTeminSekliId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Kalip", "HammaddeCinsi_HammaddeCinsiId", "dbo.HammaddeCinsi");
            DropForeignKey("dbo.Kalip", "UretimTeminSekliId", "dbo.UretimTeminSekli");
            DropForeignKey("dbo.KalipHammaddeRelation", "KalipId", "dbo.Kalip");
            DropForeignKey("dbo.KalipHammaddeRelation", "HammaddeCinsiId", "dbo.HammaddeCinsi");
            DropIndex("dbo.KalipHammaddeRelation", new[] { "HammaddeCinsiId" });
            DropIndex("dbo.KalipHammaddeRelation", new[] { "KalipId" });
            DropIndex("dbo.Kalip", new[] { "HammaddeCinsi_HammaddeCinsiId" });
            DropIndex("dbo.Kalip", new[] { "UretimTeminSekliId" });
            DropTable("dbo.UretimTeminSekli");
            DropTable("dbo.KalipHammaddeRelation");
            DropTable("dbo.Kalip");
            DropTable("dbo.HammaddeCinsi");
        }
    }
}
