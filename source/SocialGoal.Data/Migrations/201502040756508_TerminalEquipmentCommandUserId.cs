namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TerminalEquipmentCommandUserId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TerminalEquipmentCommands", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.TerminalEquipmentCommands", "UserId");
            AddForeignKey("dbo.TerminalEquipmentCommands", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TerminalEquipmentCommands", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.TerminalEquipmentCommands", new[] { "UserId" });
            DropColumn("dbo.TerminalEquipmentCommands", "UserId");
        }
    }
}
