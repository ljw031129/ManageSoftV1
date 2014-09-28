namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addlast : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TerminalEquipments", "ReceiveDataLastId", c => c.String(maxLength: 128));
            CreateIndex("dbo.TerminalEquipments", "ReceiveDataLastId");
            AddForeignKey("dbo.TerminalEquipments", "ReceiveDataLastId", "dbo.ReceiveDataLasts", "ReceiveDataLastId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TerminalEquipments", "ReceiveDataLastId", "dbo.ReceiveDataLasts");
            DropIndex("dbo.TerminalEquipments", new[] { "ReceiveDataLastId" });
            DropColumn("dbo.TerminalEquipments", "ReceiveDataLastId");
        }
    }
}
