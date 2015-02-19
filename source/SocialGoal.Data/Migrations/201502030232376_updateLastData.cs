namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateLastData : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Equipments", "EquipmentNum", c => c.String(maxLength: 50));
            AlterColumn("dbo.ReceiveDataLasts", "DevId", c => c.String(maxLength: 50));
            AlterColumn("dbo.ReceiveDataLasts", "ReceiveTime", c => c.Long(nullable: false));
            AlterColumn("dbo.ReceiveDataLasts", "CollectTime", c => c.Long(nullable: false));
            AlterColumn("dbo.ReceiveDataLasts", "GpsTime", c => c.Long(nullable: false));
            AlterColumn("dbo.ReceiveDatas", "DevId", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ReceiveDatas", "DevId", c => c.String());
            AlterColumn("dbo.ReceiveDataLasts", "GpsTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ReceiveDataLasts", "CollectTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ReceiveDataLasts", "ReceiveTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ReceiveDataLasts", "DevId", c => c.String());
            AlterColumn("dbo.Equipments", "EquipmentNum", c => c.String(maxLength: 8));
        }
    }
}
