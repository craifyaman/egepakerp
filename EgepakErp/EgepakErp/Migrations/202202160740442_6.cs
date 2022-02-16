namespace EgePakErp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.KalipUrunRelation",
                c => new
                    {
                        KalipUrunRelationId = c.Int(nullable: false, identity: true),
                        KalipId = c.Int(nullable: false),
                        UrunId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.KalipUrunRelationId)
                .ForeignKey("dbo.Kalip", t => t.KalipId, cascadeDelete: true)
                .ForeignKey("dbo.Urun", t => t.UrunId, cascadeDelete: true)
                .Index(t => t.KalipId)
                .Index(t => t.UrunId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KalipUrunRelation", "UrunId", "dbo.Urun");
            DropForeignKey("dbo.KalipUrunRelation", "KalipId", "dbo.Kalip");
            DropIndex("dbo.KalipUrunRelation", new[] { "UrunId" });
            DropIndex("dbo.KalipUrunRelation", new[] { "KalipId" });
            DropTable("dbo.KalipUrunRelation");
        }
    }
}
