namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TerminalEquipmentCommandStatue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TerminalEquipmentCommands", "OperateStatue", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TerminalEquipmentCommands", "OperateStatue");
        }
    }
}
