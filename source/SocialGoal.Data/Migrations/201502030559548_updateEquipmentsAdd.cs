namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateEquipmentsAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Equipments", "OwnerName", c => c.String());
            AddColumn("dbo.Equipments", "OwnerPhone", c => c.String());
            AddColumn("dbo.Equipments", "OwnerAddress", c => c.String());
            AddColumn("dbo.Equipments", "InstallTime", c => c.String());
            AddColumn("dbo.Equipments", "InstallUser", c => c.String());
            AddColumn("dbo.Equipments", "InstallUserPhone", c => c.String());
            AddColumn("dbo.Equipments", "InstallPlace", c => c.String());
            AddColumn("dbo.Equipments", "InstallSite", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Equipments", "InstallSite");
            DropColumn("dbo.Equipments", "InstallPlace");
            DropColumn("dbo.Equipments", "InstallUserPhone");
            DropColumn("dbo.Equipments", "InstallUser");
            DropColumn("dbo.Equipments", "InstallTime");
            DropColumn("dbo.Equipments", "OwnerAddress");
            DropColumn("dbo.Equipments", "OwnerPhone");
            DropColumn("dbo.Equipments", "OwnerName");
        }
    }
}
