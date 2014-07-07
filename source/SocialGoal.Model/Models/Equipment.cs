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
        public string EquipmentNum { get; set; }
        public string EquipmentName { get; set; }
        public DateTime EquipmentCreatTime { get; set; }
        public DateTime EquipmentUpDateTime { get; set; }
      //  public virtual ICollection<Smart> Smarts { get; set; }

        public Equipment()
        {
            EquipmentCreatTime = DateTime.Now;
            EquipmentId = Guid.NewGuid().ToString();
        }

    }
}
