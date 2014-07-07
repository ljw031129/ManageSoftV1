namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEquipmentSmart01 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Smarts", "EquipmentId", "dbo.Equipments");
            DropIndex("dbo.Smarts", new[] { "EquipmentId" });
            DropTable("dbo.Equipments");
            DropTable("dbo.Smarts");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.SmartId);
            
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
            
            CreateIndex("dbo.Smarts", "EquipmentId");
            AddForeignKey("dbo.Smarts", "EquipmentId", "dbo.Equipments", "EquipmentId");
        }
    }
}
