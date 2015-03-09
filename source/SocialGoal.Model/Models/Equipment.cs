using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Model.Models
{
    public class Equipment
    {
        public string EquipmentId { get; set; }
        ///大驾号
        public string EquipmentNum { get; set; }
        //车牌号
        public string EquipmentName { get; set; }
        //发动机号
        public string EngineNum { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPhone { get; set; }
        public string OwnerAddress { get; set; }
        public string InstallTime { get; set; }
        public string InstallUser { get; set; }
        public string InstallUserPhone { get; set; }
        public string InstallPlace { get; set; }
        public string InstallSite { get; set; }

        public string EquipmentTypeId { get; set; }
        public DateTime EquipmentCreatTime { get; set; }
        public DateTime EquipmentUpDateTime { get; set; }

        public virtual ICollection<TerminalEquipment> TerminalEquipments { get; set; }
        //企业信息
        public string OrgEnterpriseId { get; set; }
        public virtual OrgEnterprise OrgEnterprise { get; set; }

        public Equipment()
        {
            EquipmentCreatTime = DateTime.Now;
            EquipmentId = Guid.NewGuid().ToString();
        }

    }
}
