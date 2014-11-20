using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SocialGoal.Model.Models
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public ApplicationUser()
        {
            DateCreated = DateTime.Now;
        }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ProfilePicUrl { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public bool Activated { get; set; }

        public int RoleId { get; set; }
        //用户绑定的设备信息

        public virtual ICollection<TerminalEquipment> TerminalEquipment { get; set; }
        //企业信息
        public string OrgEnterpriseId { get; set; }
        public virtual OrgEnterprise OrgEnterprise { get; set; }

        public string DisplayName
        {
            get { return FirstName + " " + LastName; }
        }


    }
}
