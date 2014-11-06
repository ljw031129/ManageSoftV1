namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateJQgrid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReceiveDataDisplays", "Width", c => c.Int(nullable: false));
            AddColumn("dbo.ReceiveDataDisplays", "Search", c => c.Boolean(nullable: false));
            AddColumn("dbo.ReceiveDataDisplays", "Searchoptions", c => c.String());
            AddColumn("dbo.ReceiveDataDisplays", "sorttype", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReceiveDataDisplays", "sorttype");
            DropColumn("dbo.ReceiveDataDisplays", "Searchoptions");
            DropColumn("dbo.ReceiveDataDisplays", "Search");
            DropColumn("dbo.ReceiveDataDisplays", "Width");
        }
    }
}
