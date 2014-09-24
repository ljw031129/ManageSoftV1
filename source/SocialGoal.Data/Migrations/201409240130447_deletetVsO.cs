namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletetVsO : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrgStructures", "TerminalEquipmentVsOrgStructure_TerminalEquipmentVsOrgStructureId", "dbo.TerminalEquipmentVsOrgStructures");
            DropForeignKey("dbo.TerminalEquipments", "TerminalEquipmentVsOrgStructure_TerminalEquipmentVsOrgStructureId", "dbo.TerminalEquipmentVsOrgStructures");
            DropIndex("dbo.OrgStructures", new[] { "TerminalEquipmentVsOrgStructure_TerminalEquipmentVsOrgStructureId" });
            DropIndex("dbo.TerminalEquipments", new[] { "TerminalEquipmentVsOrgStructure_TerminalEquipmentVsOrgStructureId" });
            CreateTable(
                "dbo.TerminalEquipmentOrgStructures",
                c => new
                    {
                        TerminalEquipment_TerminalEquipmentId = c.String(nullable: false, maxLength: 128),
                        OrgStructure_OrgStructureId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.TerminalEquipment_TerminalEquipmentId, t.OrgStructure_OrgStructureId })
                .ForeignKey("dbo.TerminalEquipments", t => t.TerminalEquipment_TerminalEquipmentId, cascadeDelete: true)
                .ForeignKey("dbo.OrgStructures", t => t.OrgStructure_OrgStructureId, cascadeDelete: true)
                .Index(t => t.TerminalEquipment_TerminalEquipmentId)
                .Index(t => t.OrgStructure_OrgStructureId);
            
            DropColumn("dbo.OrgStructures", "TerminalEquipmentVsOrgStructure_TerminalEquipmentVsOrgStructureId");
            DropColumn("dbo.TerminalEquipments", "TerminalEquipmentVsOrgStructure_TerminalEquipmentVsOrgStructureId");
            DropTable("dbo.TerminalEquipmentVsOrgStructures");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TerminalEquipmentVsOrgStructures",
                c => new
                    {
                        TerminalEquipmentVsOrgStructureId = c.String(nullable: false, maxLength: 128),
                        OrgStructureId = c.String(),
                        TerminalEquipmentId = c.String(),
                    })
                .PrimaryKey(t => t.TerminalEquipmentVsOrgStructureId);
            
            AddColumn("dbo.TerminalEquipments", "TerminalEquipmentVsOrgStructure_TerminalEquipmentVsOrgStructureId", c => c.String(maxLength: 128));
            AddColumn("dbo.OrgStructures", "TerminalEquipmentVsOrgStructure_TerminalEquipmentVsOrgStructureId", c => c.String(maxLength: 128));
            DropForeignKey("dbo.TerminalEquipmentOrgStructures", "OrgStructure_OrgStructureId", "dbo.OrgStructures");
            DropForeignKey("dbo.TerminalEquipmentOrgStructures", "TerminalEquipment_TerminalEquipmentId", "dbo.TerminalEquipments");
            DropIndex("dbo.TerminalEquipmentOrgStructures", new[] { "OrgStructure_OrgStructureId" });
            DropIndex("dbo.TerminalEquipmentOrgStructures", new[] { "TerminalEquipment_TerminalEquipmentId" });
            DropTable("dbo.TerminalEquipmentOrgStructures");
            CreateIndex("dbo.TerminalEquipments", "TerminalEquipmentVsOrgStructure_TerminalEquipmentVsOrgStructureId");
            CreateIndex("dbo.OrgStructures", "TerminalEquipmentVsOrgStructure_TerminalEquipmentVsOrgStructureId");
            AddForeignKey("dbo.TerminalEquipments", "TerminalEquipmentVsOrgStructure_TerminalEquipmentVsOrgStructureId", "dbo.TerminalEquipmentVsOrgStructures", "TerminalEquipmentVsOrgStructureId");
            AddForeignKey("dbo.OrgStructures", "TerminalEquipmentVsOrgStructure_TerminalEquipmentVsOrgStructureId", "dbo.TerminalEquipmentVsOrgStructures", "TerminalEquipmentVsOrgStructureId");
        }
    }
}
