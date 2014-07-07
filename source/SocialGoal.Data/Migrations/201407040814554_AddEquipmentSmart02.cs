namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEquipmentSmart02 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Equipments",
                c => new
                    {
                        EquipmentId = c.String(nullable: false, maxLength: 32),
                        EquipmentNum = c.String(maxLength: 8),
                        EquipmentName = c.String(maxLength: 50),
                        EquipmentCreatTime = c.DateTime(nullable: false),
                        EquipmentUpDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.EquipmentId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Equipments");
        }
    }
}
