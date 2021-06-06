namespace NguyenThiPhuongThao_51.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_Table_PhongBan : DbMigration
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PhongBans");
        }
    }
}
