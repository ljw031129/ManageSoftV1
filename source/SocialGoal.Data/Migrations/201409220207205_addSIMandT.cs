namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSIMandT : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TerminalEquipments",
                c => new
                    {
                        TerminalEquipmentId = c.String(nullable: false, maxLength: 128),
                        TerminalEquipmentNum = c.String(),
                        TerminalEquipmentType = c.String(),
                        PmFInterpreterId = c.String(maxLength: 128),
                        TerminalSimCardId = c.String(maxLength: 128),
                        TerminalEquipmentCreateTime = c.DateTime(nullable: false),
                        TerminalEquipmentUpdateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TerminalEquipmentId)
                .ForeignKey("dbo.PmFInterpreters", t => t.PmFInterpreterId)
                .ForeignKey("dbo.TerminalSimCards", t => t.TerminalSimCardId)
                .Index(t => t.PmFInterpreterId)
                .Index(t => t.TerminalSimCardId);
            
            CreateTable(
                "dbo.TerminalSimCards",
                c => new
                    {
                        TerminalSimCardId = c.String(nullable: false, maxLength: 128),
                        TerminalSimCardNum = c.String(),
                        TerminalSimCardSerialNum = c.String(),
                        TerminalSimCardCreateTime = c.DateTime(nullable: false),
                        TerminalSimCardUpdateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TerminalSimCardId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TerminalEquipments", "TerminalSimCardId", "dbo.TerminalSimCards");
            DropForeignKey("dbo.TerminalEquipments", "PmFInterpreterId", "dbo.PmFInterpreters");
            DropIndex("dbo.TerminalEquipments", new[] { "TerminalSimCardId" });
            DropIndex("dbo.TerminalEquipments", new[] { "PmFInterpreterId" });
            DropTable("dbo.TerminalSimCards");
            DropTable("dbo.TerminalEquipments");
        }
    }
}
