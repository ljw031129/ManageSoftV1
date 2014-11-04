namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDisFormoptionV1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReceiveDataDisplays", "ShowWidth", c => c.String());
            AddColumn("dbo.ReceiveDataDisplays", "ShowTable", c => c.Boolean(nullable: false));
            DropColumn("dbo.ReceiveDataDisplays", "ShowType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReceiveDataDisplays", "ShowType", c => c.String());
            DropColumn("dbo.ReceiveDataDisplays", "ShowTable");
            DropColumn("dbo.ReceiveDataDisplays", "ShowWidth");
        }
    }
}
