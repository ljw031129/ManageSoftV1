namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TerminalEquipmentCommandHex : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TerminalEquipmentCommands", "OperateDataHex", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TerminalEquipmentCommands", "OperateDataHex");
        }
    }
}
