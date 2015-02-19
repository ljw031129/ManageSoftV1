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
            : base("SocialGoalEntities", throwIfV1Schema: false)
        {
        }
        static SocialGoalEntities()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<SocialGoalEntities>(new GoalsSampleData());
        }
        public static SocialGoalEntities Create()
        {
            return new SocialGoalEntities();
        }
        public DbSet<SecurityToken> SecurityTokens { get; set; }

        public DbSet<UserProfile> UserProfile { get; set; }


        //测试数据
        public DbSet<Equipment> Equipments { get; set; }


        //协议管理部分
        public DbSet<PmFInterpreter> PmFInterpreters { get; set; }
        public DbSet<PmSpeciaCalculation> PmSpeciaCalculations { get; set; }
        public DbSet<PmDataByte> PmDataBytes { get; set; }
        public DbSet<PmDataBody> PmDataBodys { get; set; }
        public DbSet<PmDataBit> PmDataBits { get; set; }
        //回传数据部分
        public DbSet<ReceiveDataLast> ReceiveDataLasts { get; set; }
        public DbSet<ReceiveData> ReceiveDatas { get; set; }
        public DbSet<ReceiveDataHistory> ReceiveDataHistorys { get; set; }

        //数据显示配置
        public DbSet<ReceiveDataDisplay> ReceiveDataDisplays { get; set; }
        public DbSet<ReDataDisplayFormat> ReDataDisplayFormats { get; set; }

        //组织机构信息
        public DbSet<OrgEnterprise> OrgEnterprises { get; set; }
        public DbSet<OrgStructure> OrgStructures { get; set; }

        //终端管理
        public DbSet<TerminalSimCard> TerminalSimCards { get; set; }
        public DbSet<TerminalEquipment> TerminalEquipments { get; set; }
        public DbSet<TerminalEquipmentCommand> TerminalEquipmentCommands { get; set; }
        public DbSet<TerminalEquipmentCommandCurrent> TerminalEquipmentCommandCurrents { get; set; }

        public virtual void Commit()
        {
            //异步保存
            //base.SaveChangesAsync();
            base.SaveChanges();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            // modelBuilder.Conventions.Remove<IncludeMetadataConvention>();


            modelBuilder.Configurations.Add(new RegistrationTokenConfiguration());
            modelBuilder.Configurations.Add(new UserProfileConfiguration());
            modelBuilder.Configurations.Add(new EquipmentConfiguration());


            //协议管理
            modelBuilder.Configurations.Add(new PmDataBitConfiguration());
            modelBuilder.Configurations.Add(new PmDataBodyConfiguration());
            modelBuilder.Configurations.Add(new PmDataByteConfiguration());
            modelBuilder.Configurations.Add(new PmFInterpreterConfiguration());
            modelBuilder.Configurations.Add(new PmSpeciaCalculationConfiguration());

            //数据部分
            modelBuilder.Configurations.Add(new ReceiveDataConfiguration());
            modelBuilder.Configurations.Add(new ReceiveDataLastConfiguration());
            modelBuilder.Configurations.Add(new ReceiveDataHistoryConfiguration());
            //数据显示内容设置
            modelBuilder.Configurations.Add(new ReDataDisplayFormatConfiguration());
            modelBuilder.Configurations.Add(new ReceiveDataDisplayConfiguration());
            //组织机构信息
            modelBuilder.Configurations.Add(new OrgEnterpriseConfiguration());
            modelBuilder.Configurations.Add(new OrgStructureConfiguration());
            //终端管理
            modelBuilder.Configurations.Add(new TerminalSimCardConfiguration());
            modelBuilder.Configurations.Add(new TerminalEquipmentConfiguration());
            modelBuilder.Configurations.Add(new TerminalEquipmentCommandConfiguration());
            modelBuilder.Configurations.Add(new TerminalEquipmentCommandCurrentConfiguration());
        }
    }
}