using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Core.Common
{
    public class ZtreeEntity
    {
        public ZtreeEntity()
        {
            open = true;
        }
        public string name { get; set; }
        public string id { get; set; }
        public bool isParent { get; set; }
        public bool open { get; set; }
        public string icon { get; set; }
        public string PID { get; set; }
        public string title { get; set; }
        //public bool nocheck { get; set; }
        public List<ZtreeEntity> children { get; set; }


    }
}
