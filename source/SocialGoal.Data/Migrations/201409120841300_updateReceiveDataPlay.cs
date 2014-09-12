namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateReceiveDataPlay : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ReceiveDataDispalies", newName: "ReceiveDataDisplays");
            DropForeignKey("dbo.ReDataDispalyFormats", "ReceiveDataDispalyId", "dbo.ReceiveDataDispalies");
            DropForeignKey("dbo.ReDataDisplayFormats", "ReceiveDataDisplayId", "dbo.ReceiveDataDisplays");
            DropIndex("dbo.ReDataDispalyFormats", new[] { "ReceiveDataDispalyId" });
            DropPrimaryKey("dbo.ReceiveDataDisplays");
            CreateTable(
                "dbo.ReDataDisplayFormats",
                c => new
                    {
                        ReDataDisplayFormatId = c.String(nullable: false, maxLength: 128),
                        FormatType = c.Int(nullable: false),
                        FormatExpression = c.String(),
                        FormatValue = c.String(),
                        FormatColor = c.String(),
                        ReceiveDataDisplayId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ReDataDisplayFormatId)
                .ForeignKey("dbo.ReceiveDataDisplays", t => t.ReceiveDataDisplayId, cascadeDelete: true)
                .Index(t => t.ReceiveDataDisplayId);
            
            AddColumn("dbo.ReceiveDataDisplays", "ReceiveDataDisplayId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.ReceiveDataDisplays", "ReceiveDataDisplayId");
            DropColumn("dbo.ReceiveDataDisplays", "ReceiveDataDispalyId");
            DropTable("dbo.ReDataDispalyFormats");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ReDataDispalyFormats",
                c => new
                    {
                        ReDataDispalyFormatId = c.String(nullable: false, maxLength: 128),
                        FormatType = c.Int(nullable: false),
                        FormatExpression = c.String(),
                        FormatValue = c.String(),
                        FormatColor = c.String(),
                        ReceiveDataDispalyId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ReDataDispalyFormatId);
            
            AddColumn("dbo.ReceiveDataDisplays", "ReceiveDataDispalyId", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.ReDataDisplayFormats", "ReceiveDataDisplayId", "dbo.ReceiveDataDisplays");
            DropIndex("dbo.ReDataDisplayFormats", new[] { "ReceiveDataDisplayId" });
            DropPrimaryKey("dbo.ReceiveDataDisplays");
            DropColumn("dbo.ReceiveDataDisplays", "ReceiveDataDisplayId");
            DropTable("dbo.ReDataDisplayFormats");
            AddPrimaryKey("dbo.ReceiveDataDisplays", "ReceiveDataDispalyId");
            CreateIndex("dbo.ReDataDispalyFormats", "ReceiveDataDispalyId");
            AddForeignKey("dbo.ReDataDisplayFormats", "ReceiveDataDisplayId", "dbo.ReceiveDataDisplays", "ReceiveDataDisplayId", cascadeDelete: true);
            AddForeignKey("dbo.ReDataDispalyFormats", "ReceiveDataDispalyId", "dbo.ReceiveDataDispalies", "ReceiveDataDispalyId", cascadeDelete: true);
            RenameTable(name: "dbo.ReceiveDataDisplays", newName: "ReceiveDataDispalies");
        }
    }
}
