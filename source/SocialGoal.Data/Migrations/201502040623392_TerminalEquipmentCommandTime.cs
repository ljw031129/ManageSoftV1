namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TerminalEquipmentCommandTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TerminalEquipmentCommands", "OperateTime", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TerminalEquipmentCommands", "OperateTime", c => c.String());
        }
    }
}
