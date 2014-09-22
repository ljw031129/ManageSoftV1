using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using SocialGoal.Model.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using SocialGoal.Data.Configuration;
using System.Reflection;

namespace SocialGoal.Data.Models
{
    public class SocialGoalEntities : IdentityDbContext<ApplicationUser>
    {
        //, throwIfV1Schema: false
        public SocialGoalEntities()
            : base("SocialGoalEntities")
        {
        }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Focus> Focuses { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Update> Updates { get; set; }
        // public DbSet<User> Users { get; set; }
        public DbSet<Metric> Metrics { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<SecurityToken> SecurityTokens { get; set; }
        public DbSet<Support> Support { get; set; }
        public DbSet<GroupInvitation> GroupInvitation { get; set; }
        public DbSet<SupportInvitation> SupportInvitation { get; set; }
        public DbSet<GroupGoal> GroupGoal { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<GoalStatus> GoalStatus { get; set; }
        public DbSet<GroupUpdate> GroupUpdate { get; set; }
        public DbSet<GroupComment> GroupComment { get; set; }
        public DbSet<GroupRequest> GroupRequests { get; set; }
        public DbSet<FollowRequest> FollowRequest { get; set; }
        public DbSet<FollowUser> FollowUser { get; set; }
        public DbSet<GroupUpdateUser> GroupUpdateUsers { get; set; }
        public DbSet<GroupCommentUser> GroupCommentUsers { get; set; }
        public DbSet<CommentUser> CommentUsers { get; set; }
        public DbSet<UpdateSupport> UpdateSupports { get; set; }
        public DbSet<GroupUpdateSupport> GroupUpdateSupports { get; set; }

        //测试数据
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Smart> Smarts { get; set; }

        //协议管理部分
        public DbSet<PmFInterpreter> PmFInterpreters { get; set; }
        public DbSet<PmSpeciaCalculation> PmSpeciaCalculations { get; set; }
        public DbSet<PmDataByte> PmDataBytes { get; set; }
        public DbSet<PmDataBody> PmDataBodys { get; set; }
        public DbSet<PmDataBit> PmDataBits { get; set; }
        //回传数据部分
        public DbSet<ReceiveDataLast> ReceiveDataLasts { get; set; }
        public DbSet<ReceiveData> ReceiveDatas { get; set; }

        //数据显示配置
        public DbSet<ReceiveDataDisplay> ReceiveDataDisplays { get; set; }
        public DbSet<ReDataDisplayFormat> ReDataDisplayFormats { get; set; }

        //组织机构信息
        public DbSet<OrgEnterprise> OrgEnterprises { get; set; }
        public DbSet<OrgStructure> OrgStructures { get; set; }

        //终端管理
        public DbSet<TerminalSimCard> TerminalSimCards { get; set; }
        public DbSet<TerminalEquipment> TerminalEquipments { get; set; }

        public virtual void Commit()
        {
            base.SaveChanges();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            // modelBuilder.Conventions.Remove<IncludeMetadataConvention>();

            modelBuilder.Configurations.Add(new CommentConfiguration());
            modelBuilder.Configurations.Add(new CommentUserConfiguration());
            modelBuilder.Configurations.Add(new FocusConfiguration());
            modelBuilder.Configurations.Add(new FollowRequestConfiguration());
            modelBuilder.Configurations.Add(new FollowUserConfiguration());
            modelBuilder.Configurations.Add(new GoalConfiguration());
            modelBuilder.Configurations.Add(new GoalStatusConfiguration());
            modelBuilder.Configurations.Add(new GroupCommentConfiguration());
            modelBuilder.Configurations.Add(new GroupCommentUserConfguration());
            modelBuilder.Configurations.Add(new GroupConfiguration());
            modelBuilder.Configurations.Add(new GroupGoalConfiguration());
            modelBuilder.Configurations.Add(new GroupInvitationConfiguration());
            modelBuilder.Configurations.Add(new GroupRequestConfiguration());
            modelBuilder.Configurations.Add(new GroupUpdateSupportConfiguration());
            modelBuilder.Configurations.Add(new GroupUpdateUserConfiguration());
            modelBuilder.Configurations.Add(new GroupUserConfiguration());
            modelBuilder.Configurations.Add(new MetricConfiguration());
            modelBuilder.Configurations.Add(new RegistrationTokenConfiguration());
            modelBuilder.Configurations.Add(new SupportConfiguration());
            modelBuilder.Configurations.Add(new SupportInvitationConfiguration());
            modelBuilder.Configurations.Add(new UpdateConfiguration());
            modelBuilder.Configurations.Add(new UpdateSupportConfiguration());
            modelBuilder.Configurations.Add(new UserProfileConfiguration());

            modelBuilder.Configurations.Add(new EquipmentConfiguration());
            modelBuilder.Configurations.Add(new SmartConfiguration());

            //协议管理
            modelBuilder.Configurations.Add(new PmDataBitConfiguration());
            modelBuilder.Configurations.Add(new PmDataBodyConfiguration());
            modelBuilder.Configurations.Add(new PmDataByteConfiguration());
            modelBuilder.Configurations.Add(new PmFInterpreterConfiguration());
            modelBuilder.Configurations.Add(new PmSpeciaCalculationConfiguration());

            //数据部分
            modelBuilder.Configurations.Add(new ReceiveDataConfiguration());
            modelBuilder.Configurations.Add(new ReceiveDataLastConfiguration());
            //数据显示内容设置
            modelBuilder.Configurations.Add(new ReDataDisplayFormatConfiguration());
            modelBuilder.Configurations.Add(new ReceiveDataDisplayConfiguration());
            //组织机构信息
            modelBuilder.Configurations.Add(new OrgEnterpriseConfiguration());
            modelBuilder.Configurations.Add(new OrgStructureConfiguration());
            //终端管理
            modelBuilder.Configurations.Add(new TerminalSimCardConfiguration());
            modelBuilder.Configurations.Add(new TerminalEquipmentConfiguration());
        }
    }
}