using System;
using System.Data;
using System.IO;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using ProtocolsManage.Model;
using ProtocolsManage.Common;
using System.Linq;

namespace ProtocolsManage.Common
{
    public class Comm
    {
        /// <summary>
        ///     异或校验
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte GetBcc(byte[] data)
        {
            if (data == null) return 0;

            int length = data.Length;
            if (length == 0) return 0;
            if (length == 1) return data[0];

            byte bcc = data[0];
            for (int i = 1; i < length; i++)
            {
                bcc ^= data[i];
            }

            return bcc;
        }


        public static bool isHex(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            //string reP = "^[0-9a-fA-f]{" + str.Length + ",}?$";
            string reP = "^[\x30-\x39\x41-\x46\x61-\x66]{" + str.Length + "}$";
            if (Regex.IsMatch(str, reP))
            {
                return true;
            }
            return false;
        }

        public static bool isTen(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            string reP = @"^[\x30-\x39]{" + str.Length + "}$";
            if (Regex.IsMatch(str, reP))
            {
                return true;
            }
            return false;
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
        ///     飞图高地位转换方式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String RenH2Lf(string str)
        {
            int isInt = str.Length % 2;
            if (isInt != 0)
            {
                str = str.Insert(str.Length - 1, "0"); // str.PadLeft(str.Length + isInt, '0');
            }
            var strTem = new StringBuilder("");
            int len = str.Length;

            for (int i = 0; i < len; i += 4)
            {
                strTem.Append(RenH2L(str.Substring(0 + i, 4)));
            }
            return strTem.ToString();
        }

        /// <summary>
        ///     字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        /// <summary>
        ///     字节数组转16进制字符串_空格
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStr_k(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2") + " ";
                }
            }
            return returnStr.Trim();
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
        public static string StrToHex(string str)
        {
            string strTemp = "";
            if (str == "")
                return "";
            byte[] bTemp = System.Text.Encoding.Default.GetBytes(str);

            for (int i = 0; i < bTemp.Length; i++)
            {
                strTemp += bTemp[i].ToString("X");
            }
            return strTemp;
        }
        /// <summary>
        ///     CRC校验
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CRC16(byte[] data, int length)
        {
            int quotient, i, j;
            int remainder, divisor = 0x1021;
            int data1;
            data[length] = 0;
            data[length + 1] = 0;
            remainder = 0;
            for (j = 0; j < (length + 2); j++)
            {
                data1 = data[j];
                for (i = 8; i > 0; i--)
                {
                    quotient = remainder & 0x8000;
                    remainder <<= 1;
                    if (Convert.ToBoolean((data1 <<= 1) & 0x0100))
                        remainder |= 1;
                    if (Convert.ToBoolean(quotient))
                        remainder ^= divisor;
                }
            }
            string fstr = Convert.ToString(remainder, 16);
            return fstr.Substring(fstr.Length - 4, 4).ToUpper();
        }

        public static int CheckReplace(byte[] mBTCmdDataBase, byte[] pattern)
        {
            //        byte[] mBTCmdDataBase =
            //{ 
            //  0X00, 0X00, 0X00, 0X03, 0X00, 0X00, 0X00, 0X00, 0X00, 0X00, 0X00, 
            //  0X04, 0X00, 0X00, 0X00, 0X03, 0X00, 0X00, 0X00, 0X04, 0X00, 0X00, 
            //  0X00, 0X04, 0X00, 0X00, 0X00, 0X08, 0X00, 0X00, 0X00, 0X00, 0X00, 
            //  0X00, 0X00, 0X00, 0X00, 0X00, 0X00, 0X08, 0X00, 0X00, 0X00, 0X06, 
            //  0X00, 0X00, 0X00, 0X06, 0X68, 0X73, 0X5f, 0X62, 0X61, 0X79, 0X00, 
            //  0X00, 0X00, 0X00, 0X00, 0X07, 0X00, 0X00, 0X00, 0X20, 0X00, 0X00, 
            //  0X00, 0X03, 0X00, 0X00, 0X00, 0X00, 0X00, 0X00, 0X00, 0X01, 0X00, 
            //  0X00, 0X00, 0X08, 0X00, 0X00, 0X00, 0X08, 0X00, 0X00, 0X00, 0X08
            //};
            //        byte[] pattern = { 0X68, 0X73, 0X5f, 0X62, 0X61, 0X79 };
            var s1 = new string(Encoding.UTF8.GetChars(mBTCmdDataBase));
            var s2 = new string(Encoding.UTF8.GetChars(pattern));
            int index = s1.IndexOf(s2);


            return index;
        }

        public static DateTime GpsVoid(double miao)
        {
            var s = new DateTime(2000, 1, 1);
            s = s.AddSeconds(miao);
            s.ToString("yyyyMMdd-HH:mm:ss");
            return s;
        }


        #region 解析经伟度

        /// <summary>
        ///     解析经伟度
        /// </summary>
        /// <param name="coordinate"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static
        Decimal Coordinate(string coordinate, bool flag)
        {
            int last = 2;
            if (flag)
            {
                last = 3;
            }
            string Later = coordinate.Substring(last);
            string begin = coordinate.Substring(0, last);
            Decimal fLater = 0;
            if (flag)
            {
                fLater = Decimal.Parse(begin) + Decimal.Parse(Later) / 6000000;
            }
            else
            {
                fLater = Decimal.Parse(begin) + Decimal.Parse(Later) / 600000;
            }
            return fLater;
        }

        #endregion

        #region gps时间转换

        /// <summary>
        ///     gps时间转换
        /// </summary>
        /// <param name="ge"></param>
        /// <param name="gpsTime"></param>
        public static string gpsTimeConvertor(String gpsTime)
        {
            string gpsTime_temp = "20" + gpsTime;
            if (!string.IsNullOrEmpty(gpsTime.Trim()))
            {
                try
                {
                    DateTime time = DateTime.ParseExact(gpsTime_temp, "yyyyMMddHHmmss", null);
                    if (time.Year > DateTime.Now.Year)
                    {
                        gpsTime_temp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        return gpsTime_temp;
                    }
                    gpsTime_temp = time.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss");
                }
                catch (Exception)
                {
                    gpsTime_temp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            else
            {
                gpsTime_temp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            return gpsTime_temp;
        }

        #endregion


    }
}