namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addte : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Equipments", "EquipmentTypeId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Equipments", "EquipmentTypeId");
        }
    }
}
