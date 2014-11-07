namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "UpdateId", "dbo.Updates");
            DropForeignKey("dbo.Goals", "GoalStatusId", "dbo.GoalStatus");
            DropForeignKey("dbo.Foci", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.GroupInvitations", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.GroupRequests", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.FollowUsers", "FromUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FollowUsers", "ToUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FollowUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.FollowUsers", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.Goals", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.GroupRequests", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.GroupGoals", "FocusId", "dbo.Foci");
            DropForeignKey("dbo.GroupGoals", "GoalStatusId", "dbo.GoalStatus");
            DropForeignKey("dbo.GroupGoals", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.GroupGoals", "GroupUserId", "dbo.GroupUsers");
            DropForeignKey("dbo.Goals", "MetricId", "dbo.Metrics");
            DropForeignKey("dbo.GroupGoals", "MetricId", "dbo.Metrics");
            DropForeignKey("dbo.GroupComments", "GroupUpdateId", "dbo.GroupUpdates");
            DropForeignKey("dbo.GroupUpdates", "GroupGoalId", "dbo.GroupGoals");
            DropForeignKey("dbo.SupportInvitations", "GoalId", "dbo.Goals");
            DropForeignKey("dbo.Supports", "GoalId", "dbo.Goals");
            DropForeignKey("dbo.Updates", "GoalId", "dbo.Goals");
            DropForeignKey("dbo.UpdateSupports", "UpdateId", "dbo.Updates");
            DropForeignKey("dbo.GroupUpdateSupports", "GroupUpdateId", "dbo.GroupUpdates");
            DropForeignKey("dbo.Smarts", "EquipmentId", "dbo.Equipments");
            DropIndex("dbo.Comments", new[] { "UpdateId" });
            DropIndex("dbo.Updates", new[] { "GoalId" });
            DropIndex("dbo.Goals", new[] { "MetricId" });
            DropIndex("dbo.Goals", new[] { "GoalStatusId" });
            DropIndex("dbo.Goals", new[] { "UserId" });
            DropIndex("dbo.GroupGoals", new[] { "MetricId" });
            DropIndex("dbo.GroupGoals", new[] { "FocusId" });
            DropIndex("dbo.GroupGoals", new[] { "GoalStatusId" });
            DropIndex("dbo.GroupGoals", new[] { "GroupUserId" });
            DropIndex("dbo.GroupGoals", new[] { "GroupId" });
            DropIndex("dbo.Foci", new[] { "GroupId" });
            DropIndex("dbo.GroupInvitations", new[] { "GroupId" });
            DropIndex("dbo.GroupRequests", new[] { "GroupId" });
            DropIndex("dbo.GroupRequests", new[] { "UserId" });
            DropIndex("dbo.FollowUsers", new[] { "ToUserId" });
            DropIndex("dbo.FollowUsers", new[] { "FromUserId" });
            DropIndex("dbo.FollowUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.FollowUsers", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.GroupUpdates", new[] { "GroupGoalId" });
            DropIndex("dbo.GroupComments", new[] { "GroupUpdateId" });
            DropIndex("dbo.SupportInvitations", new[] { "GoalId" });
            DropIndex("dbo.Supports", new[] { "GoalId" });
            DropIndex("dbo.UpdateSupports", new[] { "UpdateId" });
            DropIndex("dbo.GroupUpdateSupports", new[] { "GroupUpdateId" });
            DropIndex("dbo.Smarts", new[] { "EquipmentId" });
            DropTable("dbo.Comments");
            DropTable("dbo.Updates");
            DropTable("dbo.Goals");
            DropTable("dbo.GoalStatus");
            DropTable("dbo.GroupGoals");
            DropTable("dbo.Foci");
            DropTable("dbo.Groups");
            DropTable("dbo.GroupInvitations");
            DropTable("dbo.GroupRequests");
            DropTable("dbo.FollowUsers");
            DropTable("dbo.GroupUsers");
            DropTable("dbo.Metrics");
            DropTable("dbo.GroupUpdates");
            DropTable("dbo.GroupComments");
            DropTable("dbo.SupportInvitations");
            DropTable("dbo.Supports");
            DropTable("dbo.UpdateSupports");
            DropTable("dbo.CommentUsers");
            DropTable("dbo.FollowRequests");
            DropTable("dbo.GroupCommentUsers");
            DropTable("dbo.GroupUpdateSupports");
            DropTable("dbo.GroupUpdateUsers");
            DropTable("dbo.Smarts");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Smarts",
                c => new
                    {
                        SmartId = c.String(nullable: false, maxLength: 32),
                        SmartNum = c.String(maxLength: 8),
                        SmartSim = c.String(),
                        SmartCreatTime = c.DateTime(nullable: false),
                        SmartUpdateTime = c.DateTime(nullable: false),
                        EquipmentId = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.SmartId);
            
            CreateTable(
                "dbo.GroupUpdateUsers",
                c => new
                    {
                        GroupUpdateUserId = c.Int(nullable: false, identity: true),
                        GroupUpdateId = c.Int(nullable: false),
                        UserId = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.GroupUpdateUserId);
            
            CreateTable(
                "dbo.GroupUpdateSupports",
                c => new
                    {
                        GroupUpdateSupportId = c.Int(nullable: false, identity: true),
                        GroupUpdateId = c.Int(nullable: false),
                        GroupUserId = c.Int(nullable: false),
                        UpdateSupportedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GroupUpdateSupportId);
            
            CreateTable(
                "dbo.GroupCommentUsers",
                c => new
                    {
                        GroupCommentUserId = c.Int(nullable: false, identity: true),
                        GroupCommentId = c.Int(nullable: false),
                        UserId = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.GroupCommentUserId);
            
            CreateTable(
                "dbo.FollowRequests",
                c => new
                    {
                        FollowRequestId = c.Int(nullable: false, identity: true),
                        FromUserId = c.String(nullable: false),
                        ToUserId = c.String(nullable: false),
                        Accepted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.FollowRequestId);
            
            CreateTable(
                "dbo.CommentUsers",
                c => new
                    {
                        CommentUserId = c.Int(nullable: false, identity: true),
                        CommentId = c.Int(nullable: false),
                        UserId = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CommentUserId);
            
            CreateTable(
                "dbo.UpdateSupports",
                c => new
                    {
                        UpdateSupportId = c.Int(nullable: false, identity: true),
                        UpdateId = c.Int(nullable: false),
                        UserId = c.String(),
                        UpdateSupportedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UpdateSupportId);
            
            CreateTable(
                "dbo.Supports",
                c => new
                    {
                        SupportId = c.Int(nullable: false, identity: true),
                        GoalId = c.Int(nullable: false),
                        UserId = c.String(),
                        SupportedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SupportId);
            
            CreateTable(
                "dbo.SupportInvitations",
                c => new
                    {
                        SupportInvitationId = c.Int(nullable: false, identity: true),
                        FromUserId = c.String(),
                        GoalId = c.Int(nullable: false),
                        ToUserId = c.String(),
                        SentDate = c.DateTime(nullable: false),
                        Accepted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SupportInvitationId);
            
            CreateTable(
                "dbo.GroupComments",
                c => new
                    {
                        GroupCommentId = c.Int(nullable: false, identity: true),
                        CommentText = c.String(),
                        GroupUpdateId = c.Int(nullable: false),
                        CommentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GroupCommentId);
            
            CreateTable(
                "dbo.GroupUpdates",
                c => new
                    {
                        GroupUpdateId = c.Int(nullable: false, identity: true),
                        Updatemsg = c.String(),
                        status = c.Double(),
                        GroupGoalId = c.Int(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GroupUpdateId);
            
            CreateTable(
                "dbo.Metrics",
                c => new
                    {
                        MetricId = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.MetricId);
            
            CreateTable(
                "dbo.GroupUsers",
                c => new
                    {
                        GroupUserId = c.Int(nullable: false, identity: true),
                        GroupId = c.Int(nullable: false),
                        UserId = c.String(nullable: false),
                        Admin = c.Boolean(nullable: false),
                        AddedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GroupUserId);
            
            CreateTable(
                "dbo.FollowUsers",
                c => new
                    {
                        FollowUserId = c.Int(nullable: false, identity: true),
                        ToUserId = c.String(maxLength: 128),
                        FromUserId = c.String(maxLength: 128),
                        Accepted = c.Boolean(nullable: false),
                        AddedDate = c.DateTime(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        ApplicationUser_Id1 = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.FollowUserId);
            
            CreateTable(
                "dbo.GroupRequests",
                c => new
                    {
                        GroupRequestId = c.Int(nullable: false, identity: true),
                        GroupId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Accepted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.GroupRequestId);
            
            CreateTable(
                "dbo.GroupInvitations",
                c => new
                    {
                        GroupInvitationId = c.Int(nullable: false, identity: true),
                        FromUserId = c.String(),
                        GroupId = c.Int(nullable: false),
                        ToUserId = c.String(),
                        SentDate = c.DateTime(nullable: false),
                        Accepted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.GroupInvitationId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        GroupName = c.String(maxLength: 50),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GroupId);
            
            CreateTable(
                "dbo.Foci",
                c => new
                    {
                        FocusId = c.Int(nullable: false, identity: true),
                        FocusName = c.String(maxLength: 50),
                        Description = c.String(maxLength: 100),
                        GroupId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.FocusId);
            
            CreateTable(
                "dbo.GroupGoals",
                c => new
                    {
                        GroupGoalId = c.Int(nullable: false, identity: true),
                        GoalName = c.String(maxLength: 50),
                        Description = c.String(maxLength: 100),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Target = c.Double(),
                        MetricId = c.Int(),
                        FocusId = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        GoalStatusId = c.Int(nullable: false),
                        GroupUserId = c.Int(nullable: false),
                        AssignedGroupUserId = c.Int(),
                        AssignedTo = c.String(),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GroupGoalId);
            
            CreateTable(
                "dbo.GoalStatus",
                c => new
                    {
                        GoalStatusId = c.Int(nullable: false, identity: true),
                        GoalStatusType = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.GoalStatusId);
            
            CreateTable(
                "dbo.Goals",
                c => new
                    {
                        GoalId = c.Int(nullable: false, identity: true),
                        GoalName = c.String(nullable: false, maxLength: 55),
                        Desc = c.String(maxLength: 100),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Target = c.Double(),
                        GoalType = c.Boolean(nullable: false),
                        MetricId = c.Int(),
                        GoalStatusId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GoalId);
            
            CreateTable(
                "dbo.Updates",
                c => new
                    {
                        UpdateId = c.Int(nullable: false, identity: true),
                        Updatemsg = c.String(),
                        status = c.Double(),
                        GoalId = c.Int(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UpdateId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        CommentText = c.String(maxLength: 250),
                        UpdateId = c.Int(nullable: false),
                        CommentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId);
            
            CreateIndex("dbo.Smarts", "EquipmentId");
            CreateIndex("dbo.GroupUpdateSupports", "GroupUpdateId");
            CreateIndex("dbo.UpdateSupports", "UpdateId");
            CreateIndex("dbo.Supports", "GoalId");
            CreateIndex("dbo.SupportInvitations", "GoalId");
            CreateIndex("dbo.GroupComments", "GroupUpdateId");
            CreateIndex("dbo.GroupUpdates", "GroupGoalId");
            CreateIndex("dbo.FollowUsers", "ApplicationUser_Id1");
            CreateIndex("dbo.FollowUsers", "ApplicationUser_Id");
            CreateIndex("dbo.FollowUsers", "FromUserId");
            CreateIndex("dbo.FollowUsers", "ToUserId");
            CreateIndex("dbo.GroupRequests", "UserId");
            CreateIndex("dbo.GroupRequests", "GroupId");
            CreateIndex("dbo.GroupInvitations", "GroupId");
            CreateIndex("dbo.Foci", "GroupId");
            CreateIndex("dbo.GroupGoals", "GroupId");
            CreateIndex("dbo.GroupGoals", "GroupUserId");
            CreateIndex("dbo.GroupGoals", "GoalStatusId");
            CreateIndex("dbo.GroupGoals", "FocusId");
            CreateIndex("dbo.GroupGoals", "MetricId");
            CreateIndex("dbo.Goals", "UserId");
            CreateIndex("dbo.Goals", "GoalStatusId");
            CreateIndex("dbo.Goals", "MetricId");
            CreateIndex("dbo.Updates", "GoalId");
            CreateIndex("dbo.Comments", "UpdateId");
            AddForeignKey("dbo.Smarts", "EquipmentId", "dbo.Equipments", "EquipmentId");
            AddForeignKey("dbo.GroupUpdateSupports", "GroupUpdateId", "dbo.GroupUpdates", "GroupUpdateId", cascadeDelete: true);
            AddForeignKey("dbo.UpdateSupports", "UpdateId", "dbo.Updates", "UpdateId", cascadeDelete: true);
            AddForeignKey("dbo.Updates", "GoalId", "dbo.Goals", "GoalId", cascadeDelete: true);
            AddForeignKey("dbo.Supports", "GoalId", "dbo.Goals", "GoalId", cascadeDelete: true);
            AddForeignKey("dbo.SupportInvitations", "GoalId", "dbo.Goals", "GoalId", cascadeDelete: true);
            AddForeignKey("dbo.GroupUpdates", "GroupGoalId", "dbo.GroupGoals", "GroupGoalId", cascadeDelete: true);
            AddForeignKey("dbo.GroupComments", "GroupUpdateId", "dbo.GroupUpdates", "GroupUpdateId", cascadeDelete: true);
            AddForeignKey("dbo.GroupGoals", "MetricId", "dbo.Metrics", "MetricId");
            AddForeignKey("dbo.Goals", "MetricId", "dbo.Metrics", "MetricId");
            AddForeignKey("dbo.GroupGoals", "GroupUserId", "dbo.GroupUsers", "GroupUserId", cascadeDelete: true);
            AddForeignKey("dbo.GroupGoals", "GroupId", "dbo.Groups", "GroupId", cascadeDelete: true);
            AddForeignKey("dbo.GroupGoals", "GoalStatusId", "dbo.GoalStatus", "GoalStatusId", cascadeDelete: true);
            AddForeignKey("dbo.GroupGoals", "FocusId", "dbo.Foci", "FocusId");
            AddForeignKey("dbo.GroupRequests", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Goals", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.FollowUsers", "ApplicationUser_Id1", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.FollowUsers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.FollowUsers", "ToUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.FollowUsers", "FromUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.GroupRequests", "GroupId", "dbo.Groups", "GroupId", cascadeDelete: true);
            AddForeignKey("dbo.GroupInvitations", "GroupId", "dbo.Groups", "GroupId", cascadeDelete: true);
            AddForeignKey("dbo.Foci", "GroupId", "dbo.Groups", "GroupId", cascadeDelete: true);
            AddForeignKey("dbo.Goals", "GoalStatusId", "dbo.GoalStatus", "GoalStatusId", cascadeDelete: true);
            AddForeignKey("dbo.Comments", "UpdateId", "dbo.Updates", "UpdateId", cascadeDelete: true);
        }
    }
}
