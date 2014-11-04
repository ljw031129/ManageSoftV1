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
        //显示名称
        public string DictionaryValue { get; set; }
        //排序
        public int ShowOrder { get; set; }
        //显示单位
        public string ShowUnit { get; set; }
        //显示图标
        public string ShowIcon { get; set; }
        //主页 页块中
        public string ShowPostion { get; set; }
        //是否详细信息显示
        public bool ShowCommon { get; set; }
        //表格显示具体设置
        //对齐方式
        public string Alignment { get; set; }
        //显示数据类型
        public string Formatter { get; set; }
        //数据类型格式设置
        public string Formatoptions { get; set; }
        //显示宽度
        public string ShowWidth { get; set; }
        //是否表格显示
        public bool ShowTable { get; set; }

        public int FormatType { get; set; }

        public string PmFInterpreterId { get; set; }

        public virtual ICollection<ReDataDisplayFormat> ReDataDisplayFormats { get; set; }

    }
}
