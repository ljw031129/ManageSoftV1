namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        CommentText = c.String(maxLength: 250),
                        UpdateId = c.Int(nullable: false),
                        CommentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Updates", t => t.UpdateId, cascadeDelete: true)
                .Index(t => t.UpdateId);
            
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
                .PrimaryKey(t => t.UpdateId)
                .ForeignKey("dbo.Goals", t => t.GoalId, cascadeDelete: true)
                .Index(t => t.GoalId);
            
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
                .PrimaryKey(t => t.GoalId)
                .ForeignKey("dbo.GoalStatus", t => t.GoalStatusId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Metrics", t => t.MetricId)
                .Index(t => t.GoalStatusId)
                .Index(t => t.UserId)
                .Index(t => t.MetricId);
            
            CreateTable(
                "dbo.GoalStatus",
                c => new
                    {
                        GoalStatusId = c.Int(nullable: false, identity: true),
                        GoalStatusType = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.GoalStatusId);
            
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
                .PrimaryKey(t => t.GroupGoalId)
                .ForeignKey("dbo.Foci", t => t.FocusId)
                .ForeignKey("dbo.GoalStatus", t => t.GoalStatusId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.GroupUsers", t => t.GroupUserId, cascadeDelete: true)
                .ForeignKey("dbo.Metrics", t => t.MetricId)
                .Index(t => t.FocusId)
                .Index(t => t.GoalStatusId)
                .Index(t => t.GroupId)
                .Index(t => t.GroupUserId)
                .Index(t => t.MetricId);
            
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
                .PrimaryKey(t => t.FocusId)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
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
                .PrimaryKey(t => t.GroupInvitationId)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.GroupRequests",
                c => new
                    {
                        GroupRequestId = c.Int(nullable: false, identity: true),
                        GroupId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Accepted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.GroupRequestId)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.GroupId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        Email = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ProfilePicUrl = c.String(),
                        DateCreated = c.DateTime(),
                        LastLoginTime = c.DateTime(),
                        Activated = c.Boolean(),
                        RoleId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.FollowUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.FromUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.ToUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id1)
                .Index(t => t.FromUserId)
                .Index(t => t.ToUserId)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id1);
            
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
                "dbo.Metrics",
                c => new
                    {
                        MetricId = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.MetricId);
            
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
                .PrimaryKey(t => t.GroupUpdateId)
                .ForeignKey("dbo.GroupGoals", t => t.GroupGoalId, cascadeDelete: true)
                .Index(t => t.GroupGoalId);
            
            CreateTable(
                "dbo.GroupComments",
                c => new
                    {
                        GroupCommentId = c.Int(nullable: false, identity: true),
                        CommentText = c.String(),
                        GroupUpdateId = c.Int(nullable: false),
                        CommentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GroupCommentId)
                .ForeignKey("dbo.GroupUpdates", t => t.GroupUpdateId, cascadeDelete: true)
                .Index(t => t.GroupUpdateId);
            
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
                .PrimaryKey(t => t.SupportInvitationId)
                .ForeignKey("dbo.Goals", t => t.GoalId, cascadeDelete: true)
                .Index(t => t.GoalId);
            
            CreateTable(
                "dbo.Supports",
                c => new
                    {
                        SupportId = c.Int(nullable: false, identity: true),
                        GoalId = c.Int(nullable: false),
                        UserId = c.String(),
                        SupportedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SupportId)
                .ForeignKey("dbo.Goals", t => t.GoalId, cascadeDelete: true)
                .Index(t => t.GoalId);
            
            CreateTable(
                "dbo.UpdateSupports",
                c => new
                    {
                        UpdateSupportId = c.Int(nullable: false, identity: true),
                        UpdateId = c.Int(nullable: false),
                        UserId = c.String(),
                        UpdateSupportedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UpdateSupportId)
                .ForeignKey("dbo.Updates", t => t.UpdateId, cascadeDelete: true)
                .Index(t => t.UpdateId);
            
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
                "dbo.GroupCommentUsers",
                c => new
                    {
                        GroupCommentUserId = c.Int(nullable: false, identity: true),
                        GroupCommentId = c.Int(nullable: false),
                        UserId = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.GroupCommentUserId);
            
            CreateTable(
                "dbo.GroupUpdateSupports",
                c => new
                    {
                        GroupUpdateSupportId = c.Int(nullable: false, identity: true),
                        GroupUpdateId = c.Int(nullable: false),
                        GroupUserId = c.Int(nullable: false),
                        UpdateSupportedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GroupUpdateSupportId)
                .ForeignKey("dbo.GroupUpdates", t => t.GroupUpdateId, cascadeDelete: true)
                .Index(t => t.GroupUpdateId);
            
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
                "dbo.SecurityTokens",
                c => new
                    {
                        SecurityTokenId = c.Int(nullable: false, identity: true),
                        Token = c.Guid(nullable: false),
                        ActualID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SecurityTokenId);
            
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        UserProfileId = c.Int(nullable: false, identity: true),
                        DateEdited = c.DateTime(nullable: false),
                        Email = c.String(),
                        FirstName = c.String(maxLength: 100),
                        LastName = c.String(),
                        DateOfBirth = c.DateTime(),
                        Gender = c.Boolean(),
                        Address = c.String(),
                        City = c.String(maxLength: 100),
                        State = c.String(maxLength: 50),
                        Country = c.String(maxLength: 50),
                        ZipCode = c.Double(),
                        ContactNo = c.Double(),
                        UserId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.UserProfileId);
            
            CreateTable(
                "dbo.RegistrationTokens",
                c => new
                    {
                        RegistrationTokenId = c.Int(nullable: false, identity: true),
                        Token = c.Guid(nullable: false),
                        Role = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.RegistrationTokenId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GroupUpdateSupports", "GroupUpdateId", "dbo.GroupUpdates");
            DropForeignKey("dbo.UpdateSupports", "UpdateId", "dbo.Updates");
            DropForeignKey("dbo.Updates", "GoalId", "dbo.Goals");
            DropForeignKey("dbo.Supports", "GoalId", "dbo.Goals");
            DropForeignKey("dbo.SupportInvitations", "GoalId", "dbo.Goals");
            DropForeignKey("dbo.GroupUpdates", "GroupGoalId", "dbo.GroupGoals");
            DropForeignKey("dbo.GroupComments", "GroupUpdateId", "dbo.GroupUpdates");
            DropForeignKey("dbo.GroupGoals", "MetricId", "dbo.Metrics");
            DropForeignKey("dbo.Goals", "MetricId", "dbo.Metrics");
            DropForeignKey("dbo.GroupGoals", "GroupUserId", "dbo.GroupUsers");
            DropForeignKey("dbo.GroupGoals", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.GroupGoals", "GoalStatusId", "dbo.GoalStatus");
            DropForeignKey("dbo.GroupGoals", "FocusId", "dbo.Foci");
            DropForeignKey("dbo.GroupRequests", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Goals", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FollowUsers", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.FollowUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.FollowUsers", "ToUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FollowUsers", "FromUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.GroupRequests", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.GroupInvitations", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Foci", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Goals", "GoalStatusId", "dbo.GoalStatus");
            DropForeignKey("dbo.Comments", "UpdateId", "dbo.Updates");
            DropIndex("dbo.GroupUpdateSupports", new[] { "GroupUpdateId" });
            DropIndex("dbo.UpdateSupports", new[] { "UpdateId" });
            DropIndex("dbo.Updates", new[] { "GoalId" });
            DropIndex("dbo.Supports", new[] { "GoalId" });
            DropIndex("dbo.SupportInvitations", new[] { "GoalId" });
            DropIndex("dbo.GroupUpdates", new[] { "GroupGoalId" });
            DropIndex("dbo.GroupComments", new[] { "GroupUpdateId" });
            DropIndex("dbo.GroupGoals", new[] { "MetricId" });
            DropIndex("dbo.Goals", new[] { "MetricId" });
            DropIndex("dbo.GroupGoals", new[] { "GroupUserId" });
            DropIndex("dbo.GroupGoals", new[] { "GroupId" });
            DropIndex("dbo.GroupGoals", new[] { "GoalStatusId" });
            DropIndex("dbo.GroupGoals", new[] { "FocusId" });
            DropIndex("dbo.GroupRequests", new[] { "UserId" });
            DropIndex("dbo.Goals", new[] { "UserId" });
            DropIndex("dbo.FollowUsers", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.FollowUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.FollowUsers", new[] { "ToUserId" });
            DropIndex("dbo.FollowUsers", new[] { "FromUserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.GroupRequests", new[] { "GroupId" });
            DropIndex("dbo.GroupInvitations", new[] { "GroupId" });
            DropIndex("dbo.Foci", new[] { "GroupId" });
            DropIndex("dbo.Goals", new[] { "GoalStatusId" });
            DropIndex("dbo.Comments", new[] { "UpdateId" });
            DropTable("dbo.RegistrationTokens");
            DropTable("dbo.UserProfiles");
            DropTable("dbo.SecurityTokens");
            DropTable("dbo.GroupUpdateUsers");
            DropTable("dbo.GroupUpdateSupports");
            DropTable("dbo.GroupCommentUsers");
            DropTable("dbo.FollowRequests");
            DropTable("dbo.CommentUsers");
            DropTable("dbo.UpdateSupports");
            DropTable("dbo.Supports");
            DropTable("dbo.SupportInvitations");
            DropTable("dbo.GroupComments");
            DropTable("dbo.GroupUpdates");
            DropTable("dbo.Metrics");
            DropTable("dbo.GroupUsers");
            DropTable("dbo.FollowUsers");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.GroupRequests");
            DropTable("dbo.GroupInvitations");
            DropTable("dbo.Groups");
            DropTable("dbo.Foci");
            DropTable("dbo.GroupGoals");
            DropTable("dbo.GoalStatus");
            DropTable("dbo.Goals");
            DropTable("dbo.Updates");
            DropTable("dbo.Comments");
        }
    }
}
