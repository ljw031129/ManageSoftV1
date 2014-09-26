namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTlist1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TerminalEquipmentOrgStructures", newName: "OrgStructureTerminalEquipments");
            DropForeignKey("dbo.Equipments", "TerminalEquipment_TerminalEquipmentId", "dbo.TerminalEquipments");
            DropIndex("dbo.Equipments", new[] { "TerminalEquipment_TerminalEquipmentId" });
            DropPrimaryKey("dbo.OrgStructureTerminalEquipments");
            AlterColumn("dbo.TerminalEquipments", "EquipmentId", c => c.String(maxLength: 36));
            AddPrimaryKey("dbo.OrgStructureTerminalEquipments", new[] { "OrgStructure_OrgStructureId", "TerminalEquipment_TerminalEquipmentId" });
            CreateIndex("dbo.TerminalEquipments", "EquipmentId");
            AddForeignKey("dbo.TerminalEquipments", "EquipmentId", "dbo.Equipments", "EquipmentId");
            DropColumn("dbo.Equipments", "TerminalEquipment_TerminalEquipmentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Equipments", "TerminalEquipment_TerminalEquipmentId", c => c.String(maxLength: 128));
            DropForeignKey("dbo.TerminalEquipments", "EquipmentId", "dbo.Equipments");
            DropIndex("dbo.TerminalEquipments", new[] { "EquipmentId" });
            DropPrimaryKey("dbo.OrgStructureTerminalEquipments");
            AlterColumn("dbo.TerminalEquipments", "EquipmentId", c => c.String());
            AddPrimaryKey("dbo.OrgStructureTerminalEquipments", new[] { "TerminalEquipment_TerminalEquipmentId", "OrgStructure_OrgStructureId" });
            CreateIndex("dbo.Equipments", "TerminalEquipment_TerminalEquipmentId");
            AddForeignKey("dbo.Equipments", "TerminalEquipment_TerminalEquipmentId", "dbo.TerminalEquipments", "TerminalEquipmentId");
            RenameTable(name: "dbo.OrgStructureTerminalEquipments", newName: "TerminalEquipmentOrgStructures");
        }
    }
}
