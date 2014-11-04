namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDisFormoption : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReceiveDataDisplays", "Formatter", c => c.String());
            AddColumn("dbo.ReceiveDataDisplays", "Formatoptions", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReceiveDataDisplays", "Formatoptions");
            DropColumn("dbo.ReceiveDataDisplays", "Formatter");
        }
    }
}
