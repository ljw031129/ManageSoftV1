namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateEquipment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Equipments", "OrgEnterpriseId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Equipments", "OrgEnterpriseId");
            AddForeignKey("dbo.Equipments", "OrgEnterpriseId", "dbo.OrgEnterprises", "OrgEnterpriseId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Equipments", "OrgEnterpriseId", "dbo.OrgEnterprises");
            DropIndex("dbo.Equipments", new[] { "OrgEnterpriseId" });
            DropColumn("dbo.Equipments", "OrgEnterpriseId");
        }
    }
}
