using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialGoal.Model.Models
{
    public class PmSpeciaCalculation
    {
        public string PmSpeciaCalculationId { get; set; }
        //引用KEY名称
        public String SrcDictionaryKey { get; set; }
        //计算公式
        public string Formula { get; set; }
        //依据计算公式生成的目标KEY名称
        public String TargetDictionaryKey { get; set; }
    }
}
