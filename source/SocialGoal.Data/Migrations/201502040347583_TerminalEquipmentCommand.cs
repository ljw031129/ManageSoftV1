namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TerminalEquipmentCommand : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TerminalEquipmentCommands",
                c => new
                    {
                        TerminalEquipmentCommandId = c.String(nullable: false, maxLength: 128),
                        IMEI = c.String(maxLength: 50),
                        OperateTime = c.String(),
                        Dtype = c.String(),
                        CommandJsonData = c.String(),
                        CommandFromTo = c.String(),
                    })
                .PrimaryKey(t => t.TerminalEquipmentCommandId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TerminalEquipmentCommands");
        }
    }
}
