using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Model.Models
{
    public class ReceiveDataHistory
    {
        public string ReceiveDataHistoryId { get; set; }
        public string IMEI { get; set; }
        public string DataStr { get; set; }
        public DateTime ReceiveTime { get; set; }
    }
}
