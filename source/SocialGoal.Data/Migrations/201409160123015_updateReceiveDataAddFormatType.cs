namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateReceiveDataAddFormatType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReceiveDataDisplays", "DictionaryValue", c => c.String());
            AddColumn("dbo.ReceiveDataDisplays", "FormatType", c => c.Int(nullable: false));
            DropColumn("dbo.ReDataDisplayFormats", "FormatType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReDataDisplayFormats", "FormatType", c => c.Int(nullable: false));
            DropColumn("dbo.ReceiveDataDisplays", "FormatType");
            DropColumn("dbo.ReceiveDataDisplays", "DictionaryValue");
        }
    }
}
