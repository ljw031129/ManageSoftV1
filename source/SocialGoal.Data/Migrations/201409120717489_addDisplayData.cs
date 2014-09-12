namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDisplayData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReceiveDataDispalies",
                c => new
                    {
                        ReceiveDataDispalyId = c.String(nullable: false, maxLength: 128),
                        DictionaryKey = c.String(),
                        DefaultValue = c.String(),
                        ShowColor = c.String(),
                        ChartRange = c.String(),
                        ShowType = c.String(),
                        ShowIcon = c.String(),
                        ShowPostion = c.String(),
                    })
                .PrimaryKey(t => t.ReceiveDataDispalyId);
            
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
                .PrimaryKey(t => t.ReDataDispalyFormatId)
                .ForeignKey("dbo.ReceiveDataDispalies", t => t.ReceiveDataDispalyId, cascadeDelete: true)
                .Index(t => t.ReceiveDataDispalyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReDataDispalyFormats", "ReceiveDataDispalyId", "dbo.ReceiveDataDispalies");
            DropIndex("dbo.ReDataDispalyFormats", new[] { "ReceiveDataDispalyId" });
            DropTable("dbo.ReDataDispalyFormats");
            DropTable("dbo.ReceiveDataDispalies");
        }
    }
}
