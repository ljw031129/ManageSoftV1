namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtVsO : DbMigration
    {
        public override void Up()
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
            
            AddColumn("dbo.OrgStructures", "TerminalEquipmentVsOrgStructure_TerminalEquipmentVsOrgStructureId", c => c.String(maxLength: 128));
            AddColumn("dbo.TerminalEquipments", "OrgEnterpriseId", c => c.String(maxLength: 128));
            AddColumn("dbo.TerminalEquipments", "TerminalEquipmentVsOrgStructure_TerminalEquipmentVsOrgStructureId", c => c.String(maxLength: 128));
            CreateIndex("dbo.OrgStructures", "TerminalEquipmentVsOrgStructure_TerminalEquipmentVsOrgStructureId");
            CreateIndex("dbo.TerminalEquipments", "OrgEnterpriseId");
            CreateIndex("dbo.TerminalEquipments", "TerminalEquipmentVsOrgStructure_TerminalEquipmentVsOrgStructureId");
            AddForeignKey("dbo.TerminalEquipments", "OrgEnterpriseId", "dbo.OrgEnterprises", "OrgEnterpriseId");
            AddForeignKey("dbo.OrgStructures", "TerminalEquipmentVsOrgStructure_TerminalEquipmentVsOrgStructureId", "dbo.TerminalEquipmentVsOrgStructures", "TerminalEquipmentVsOrgStructureId");
            AddForeignKey("dbo.TerminalEquipments", "TerminalEquipmentVsOrgStructure_TerminalEquipmentVsOrgStructureId", "dbo.TerminalEquipmentVsOrgStructures", "TerminalEquipmentVsOrgStructureId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TerminalEquipments", "TerminalEquipmentVsOrgStructure_TerminalEquipmentVsOrgStructureId", "dbo.TerminalEquipmentVsOrgStructures");
            DropForeignKey("dbo.OrgStructures", "TerminalEquipmentVsOrgStructure_TerminalEquipmentVsOrgStructureId", "dbo.TerminalEquipmentVsOrgStructures");
            DropForeignKey("dbo.TerminalEquipments", "OrgEnterpriseId", "dbo.OrgEnterprises");
            DropIndex("dbo.TerminalEquipments", new[] { "TerminalEquipmentVsOrgStructure_TerminalEquipmentVsOrgStructureId" });
            DropIndex("dbo.TerminalEquipments", new[] { "OrgEnterpriseId" });
            DropIndex("dbo.OrgStructures", new[] { "TerminalEquipmentVsOrgStructure_TerminalEquipmentVsOrgStructureId" });
            DropColumn("dbo.TerminalEquipments", "TerminalEquipmentVsOrgStructure_TerminalEquipmentVsOrgStructureId");
            DropColumn("dbo.TerminalEquipments", "OrgEnterpriseId");
            DropColumn("dbo.OrgStructures", "TerminalEquipmentVsOrgStructure_TerminalEquipmentVsOrgStructureId");
            DropTable("dbo.TerminalEquipmentVsOrgStructures");
        }
    }
}
