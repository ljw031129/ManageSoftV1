namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDis : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReceiveDataDisplays", "Alignment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReceiveDataDisplays", "Alignment");
        }
    }
}
