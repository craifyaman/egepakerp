namespace EgePakErp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sepet",
                c => new
                    {
                        SepetId = c.Int(nullable: false, identity: true),
                        CariId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SepetId)
                .ForeignKey("dbo.Cari", t => t.CariId, cascadeDelete: true)
                .Index(t => t.CariId);
            
            CreateTable(
                "dbo.SepetIcerik",
                c => new
                    {
                        SepetIcerikId = c.Int(nullable: false, identity: true),
                        SepetId = c.Int(nullable: false),
                        UrunId = c.Int(),
                        KalipId = c.Int(),
                    })
                .PrimaryKey(t => t.SepetIcerikId)
                .ForeignKey("dbo.Kalip", t => t.KalipId)
                .ForeignKey("dbo.Sepet", t => t.SepetId, cascadeDelete: true)
                .ForeignKey("dbo.Urun", t => t.UrunId)
                .Index(t => t.SepetId)
                .Index(t => t.UrunId)
                .Index(t => t.KalipId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SepetIcerik", "UrunId", "dbo.Urun");
            DropForeignKey("dbo.SepetIcerik", "SepetId", "dbo.Sepet");
            DropForeignKey("dbo.SepetIcerik", "KalipId", "dbo.Kalip");
            DropForeignKey("dbo.Sepet", "CariId", "dbo.Cari");
            DropIndex("dbo.SepetIcerik", new[] { "KalipId" });
            DropIndex("dbo.SepetIcerik", new[] { "UrunId" });
            DropIndex("dbo.SepetIcerik", new[] { "SepetId" });
            DropIndex("dbo.Sepet", new[] { "CariId" });
            DropTable("dbo.SepetIcerik");
            DropTable("dbo.Sepet");
        }
    }
}
