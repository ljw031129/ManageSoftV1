using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Utilities;

namespace SocialGoal.Model.Models
{
    public class ReceiveDataLast
    {
        //初始值
        public ReceiveDataLast()
        {
            // ReceiveDataLastId =Guid.NewGuid().ToString();
            ReceiveTime = DateUtils.ConvertDateTimeIntInt(DateTime.Now);
            CollectTime = DateUtils.ConvertDateTimeIntInt(DateTime.Now);
            TotalWorkTime = 0;
            TotalMileage = 0;
            GpsTime = DateUtils.ConvertDateTimeIntInt(DateTime.Now);
            GpsPos = "北京市石景山区石景山路15号";
            GpsPlat = "39.9050521650";
            GpsPlog = "116.2278704230";
            GpsSpeed = "0";
            GpsDirection = "0";
        }
        public string ReceiveDataLastId { get; set; }
        public string IMEI { get; set; }
        public long ReceiveTime { get; set; }
        public long CollectTime { get; set; }
        //总工作时间
        public long TotalWorkTime { get; set; }
        //总里程
        public long TotalMileage { get; set; }
        public long GpsTime { get; set; }
        public string GpsIsPos { get; set; }
        public string GpsPosProvince { get; set; }
        public string GpsPos { get; set; }
        public string GpsPlat { get; set; }
        public string GpsPlog { get; set; }
        public string GpsSpeed { get; set; }
        public string GpsDirection { get; set; }
        //海拔
        public string GpsElevation { get; set; }
        public string GsmSignal { get; set; }
        //基站信息
        public string CellInformationMain { get; set; }
        public string CellInformation1 { get; set; }
        public string CellInformation2 { get; set; }
        public string CellInformation3 { get; set; }
        //天线状态
        public string AntennaStatus { get; set; }
        public string AccStatus { get; set; }
        public string OutStatus { get; set; }
        public string SleepStatus { get; set; }
        //报警状态
        public string AlarmStatus { get; set; }
        //锁车状态
        public string LockStatus { get; set; }

        public string F1 { get; set; }
        public string F2 { get; set; }
        public string F3 { get; set; }
        public string F4 { get; set; }
        public string F5 { get; set; }
        public string F6 { get; set; }
        public string F7 { get; set; }
        public string F8 { get; set; }
        public string F9 { get; set; }
        public string F10 { get; set; }
        public string F11 { get; set; }
        public string F12 { get; set; }
        public string F13 { get; set; }
        public string F14 { get; set; }
        public string F15 { get; set; }
        public string F16 { get; set; }
        public string F17 { get; set; }
        public string F18 { get; set; }
        public string F19 { get; set; }
        public string F20 { get; set; }
        public string F21 { get; set; }
        public string F22 { get; set; }
        public string F23 { get; set; }
        public string F24 { get; set; }
        public string F25 { get; set; }
        public string F26 { get; set; }
        public string F27 { get; set; }
        public string F28 { get; set; }
        public string F29 { get; set; }
        public string F30 { get; set; }
        public string F31 { get; set; }
        public string F32 { get; set; }
        public string F33 { get; set; }
        public string F34 { get; set; }
        public string F35 { get; set; }
        public string F36 { get; set; }
        public string F37 { get; set; }
        public string F38 { get; set; }
        public string F39 { get; set; }
        public string F40 { get; set; }
    }
}
