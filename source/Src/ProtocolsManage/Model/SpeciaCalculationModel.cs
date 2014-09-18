using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolsManage.Model
{
    public class SpeciaCalculationModel
    {
        //引用KEY名称
        public String SrcDictionaryKey { get; set; }
        //计算公式
        public string Formula { get; set; }
        //依据计算公式生成的目标KEY名称
        public String TargetDictionaryKey { get; set; }
    }
}
