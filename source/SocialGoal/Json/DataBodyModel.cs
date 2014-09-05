using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolsManage.Model
{
    public class DataBodyModel
    {
        public int Id { get; set; }
        //信息体类型标志
        public string InfoTypeNumber { get; set; }
        //开始位置
        public int StartPosition { get; set; }
        //数据长度
        public int DataLength { get; set; }
        //数据类型 1 byte,2 bit
        public int DataType { get; set; }

        public virtual DataByteModel DataByteModel { get; set; }
        public virtual List<DataBitModel> DataBitModels { get; set; }
    }
}
