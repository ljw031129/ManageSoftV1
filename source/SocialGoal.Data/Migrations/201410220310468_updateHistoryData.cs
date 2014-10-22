namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateHistoryData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReceiveDataHistories",
                c => new
                    {
                        ReceiveDataHistoryId = c.String(nullable: false, maxLength: 128),
                        DevId = c.String(),
                        DataStr = c.String(),
                        ReceiveTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ReceiveDataHistoryId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ReceiveDataHistories");
        }
    }
}
