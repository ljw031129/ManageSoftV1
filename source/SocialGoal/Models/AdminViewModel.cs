using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SocialGoal.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }
        //[Required]
        [EmailAddress]
        [Display(Name = "邮箱账号")]
        public string Email { get; set; }
        public string OrgEnterpriseId { get; set; }
        public string OrgEnterpriseName { get; set; }
        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}