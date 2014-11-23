using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SocialGoal.Model.ViewModels
{
    public class OrgEnterpriseViewModel
    {
        public string OrgEnterpriseId { get; set; }
        public string OrgEnterprisePId { get; set; }

        [DisplayName("企业编号")]
        public string OrgEnterpriseNum { get; set; }
         [DisplayName("企业名称")]
        public string OrgEnterpriseName { get; set; }
         [DisplayName("备注信息")]
        public string OrgEnterpriseDescribe { get; set; }
          [DisplayName("企业创建时间")]
         public string OrgEnterpriseCreateTime { get; set; }
          [DisplayName("信息更新时间")]
          public string OrgEnterpriseUpdateTime { get; set; }
        //del  edit  add
        public string oper { get; set; }
        public string id { get; set; }
    }
}