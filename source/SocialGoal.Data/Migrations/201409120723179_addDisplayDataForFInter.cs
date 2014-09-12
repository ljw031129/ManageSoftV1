namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDisplayDataForFInter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReceiveDataDispalies", "PmFInterpreterId", c => c.String(maxLength: 128));
            CreateIndex("dbo.ReceiveDataDispalies", "PmFInterpreterId");
            AddForeignKey("dbo.ReceiveDataDispalies", "PmFInterpreterId", "dbo.PmFInterpreters", "PmFInterpreterId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReceiveDataDispalies", "PmFInterpreterId", "dbo.PmFInterpreters");
            DropIndex("dbo.ReceiveDataDispalies", new[] { "PmFInterpreterId" });
            DropColumn("dbo.ReceiveDataDispalies", "PmFInterpreterId");
        }
    }
}
