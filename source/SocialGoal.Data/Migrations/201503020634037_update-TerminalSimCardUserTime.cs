namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTerminalSimCardUserTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TerminalSimCards", "TerminalSimCardUserTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TerminalSimCards", "TerminalSimCardUserTime");
        }
    }
}
