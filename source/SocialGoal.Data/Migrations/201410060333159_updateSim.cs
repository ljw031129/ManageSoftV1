namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateSim : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TerminalSimCards", "TerminalSimCardState", c => c.String());
            AddColumn("dbo.TerminalSimCards", "TerminalSimCardDescribe", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TerminalSimCards", "TerminalSimCardDescribe");
            DropColumn("dbo.TerminalSimCards", "TerminalSimCardState");
        }
    }
}
