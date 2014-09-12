namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateReceiveData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReceiveDataDispalies", "ShowOrder", c => c.Int(nullable: false));
            AddColumn("dbo.ReceiveDataDispalies", "ShowUnit", c => c.String());
            AddColumn("dbo.ReceiveDataDispalies", "ShowCommon", c => c.Boolean(nullable: false));
            DropColumn("dbo.ReceiveDataDispalies", "DefaultValue");
            DropColumn("dbo.ReceiveDataDispalies", "ShowColor");
            DropColumn("dbo.ReceiveDataDispalies", "ChartRange");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReceiveDataDispalies", "ChartRange", c => c.String());
            AddColumn("dbo.ReceiveDataDispalies", "ShowColor", c => c.String());
            AddColumn("dbo.ReceiveDataDispalies", "DefaultValue", c => c.String());
            DropColumn("dbo.ReceiveDataDispalies", "ShowCommon");
            DropColumn("dbo.ReceiveDataDispalies", "ShowUnit");
            DropColumn("dbo.ReceiveDataDispalies", "ShowOrder");
        }
    }
}
