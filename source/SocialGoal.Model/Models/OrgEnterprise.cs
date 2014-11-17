using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Model.Models
{
    public class OrgEnterprise
    {
        public OrgEnterprise()
        {
            OrgEnterpriseCreateTime = DateTime.Now;
            OrgEnterpriseUpdateTime = DateTime.Now;
        }
        public string OrgEnterpriseId { get; set; }
        public string OrgEnterprisePId { get; set; }
        public string OrgEnterpriseNum { get; set; }
        public string OrgEnterpriseName { get; set; }
        public string OrgEnterpriseDescribe { get; set; }
        public DateTime OrgEnterpriseCreateTime { get; set; }
        public DateTime OrgEnterpriseUpdateTime { get; set; }
      //  public virtual ICollection<OrgStructure> OrgStructures { get; set; }
    }
}
