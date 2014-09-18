namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addOrderIndex : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PmDataBodies", "OrderIndex", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PmDataBodies", "OrderIndex");
        }
    }
}
