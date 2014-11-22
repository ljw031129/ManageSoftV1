using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialGoal.Model.ViewModels
{
    public class OrgEnterpriseViewModel
    {
        public string OrgEnterpriseId { get; set; }
        public string OrgEnterprisePId { get; set; }
        public string OrgEnterpriseNum { get; set; }
        public string OrgEnterpriseName { get; set; }
        public string OrgEnterpriseDescribe { get; set; }
        public DateTime OrgEnterpriseCreateTime { get; set; }
        public DateTime OrgEnterpriseUpdateTime { get; set; }
        //del  edit  add
        public string oper { get; set; }
        public string id { get; set; }
    }
}