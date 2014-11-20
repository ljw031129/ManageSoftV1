using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using SocialGoal.Model.Models;
using SocialGoal.Data.Models;

namespace SocialGoal.Data
{
    public class GoalsSampleData : DropCreateDatabaseIfModelChanges<SocialGoalEntities>
    {
        protected override void Seed(SocialGoalEntities context)
        {           
            //new List<Metric>
            //{
            //    new Metric { Type ="%"},
            //    new Metric { Type ="$"},
            //    new Metric { Type ="$ M"},
            //    new Metric { Type ="Rs"},
            //    new Metric { Type ="Hours"},
            //    new Metric { Type ="Km"},
            //    new Metric { Type ="Kg"},
            //    new Metric { Type ="Years"}

            //}.ForEach(m => context.Metrics.Add(m));

            //new List<GoalStatus>
            //{
            //    new GoalStatus{GoalStatusType="In Progress"},
            //    new GoalStatus{GoalStatusType="On Hold"},
            //    new GoalStatus{GoalStatusType="Completed"}
            //}.ForEach(m => context.GoalStatus.Add(m));

            //context.Commit();
            InitializeIdentityForEF(context);
            base.Seed(context);
        }
        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public static void InitializeIdentityForEF(SocialGoalEntities db)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            const string name = "340704008@qq.com";
            const string password = "Admin@123456";
            const string roleName = "Admin";

            //Create Role Admin if it does not exist
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new IdentityRole(roleName);
                var roleresult = roleManager.Create(role);
            }

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }
        }

    }
}