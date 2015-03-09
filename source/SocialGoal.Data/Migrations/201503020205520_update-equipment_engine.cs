namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateequipment_engine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Equipments", "EngineNum", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Equipments", "EngineNum");
        }
    }
}
