namespace NguyenThiPhuongThao_51.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_Table_NhanVien : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PhongBans",
                c => new
                    {
                        MaPB = c.Int(nullable: false, identity: true),
                        TenPB = c.String(),
                    })
                .PrimaryKey(t => t.MaPB);
            
            CreateTable(
                "dbo.NhanViens",
                c => new
                    {
                        MaPB = c.Int(nullable: false),
                        MaNV = c.String(),
                        HoTen = c.String(),
                    })
                .PrimaryKey(t => t.MaPB)
                .ForeignKey("dbo.PhongBans", t => t.MaPB)
                .Index(t => t.MaPB);
            
            DropTable("dbo.PhongBans");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PhongBans",
                c => new
                    {
                        MaPB = c.Int(nullable: false, identity: true),
                        TenPB = c.String(),
                    })
                .PrimaryKey(t => t.MaPB);
            
            DropForeignKey("dbo.NhanViens", "MaPB", "dbo.PhongBans");
            DropIndex("dbo.NhanViens", new[] { "MaPB" });
            DropTable("dbo.NhanViens");
            DropTable("dbo.PhongBans");
        }
    }
}
