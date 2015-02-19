namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateIMEI : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReceiveDataLasts", "IMEI", c => c.String(maxLength: 50));
            AddColumn("dbo.ReceiveDataHistories", "IMEI", c => c.String());
            AddColumn("dbo.ReceiveDatas", "IMEI", c => c.String(maxLength: 50));
            DropColumn("dbo.ReceiveDataLasts", "DevId");
            DropColumn("dbo.ReceiveDataHistories", "DevId");
            DropColumn("dbo.ReceiveDatas", "DevId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReceiveDatas", "DevId", c => c.String(maxLength: 50));
            AddColumn("dbo.ReceiveDataHistories", "DevId", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "DevId", c => c.String(maxLength: 50));
            DropColumn("dbo.ReceiveDatas", "IMEI");
            DropColumn("dbo.ReceiveDataHistories", "IMEI");
            DropColumn("dbo.ReceiveDataLasts", "IMEI");
        }
    }
}
