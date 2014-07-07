namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEquipmentSmart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Equipments",
                c => new
                    {
                        EquipmentId = c.String(nullable: false, maxLength: 32),
                        EquipmentNum = c.String(maxLength: 8),
                        EquipmentName = c.String(maxLength: 50),
                        EquipmentCreatTime = c.DateTime(nullable: false),
                        EquipmentUpDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.EquipmentId);
            
            CreateTable(
                "dbo.Smarts",
                c => new
                    {
                        SmartId = c.String(nullable: false, maxLength: 32),
                        SmartNum = c.String(maxLength: 8),
                        SmartSim = c.String(),
                        SmartCreatTime = c.DateTime(nullable: false),
                        SmartUpdateTime = c.DateTime(nullable: false),
                        EquipmentId = c.String(maxLength: 32),
                    })
                .PrimaryKey(t => t.SmartId)
                .ForeignKey("dbo.Equipments", t => t.EquipmentId)
                .Index(t => t.EquipmentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Smarts", "EquipmentId", "dbo.Equipments");
            DropIndex("dbo.Smarts", new[] { "EquipmentId" });
            DropTable("dbo.Smarts");
            DropTable("dbo.Equipments");
        }
    }
}
