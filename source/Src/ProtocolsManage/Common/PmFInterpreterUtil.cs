using SocialGoal.Model.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolsManage.Common
{
    public class PmFInterpreterUtil
    {
        /// <summary>
        /// 按照协议解析方式对，上传数据进行分组
        /// </summary>
        /// <param name="fInterpretersModel">解析格式Json</param>
        /// <param name="bytestr">长传的字节数组</param>
        /// <returns></returns>
        public static Dictionary<string, byte[]> SplitData(PmFInterpreter fInterpretersModel, byte[] bytestr)
        {
            //按照协议对数据进行分组
            Dictionary<string, byte[]> dicBytes = new Dictionary<string, byte[]>();


            int nextInfoPostion = 0;
            string dicKey = "";

            int splitLength = 0;

            for (int i = 0; i < bytestr[fInterpretersModel.InfoCountsPostion]; i++)
            {
                if (nextInfoPostion == 0)
                {
                    dicKey = bytestr.CloneRange(fInterpretersModel.InfoSignStartPosition, fInterpretersModel.InfoSignLength)[0].ToString();

                    splitLength = bytestr.CloneRange(fInterpretersModel.InfoSignStartPosition + 1, 1)[0];

                    dicBytes.Add(dicKey, bytestr.CloneRange(fInterpretersModel.InfoSignStartPosition + 2, splitLength));
                    nextInfoPostion = fInterpretersModel.InfoSignStartPosition + 2 + splitLength;
                }
                else
                {
                    dicKey = bytestr.CloneRange(nextInfoPostion, fInterpretersModel.InfoSignLength)[0].ToString();

                    splitLength = bytestr.CloneRange(nextInfoPostion + 1, 1)[0];

                    dicBytes.Add(dicKey, bytestr.CloneRange(nextInfoPostion + 2, splitLength));
                    nextInfoPostion = nextInfoPostion + 2 + splitLength;
                }

            }

            return dicBytes;
        }
        /// <summary>
        /// byte类型字节数组目标类型Int类型数据
        /// </summary>
        /// <param name="dataBodyModel"></param>
        /// <param name="dicDatas"></param>
        /// <param name="childData"></param>
        public static void AnalysisByte(PmDataBody dataBodyModel, Dictionary<string, string> dicDatas, byte[] childData)
        {
            byte[] subChildData = childData.CloneRange(dataBodyModel.StartPosition, dataBodyModel.DataLength);
            //处理数据方式
            PmDataByte dataByteModel = dataBodyModel.PmDataByte;

            switch (dataByteModel.Representation)
            {
                case 1:
                    //16进制处理函数
                    if (dataByteModel.IsBigEndian)
                    {
                        subChildData = tranBigEndian(subChildData);
                    }
                    //是否为有符号
                    if (!dataByteModel.IsUnsigned)
                    {
                        long cData = ConvertBytesToInt(subChildData, 0, dataBodyModel.DataLength);

                        if (dataByteModel.Formula != "" && dataByteModel.Formula != null)
                        {
                            string reExp = EvaluateExpression11.ReplaceChar(dataByteModel.Formula, '$', cData.ToString());
                            double reEval = EvaluateExpression11.Calculate(reExp);
                            dicDatas.Add(dataByteModel.DictionaryKey, reEval.ToString());
                        }
                        else
                        {
                            dicDatas.Add(dataByteModel.DictionaryKey, cData.ToString());
                        }

                    }
                    else
                    {
                        ulong cData = ConvertBytesToUInt(subChildData, 0, dataBodyModel.DataLength);
                        if (dataByteModel.Formula != "" && dataByteModel.Formula != null)
                        {
                            string reExp = EvaluateExpression11.ReplaceChar(dataByteModel.Formula, '$', cData.ToString());
                            double reEval = EvaluateExpression11.Calculate(reExp);
                            dicDatas.Add(dataByteModel.DictionaryKey, reEval.ToString());
                        }
                        else
                        {
                            dicDatas.Add(dataByteModel.DictionaryKey, cData.ToString());
                        }
                    }

                    break;
                case 2:
                    //ASCII码处理方式                  
                    dicDatas.Add(dataByteModel.DictionaryKey, Encoding.ASCII.GetString(subChildData));
                    break;
                case 3:
                    //时间  
                    string data = (2000 + subChildData[0]) + "-" + subChildData[1] + "-" + subChildData[2] + " " + subChildData[3] + ":" + subChildData[4] + ":" + subChildData[5];
                    DateTime dtime = DateTime.Now;
                    try
                    {
                        DateTime.TryParse(data, out dtime);

                    }
                    catch (Exception ex) { }

                    dicDatas.Add(dataByteModel.DictionaryKey, dtime.ToString("yyyy-MM-dd HH:mm:ss"));
                    break;
                case 4:
                    //不处理                 
                    dicDatas.Add(dataByteModel.DictionaryKey, ByteToHexStr(subChildData).ToUpper());
                    break;
                case 5:
                    //电池包温度数据
                    StringBuilder st = new StringBuilder();
                    st.Append(subChildData[2]);
                    st.Append(":");
                    st.Append(subChildData[3]);
                    st.Append(":");
                    st.Append(subChildData[4]);
                    st.Append(":");
                    st.Append(subChildData[5]);
                    st.Append(":");
                    st.Append(subChildData[6]);
                    st.Append(":");
                    st.Append(subChildData[7]);
                    dicDatas.Add(dataByteModel.DictionaryKey, st.ToString());
                    break;
                case 6:
                    //电池包电压数据
                    byte[] bytesData = new byte[2];
                    StringBuilder st6 = new StringBuilder();
                    bytesData = subChildData.CloneRange(2, 2);
                    st6.Append(BitConverter.ToUInt16(bytesData, 0).ToString());
                    st6.Append(":");
                    bytesData = subChildData.CloneRange(4, 2);
                    st6.Append(BitConverter.ToUInt16(bytesData, 0).ToString());
                    st6.Append(":");
                    bytesData = subChildData.CloneRange(6, 2);
                    st6.Append(BitConverter.ToUInt16(bytesData, 0).ToString());
                    dicDatas.Add(dataByteModel.DictionaryKey, st6.ToString());
                    break;
                default:
                    break;
            }

        }
        /// <summary>
        /// Bit类型数据解析方式---目前支持单字节位解析
        /// </summary>
        /// <param name="dataBodyModel"></param>
        /// <param name="dicDatas"></param>
        /// <param name="childData"></param>
        ///   int valI = ((int)Ivar & (Convert.ToInt16("1".PadLeft(stratPos + len, '1'), 2))) >> (stratPos);
        ///    int valI = ((int)Ivar & ((int)Math.Pow(2, stratPos))) >> stratPos;
        public static void AnalysisBitState(PmDataBody dataBodyModel, Dictionary<string, string> dicDatas, byte[] childData)
        {
            byte[] subChildData = childData.CloneRange(dataBodyModel.StartPosition, dataBodyModel.DataLength);
            //处理数据方式
            List<PmDataBit> dataBitModel = dataBodyModel.PmDataBits.ToList();
            foreach (PmDataBit itemDataBit in dataBitModel)
            {
                //支持单字节按位处理
                int valI = ((int)subChildData[0] & (Convert.ToInt16("1".PadLeft(itemDataBit.ChildStartPostion + itemDataBit.ChildDataLength, '1'), 2))) >> (itemDataBit.ChildStartPostion);
                string reGetPostion = Convert.ToString(valI, 2).PadLeft(8, '0');
                dicDatas.Add(itemDataBit.DictionaryKey, reGetPostion);

            }

        }

        public static void AnalysisBit(PmDataBody dataBodyModel, Dictionary<string, string> dicDatas, byte[] childData)
        {
            byte[] subChildData = childData.CloneRange(dataBodyModel.StartPosition, dataBodyModel.DataLength);
            //处理数据方式
            List<PmDataBit> dataBitModel = dataBodyModel.PmDataBits.ToList();
            foreach (PmDataBit itemDataBit in dataBitModel)
            {
                string reGetPostion = GetPostionBitByBitArray(subChildData, itemDataBit.ChildStartPostion);
                dicDatas.Add(itemDataBit.DictionaryKey, reGetPostion);

            }

        }

        public static void AnalysisBitBool(PmDataBody dataBodyModel, Dictionary<string, string> dicDatas, byte[] childData)
        {
            byte[] subChildData = childData.CloneRange(dataBodyModel.StartPosition, dataBodyModel.DataLength);
            //处理数据方式
            PmDataByte dataByteModel = dataBodyModel.PmDataByte;

        }
        /// <summary>
        /// 移位算法
        /// </summary>
        /// <param name="byteData"></param>
        /// <param name="startPostion"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetPostionBitByBit(byte[] byteData, int startPostion, int length)
        {

            int valI = ((int)Convert.ToInt32(ByteToHexStr(byteData)) & (Convert.ToInt16("1".PadLeft(startPostion + length, '1'), 2))) >> (startPostion);
            return Convert.ToString(valI, 2).PadLeft(length, '0');
        }
        /// <summary>
        /// 使用BitArray函数确相应的位数据
        /// </summary>
        /// <param name="byteData"></param>
        /// <param name="startPostion"></param>
        /// <returns></returns>
        public static string GetPostionBitByBitArray(byte[] byteData, int startPostion)
        {
            StringBuilder st = new StringBuilder();
            BitArray bitSet = new BitArray(byteData);
            if (bitSet[startPostion])
            {
                return "1";
            }
            else
            {
                return "0";
            }

        }
        /// <summary>
        /// 获取指定byte[]指定字节中指定位置的二进制数据----------算法待优化
        /// </summary>
        /// <param name="byteData">源数据</param>

        /// <param name="startPostion">指定位开始位置</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string GetPostionBit(byte[] byteData, int startPostion, int length)
        {
            StringBuilder st = new StringBuilder();
            BitArray bitSet = new BitArray(byteData);
            int bits = 0;
            int binary = 7;
            string[] binNumber = new string[8];
            for (int i = 0; i <= bitSet.Count - 1; i++)
            {

                if (bitSet.Get(i) == true)

                    binNumber[binary] = "1";

                else

                    binNumber[binary] = "0";

                bits++; binary--;

                if ((bits % 8) == 0)
                {

                    binary = 7;

                    bits = 0;

                    for (int j = 0; j <= 7; j++)
                    {
                        st.Append(binNumber[j]);
                    }
                }
            }
            //字符串反转
            char[] newArr = new char[length];
            char[] arr = st.ToString().ToCharArray();
            Array.Copy(arr, startPostion, newArr, 0, length);

            return new String(newArr);
        }
        /// <summary>
        /// 字节数组经过高低位转化为新的字节数组-------效率可能有问题，后期优化
        /// </summary>
        /// <param name="recData"></param>
        /// <returns></returns>
        public static byte[] tranBigEndian(byte[] recData)
        {
            return StrToToHexByte(RenH2L(ByteToHexStr(recData)));
        }

        /// <summary>
        ///     字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteToHexStr(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
                sb.AppendFormat("{0:x2}", b);
            return sb.ToString();
        }
        /// <summary>
        ///     高低位转为正常
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String RenH2L(string str)
        {
            int isInt = str.Length % 2;
            if (isInt != 0)
            {
                str = str.Insert(str.Length - 1, "0"); // str.PadLeft(str.Length + isInt, '0');
            }
            var strTem = new StringBuilder("");
            int len = str.Length;

            for (int i = 0; i < len; i += 2)
            {
                strTem.Append(str.Substring(len - 2 - i, 2));
            }
            return strTem.ToString();
        }
        /// <summary>
        ///     字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] StrToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            var returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        /// <summary>
        ///默认已经按低字节在前的方式转换-- 字节数组转换为有符号的整型
        /// </summary>
        /// <param name="arrByte">源字节</param>
        /// <param name="offset">开始转换位</param>
        /// <param name="counts">字节数,1\2\4\8</param>      
        /// <returns></returns>
        public static long ConvertBytesToInt(byte[] arrByte, int offset, int counts)
        {
            long reInt = 0;

            switch (counts)
            {
                case 1:
                    reInt = arrByte[0];
                    break;
                case 2:
                    reInt = BitConverter.ToInt16(arrByte, offset);
                    break;
                case 4:
                    reInt = BitConverter.ToInt32(arrByte, offset);
                    break;
                case 8:
                    reInt = BitConverter.ToInt64(arrByte, offset);
                    break;
            }
            return reInt;
        }

        /// <summary>
        /// 字节数组转换为无符号的整型
        /// </summary>
        /// <param name="arrByte">源字节</param>
        /// <param name="offset">开始转换位</param>
        /// <param name="counts">字节数,1\2\4\8</param>       
        /// <returns></returns>
        public static ulong ConvertBytesToUInt(byte[] arrByte, int offset, int counts)
        {
            ulong reInt = 0;

            switch (counts)
            {
                case 1:
                    reInt = arrByte[0];
                    break;
                case 2:
                    reInt = BitConverter.ToUInt16(arrByte, offset);
                    break;
                case 4:
                    reInt = BitConverter.ToUInt32(arrByte, offset);
                    break;
                case 8:
                    reInt = BitConverter.ToUInt64(arrByte, offset);
                    break;
            }
            return reInt;
        }




        public static string GetReadRec(byte[] message)
        {
            var content = "";
            StringBuilder st = new StringBuilder();
            switch (message[0])
            {
                case 1:
                    st.Append("主IP:");
                    st.Append(message[1]);
                    st.Append(".");
                    st.Append(message[2]);
                    st.Append(".");
                    st.Append(message[3]);
                    st.Append(".");
                    st.Append(message[4]);
                    st.Append(":");
                    st.Append(BitConverter.ToInt16(message, 5).ToString());
                    st.Append("备用IP:");
                    st.Append(message[7]);
                    st.Append(".");
                    st.Append(message[8]);
                    st.Append(".");
                    st.Append(message[9]);
                    st.Append(".");
                    st.Append(message[10]);
                    st.Append(":");
                    st.Append(BitConverter.ToInt16(message, 11).ToString());
                    content = st.ToString();
                    break;
                case 2:
                    content = BitConverter.ToInt16(message, 1).ToString();
                    break;
                case 3:
                    content = BitConverter.ToInt32(message, 1).ToString();
                    break;
                case 4:
                    content = BitConverter.ToInt32(message, 1).ToString();
                    break;
                case 5:
                    content = BitConverter.ToInt32(message, 1).ToString();
                    break;
                case 6:
                    content = message[1].ToString();
                    break;


            }
            return content;
        }
    }
}
