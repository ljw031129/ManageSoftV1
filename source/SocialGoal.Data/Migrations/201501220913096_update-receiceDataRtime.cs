namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatereceiceDataRtime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ReceiveDatas", "ReceiveTime", c => c.Long(nullable: false));
            AlterColumn("dbo.ReceiveDatas", "CollectTime", c => c.Long(nullable: false));
            AlterColumn("dbo.ReceiveDatas", "GpsTime", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ReceiveDatas", "GpsTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ReceiveDatas", "CollectTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ReceiveDatas", "ReceiveTime", c => c.DateTime(nullable: false));
        }
    }
}
