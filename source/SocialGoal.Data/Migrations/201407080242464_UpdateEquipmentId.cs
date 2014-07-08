namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateEquipmentId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Smarts", "EquipmentId", "dbo.Equipments");
            DropIndex("dbo.Smarts", new[] { "EquipmentId" });
            DropPrimaryKey("dbo.Equipments");
            AlterColumn("dbo.Equipments", "EquipmentId", c => c.String(nullable: false, maxLength: 36));
            AlterColumn("dbo.Smarts", "EquipmentId", c => c.String(maxLength: 36));
            AddPrimaryKey("dbo.Equipments", "EquipmentId");
            CreateIndex("dbo.Smarts", "EquipmentId");
            AddForeignKey("dbo.Smarts", "EquipmentId", "dbo.Equipments", "EquipmentId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Smarts", "EquipmentId", "dbo.Equipments");
            DropIndex("dbo.Smarts", new[] { "EquipmentId" });
            DropPrimaryKey("dbo.Equipments");
            AlterColumn("dbo.Smarts", "EquipmentId", c => c.String(maxLength: 32));
            AlterColumn("dbo.Equipments", "EquipmentId", c => c.String(nullable: false, maxLength: 32));
            AddPrimaryKey("dbo.Equipments", "EquipmentId");
            CreateIndex("dbo.Smarts", "EquipmentId");
            AddForeignKey("dbo.Smarts", "EquipmentId", "dbo.Equipments", "EquipmentId");
        }
    }
}
