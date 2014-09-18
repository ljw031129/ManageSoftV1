using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolsManage.Model
{
    public class DataBitModel
    {
        public int Id { get; set; }
        //处理方式 16进制、ASCII IsUnsigned 
        public int Representation { get; set; }
        //字节总数
        public int ByteCounts { get; set; }
        //是否为大端模式
        public bool IsBigEndian { get; set; }
        //bit类型，bool（布尔）,state(状态)
        public int BitType { get; set; }
        //Bit位开始位置
        public int ChildStartPostion { get; set; }
        //Bit位长度

        public int ChildDataLength { get; set; }
        //默认值
        public string DefaultValue { get; set; }
        public string DictionaryKey { get; set; }
    }
}
