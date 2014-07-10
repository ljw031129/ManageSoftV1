using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialGoal.Web.ViewModels
{
    public class EquipmentViewModel
    {
        public string EquipmentId { get; set; }
        public string EquipmentNum { get; set; }
        public string EquipmentName { get; set; }
        public DateTime EquipmentCreatTime { get; set; }
        public DateTime EquipmentUpDateTime { get; set; }
        public EquipmentViewModel()
        {
         //EquipmentUpDateTime = DateTime.Now;
         //EquipmentCreatTime = DateTime.Now;
          //  EquipmentId = Guid.NewGuid().ToString();
        }
    }
}