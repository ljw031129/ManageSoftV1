namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class commondCurrent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TerminalEquipmentCommandCurrents",
                c => new
                    {
                        TerminalEquipmentCommandCurrentId = c.String(nullable: false, maxLength: 128),
                        IMEI = c.String(maxLength: 50),
                        OperateTime = c.Long(nullable: false),
                        Dtype = c.String(),
                        CommandJsonData = c.String(),
                        CommandFromTo = c.String(),
                        OperateDataHex = c.String(),
                        OperateStatue = c.String(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.TerminalEquipmentCommandCurrentId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TerminalEquipmentCommandCurrents", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.TerminalEquipmentCommandCurrents", new[] { "UserId" });
            DropTable("dbo.TerminalEquipmentCommandCurrents");
        }
    }
}
