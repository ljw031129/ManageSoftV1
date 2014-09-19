namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPrg : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrgEnterprises",
                c => new
                    {
                        OrgEnterpriseId = c.String(nullable: false, maxLength: 128),
                        OrgEnterpriseNum = c.String(),
                        OrgEnterpriseName = c.String(),
                        OrgEnterpriseDescribe = c.String(),
                        OrgEnterpriseCreateTime = c.DateTime(nullable: false),
                        OrgEnterpriseUpdateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.OrgEnterpriseId);
            
            CreateTable(
                "dbo.OrgStructures",
                c => new
                    {
                        OrgStructureId = c.String(nullable: false, maxLength: 128),
                        OrgStructurePId = c.String(),
                        OrgStructureNum = c.String(),
                        OrgStructureName = c.String(),
                        OrgStructureDescribe = c.String(),
                        OrgStructureCreateTime = c.DateTime(nullable: false),
                        OrgStructureUpdateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.OrgStructureId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OrgStructures");
            DropTable("dbo.OrgEnterprises");
        }
    }
}
