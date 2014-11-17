using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Model.Models
{
    public class OrgStructure
    {
        public OrgStructure()
        {
            OrgStructureCreateTime = DateTime.Now;
            OrgStructureUpdateTime = DateTime.Now;
        }
        public string OrgStructureId { get; set; }
        public string OrgStructurePId { get; set; }
        public string OrgStructureNum { get; set; }
        public string OrgStructureName { get; set; }
        public string OrgStructureDescribe { get; set; }
        public DateTime OrgStructureCreateTime { get; set; }
        public DateTime OrgStructureUpdateTime { get; set; }
       // public string OrgEnterpriseId { get; set; }
        //public string level { get; set; }
        //public string parent { get; set; }
        //public bool isLeaf { get; set; }
        //public bool expanded { get; set; }
        //public bool loaded { get; set; }
        //public string icon { get; set; }

        public virtual ICollection<TerminalEquipment> TerminalEquipments { get; set; }
    }
}
