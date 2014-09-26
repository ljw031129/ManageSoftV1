namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTlist : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Equipments", "TerminalEquipment_TerminalEquipmentId", c => c.String(maxLength: 128));
            AddColumn("dbo.TerminalEquipments", "EquipmentId", c => c.String());
            CreateIndex("dbo.Equipments", "TerminalEquipment_TerminalEquipmentId");
            AddForeignKey("dbo.Equipments", "TerminalEquipment_TerminalEquipmentId", "dbo.TerminalEquipments", "TerminalEquipmentId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Equipments", "TerminalEquipment_TerminalEquipmentId", "dbo.TerminalEquipments");
            DropIndex("dbo.Equipments", new[] { "TerminalEquipment_TerminalEquipmentId" });
            DropColumn("dbo.TerminalEquipments", "EquipmentId");
            DropColumn("dbo.Equipments", "TerminalEquipment_TerminalEquipmentId");
        }
    }
}
