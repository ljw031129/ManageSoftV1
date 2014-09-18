using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialGoal.Model.Models
{
    public class PmDataBody
    {
        public string PmDataBodyId { get; set; }
        //信息体类型标志
        public string InfoTypeNumber { get; set; }
        //开始位置
        public int StartPosition { get; set; }
        //数据长度
        public int DataLength { get; set; }
        //数据类型 1 byte,2 bit
        public int DataType { get; set; }
        public int OrderIndex { get; set; }

        public string PmFInterpreterId { get; set; }
        public string PmDataByteId { get; set; }

        public virtual PmDataByte PmDataByte { get; set; }
        public virtual ICollection<PmDataBit> PmDataBits { get; set; }
    }
}
