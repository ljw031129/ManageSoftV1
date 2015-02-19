namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReceiveTData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TerminalEquipmentCommands", "ReceiveTData", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TerminalEquipmentCommands", "ReceiveTData");
        }
    }
}
