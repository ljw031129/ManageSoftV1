using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolsManage.Model
{
    public class FInterpretersModel
    {
        public FInterpretersModel()
        {
            InfoSignLength = 1;
        }
        //协议版本号
        public string ProtocolVersion { get; set; }
        //解析方式
        public int AnalysisWay { get; set; }
        //信息体个数所在位置
        public int InfoCountsPostion { get; set; }
        //信息体标志开始位置
        public int InfoSignStartPosition { get; set; }
        //信息体标志所占长度
        public int InfoSignLength { get; set; }
        //信息体个数
       // public int InfoCounts { get; set; }
        public virtual List<DataBodyModel> DataBodyModels { get; set; }
        public virtual List<SpeciaCalculationModel> SpeciaCalculationModels { get; set; }


    }
}
