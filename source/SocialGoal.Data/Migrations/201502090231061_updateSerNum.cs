namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateSerNum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TerminalEquipmentCommandCurrents", "SerNum", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.TerminalEquipmentCommands", "SerNum", c => c.Int(nullable: false, identity: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TerminalEquipmentCommands", "SerNum");
            DropColumn("dbo.TerminalEquipmentCommandCurrents", "SerNum");
        }
    }
}
