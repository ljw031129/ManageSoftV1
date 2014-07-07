using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Model.Models
{
    public class Smart
    {
        public string SmartId { get; set; }
        public string  SmartNum { get; set; }
        public string SmartSim { get; set; }
        public DateTime SmartCreatTime { get; set; }
        public DateTime SmartUpdateTime { get; set; }

        public string EquipmentId { get; set; }

        public virtual Equipment Equipment { get; set; }

        public Smart()
        {
            SmartCreatTime = DateTime.Now;
            SmartId = Guid.NewGuid().ToString();
        }
    }
}
