﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialGoal.Model.Models
{
    public class PmDataByte
    {
        public PmDataByte()
        {
            Representation = 1;
        }
        public string PmDataByteId { get; set; }
        //处理方式 16进制、ASCII IsUnsigned 
        public int Representation { get; set; }
        //字节总数
        // public int ByteCounts { get; set; }
        //是否为大端模式
        public bool IsBigEndian { get; set; }

        //是否为有符号整数
        public bool IsUnsigned { get; set; }
        //计算公式
        public string Formula { get; set; }
        //默认值
        public string DefaultValue { get; set; }
        public string DictionaryKey { get; set; }
    }
}
