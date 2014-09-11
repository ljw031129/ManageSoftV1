using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Model.Models
{
    public class ReceiveDataDispaly
    {
        public string ReceiveDataDispalyId { get; set; }
        //存储字段
        public string DictionaryKey { get; set; }
        //显示存储字段名称
        public string DefaultValue { get; set; }
        public string ShowColor { get; set; }
        public string ChartRange { get; set; }
        //input button select chart
        public string ShowType { get; set; }
        public string ShowIcon { get; set; }
        //主页 页块中
        public string ShowPostion { get; set; }


    }
}
