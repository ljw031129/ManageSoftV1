using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Model.ViewModels
{ 

    public class OrgStructureViewModel
    {      
        public string OrgStructureId { get; set; }
        public string OrgStructurePId { get; set; }
        [Required]
        [Display(Name = "组织编号")]
        public string OrgStructureNum { get; set; }
        [Required]
        [Display(Name = "组织名称")]
        public string OrgStructureName { get; set; }
        [Required]
        [Display(Name = "组织描述")]
        public string OrgStructureDescribe { get; set; }
        public DateTime OrgStructureCreateTime { get; set; }
        public DateTime OrgStructureUpdateTime { get; set; }
        public string OrgEnterpriseId { get; set; }
        public string level { get; set; }
        public string parent { get; set; }
        public bool isLeaf { get; set; }
        public bool expanded { get; set; }
        public bool loaded { get; set; }
        public string icon { get; set; }

    }
}
