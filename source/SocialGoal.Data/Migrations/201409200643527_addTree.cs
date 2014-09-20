namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTree : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrgStructures", "OrgEnterpriseId", c => c.String(maxLength: 128));
            AddColumn("dbo.OrgStructures", "level", c => c.String());
            AddColumn("dbo.OrgStructures", "parent", c => c.String());
            AddColumn("dbo.OrgStructures", "isLeaf", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrgStructures", "expanded", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrgStructures", "loaded", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrgStructures", "icon", c => c.String());
            CreateIndex("dbo.OrgStructures", "OrgEnterpriseId");
            AddForeignKey("dbo.OrgStructures", "OrgEnterpriseId", "dbo.OrgEnterprises", "OrgEnterpriseId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrgStructures", "OrgEnterpriseId", "dbo.OrgEnterprises");
            DropIndex("dbo.OrgStructures", new[] { "OrgEnterpriseId" });
            DropColumn("dbo.OrgStructures", "icon");
            DropColumn("dbo.OrgStructures", "loaded");
            DropColumn("dbo.OrgStructures", "expanded");
            DropColumn("dbo.OrgStructures", "isLeaf");
            DropColumn("dbo.OrgStructures", "parent");
            DropColumn("dbo.OrgStructures", "level");
            DropColumn("dbo.OrgStructures", "OrgEnterpriseId");
        }
    }
}
