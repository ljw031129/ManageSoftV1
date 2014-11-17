namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateOrgStructure2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrgStructureTerminalEquipments", "OrgStructure_OrgStructureId", "dbo.OrgStructures");
            DropForeignKey("dbo.OrgStructureTerminalEquipments", "TerminalEquipment_TerminalEquipmentId", "dbo.TerminalEquipments");
            DropIndex("dbo.OrgStructureTerminalEquipments", new[] { "OrgStructure_OrgStructureId" });
            DropIndex("dbo.OrgStructureTerminalEquipments", new[] { "TerminalEquipment_TerminalEquipmentId" });
            AddColumn("dbo.TerminalEquipments", "UserId", c => c.String(maxLength: 128));
            AddColumn("dbo.TerminalEquipments", "OrgStructure_OrgStructureId", c => c.String(maxLength: 128));
            AddColumn("dbo.OrgEnterprises", "OrgEnterprisePId", c => c.String());
            AddColumn("dbo.AspNetUsers", "OrgEnterpriseId", c => c.String(maxLength: 128));
            CreateIndex("dbo.TerminalEquipments", "UserId");
            CreateIndex("dbo.TerminalEquipments", "OrgStructure_OrgStructureId");
            CreateIndex("dbo.AspNetUsers", "OrgEnterpriseId");
            AddForeignKey("dbo.AspNetUsers", "OrgEnterpriseId", "dbo.OrgEnterprises", "OrgEnterpriseId");
            AddForeignKey("dbo.TerminalEquipments", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.TerminalEquipments", "OrgStructure_OrgStructureId", "dbo.OrgStructures", "OrgStructureId");
            DropTable("dbo.OrgStructureTerminalEquipments");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OrgStructureTerminalEquipments",
                c => new
                    {
                        OrgStructure_OrgStructureId = c.String(nullable: false, maxLength: 128),
                        TerminalEquipment_TerminalEquipmentId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.OrgStructure_OrgStructureId, t.TerminalEquipment_TerminalEquipmentId });
            
            DropForeignKey("dbo.TerminalEquipments", "OrgStructure_OrgStructureId", "dbo.OrgStructures");
            DropForeignKey("dbo.TerminalEquipments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "OrgEnterpriseId", "dbo.OrgEnterprises");
            DropIndex("dbo.AspNetUsers", new[] { "OrgEnterpriseId" });
            DropIndex("dbo.TerminalEquipments", new[] { "OrgStructure_OrgStructureId" });
            DropIndex("dbo.TerminalEquipments", new[] { "UserId" });
            DropColumn("dbo.AspNetUsers", "OrgEnterpriseId");
            DropColumn("dbo.OrgEnterprises", "OrgEnterprisePId");
            DropColumn("dbo.TerminalEquipments", "OrgStructure_OrgStructureId");
            DropColumn("dbo.TerminalEquipments", "UserId");
            CreateIndex("dbo.OrgStructureTerminalEquipments", "TerminalEquipment_TerminalEquipmentId");
            CreateIndex("dbo.OrgStructureTerminalEquipments", "OrgStructure_OrgStructureId");
            AddForeignKey("dbo.OrgStructureTerminalEquipments", "TerminalEquipment_TerminalEquipmentId", "dbo.TerminalEquipments", "TerminalEquipmentId", cascadeDelete: true);
            AddForeignKey("dbo.OrgStructureTerminalEquipments", "OrgStructure_OrgStructureId", "dbo.OrgStructures", "OrgStructureId", cascadeDelete: true);
        }
    }
}
