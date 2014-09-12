using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Model.Models
{
    public class ReceiveDataDisplay
    {
        public string ReceiveDataDisplayId { get; set; }
        //存储字段
        public string DictionaryKey { get; set; }
        //排序
        public int ShowOrder { get; set; }
        public string ShowUnit { get; set; }
        //input button select chart
        public string ShowType { get; set; }
        public string ShowIcon { get; set; }
        //主页 页块中
        public string ShowPostion { get; set; }
        public bool ShowCommon { get; set; }

        public string PmFInterpreterId { get; set; }


        public virtual ICollection<ReDataDisplayFormat> ReDataDisplayFormats { get; set; }

    }
}
