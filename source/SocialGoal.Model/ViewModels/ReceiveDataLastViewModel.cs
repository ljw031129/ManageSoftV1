using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Model.ViewModels
{
    public class ReceiveDataLastViewModel
    {
        public string ReceiveDataLastId { get; set; }
        public string IMEI { get; set; }
        public string  ReceiveTime { get; set; }
        public long CollectTime { get; set; }
        //总工作时间
        public long TotalWorkTime { get; set; }
        //总里程
        public long TotalMileage { get; set; }
        public string GpsTime { get; set; }
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
      
    }
}
