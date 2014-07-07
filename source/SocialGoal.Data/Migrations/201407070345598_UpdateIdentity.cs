namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateIdentity : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.AspNetUserClaims", name: "User_Id", newName: "UserId");
            DropPrimaryKey("dbo.AspNetUserLogins");
            AddColumn("dbo.AspNetUsers", "EmailConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "PhoneNumber", c => c.String());
            AddColumn("dbo.AspNetUsers", "PhoneNumberConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "TwoFactorEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "LockoutEndDateUtc", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "LockoutEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "AccessFailedCount", c => c.Int(nullable: false));
            AlterColumn("dbo.AspNetUsers", "UserName", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.AspNetUsers", "DateCreated", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Activated", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AspNetUsers", "RoleId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.AspNetUserLogins", new[] { "LoginProvider", "ProviderKey", "UserId" });
           // CreateIndex("dbo.Comments", "UpdateId");
            //CreateIndex("dbo.Updates", "GoalId");
            //CreateIndex("dbo.Goals", "MetricId");
            //CreateIndex("dbo.Goals", "GoalStatusId");
            //CreateIndex("dbo.Goals", "UserId");
            //CreateIndex("dbo.GroupGoals", "MetricId");
            //CreateIndex("dbo.GroupGoals", "FocusId");
            //CreateIndex("dbo.GroupGoals", "GoalStatusId");
            //CreateIndex("dbo.GroupGoals", "GroupUserId");
            //CreateIndex("dbo.GroupGoals", "GroupId");
            //CreateIndex("dbo.Foci", "GroupId");
            //CreateIndex("dbo.GroupInvitations", "GroupId");
            //CreateIndex("dbo.GroupRequests", "GroupId");
            //CreateIndex("dbo.GroupRequests", "UserId");
            //CreateIndex("dbo.AspNetUsers", "UserName", unique: true, name: "UserNameIndex");
            //CreateIndex("dbo.AspNetUserClaims", "UserId");
            //CreateIndex("dbo.FollowUsers", "ToUserId");
            //CreateIndex("dbo.FollowUsers", "FromUserId");
            //CreateIndex("dbo.FollowUsers", "ApplicationUser_Id");
            //CreateIndex("dbo.FollowUsers", "ApplicationUser_Id1");
            //CreateIndex("dbo.AspNetUserLogins", "UserId");
            //CreateIndex("dbo.AspNetUserRoles", "UserId");
            //CreateIndex("dbo.AspNetUserRoles", "RoleId");
            //CreateIndex("dbo.GroupUpdates", "GroupGoalId");
            //CreateIndex("dbo.GroupComments", "GroupUpdateId");
            //CreateIndex("dbo.SupportInvitations", "GoalId");
            //CreateIndex("dbo.Supports", "GoalId");
            //CreateIndex("dbo.UpdateSupports", "UpdateId");
            //CreateIndex("dbo.GroupUpdateSupports", "GroupUpdateId");
            //CreateIndex("dbo.AspNetRoles", "Name", unique: true, name: "RoleNameIndex");
            //CreateIndex("dbo.Smarts", "EquipmentId");
            DropColumn("dbo.AspNetUsers", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropIndex("dbo.Smarts", new[] { "EquipmentId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.GroupUpdateSupports", new[] { "GroupUpdateId" });
            DropIndex("dbo.UpdateSupports", new[] { "UpdateId" });
            DropIndex("dbo.Supports", new[] { "GoalId" });
            DropIndex("dbo.SupportInvitations", new[] { "GoalId" });
            DropIndex("dbo.GroupComments", new[] { "GroupUpdateId" });
            DropIndex("dbo.GroupUpdates", new[] { "GroupGoalId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.FollowUsers", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.FollowUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.FollowUsers", new[] { "FromUserId" });
            DropIndex("dbo.FollowUsers", new[] { "ToUserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.GroupRequests", new[] { "UserId" });
            DropIndex("dbo.GroupRequests", new[] { "GroupId" });
            DropIndex("dbo.GroupInvitations", new[] { "GroupId" });
            DropIndex("dbo.Foci", new[] { "GroupId" });
            DropIndex("dbo.GroupGoals", new[] { "GroupId" });
            DropIndex("dbo.GroupGoals", new[] { "GroupUserId" });
            DropIndex("dbo.GroupGoals", new[] { "GoalStatusId" });
            DropIndex("dbo.GroupGoals", new[] { "FocusId" });
            DropIndex("dbo.GroupGoals", new[] { "MetricId" });
            DropIndex("dbo.Goals", new[] { "UserId" });
            DropIndex("dbo.Goals", new[] { "GoalStatusId" });
            DropIndex("dbo.Goals", new[] { "MetricId" });
            DropIndex("dbo.Updates", new[] { "GoalId" });
            DropIndex("dbo.Comments", new[] { "UpdateId" });
            DropPrimaryKey("dbo.AspNetUserLogins");
            AlterColumn("dbo.AspNetUsers", "RoleId", c => c.Int());
            AlterColumn("dbo.AspNetUsers", "Activated", c => c.Boolean());
            AlterColumn("dbo.AspNetUsers", "DateCreated", c => c.DateTime());
            AlterColumn("dbo.AspNetUsers", "UserName", c => c.String());
            DropColumn("dbo.AspNetUsers", "AccessFailedCount");
            DropColumn("dbo.AspNetUsers", "LockoutEnabled");
            DropColumn("dbo.AspNetUsers", "LockoutEndDateUtc");
            DropColumn("dbo.AspNetUsers", "TwoFactorEnabled");
            DropColumn("dbo.AspNetUsers", "PhoneNumberConfirmed");
            DropColumn("dbo.AspNetUsers", "PhoneNumber");
            DropColumn("dbo.AspNetUsers", "EmailConfirmed");
            AddPrimaryKey("dbo.AspNetUserLogins", new[] { "UserId", "LoginProvider", "ProviderKey" });
            RenameColumn(table: "dbo.AspNetUserClaims", name: "UserId", newName: "User_Id");
        }
    }
}
