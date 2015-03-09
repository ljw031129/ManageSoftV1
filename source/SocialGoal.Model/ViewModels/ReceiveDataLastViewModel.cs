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
        //终端编号
        public string IMEI { get; set; }
        //判断当前上传数据的类型---定位数据/状态数据
        public string DataType { get; set; }
        //回传时间
        public string  Rtime { get; set; }
        //定位时间
        public string  Ptime { get; set; }
        public string AccStatus { get; set; }
        //定位状态-是否定位
        public string IsPos { get; set; }
        //位置
        public string Pos { get; set; }
        //定位模式-实时/差分定位
        public string PostMode { get; set; }
        //纬度
        public string Lat { get; set; }
        //经度
        public string Lng { get; set; }
        //速度
        public string Gspeed { get; set; }
        //高度
        public string Ghigh { get; set; }
        //方向
        public string GsDirection { get; set; }
        //GSM信号强度
        public string GsmSignal { get; set; }
        //可视卫星颗数
        public string SeeSatelliteCount { get; set; }
        //使用卫星颗数
        public string UseSatelliteCount { get; set; }
        //基站数据
        public string MCC { get; set; }
        public string MNC { get; set; }
        public string LAC { get; set; }
        public string CID { get; set; }
        //是否为东经
        public string East { get; set; }
        //是否为北纬
        public string North { get; set; }
        //区分实时还是补发数据----盲去数据
        // public string SupplyData { get; set; }
        //定位类型，实时/差分
        public string GpsType { get; set; }

        //无线设备工作情况

        //设备工作状态  测试  拆除  激活
        public string WorkStatue { get; set; }
        //累计工作时间
        public string TootalWorkTime { get; set; }
        //设备工作模式  标准  精准 追车 
        public string WorkModel { get; set; }
        //每天第一次启动时间
        public string Timing { get; set; }
        //设备休眠时间
        public string SleepTime { get; set; }

        //工作时间  默认2分钟
        public string WorkTime { get; set; }
        //设备电池电压
        public string BatteryVoltage { get; set; }
        //盲去数据数量
        public string BlindDataCount { get; set; }
        //盲区标志
        public string BlindSign { get; set; }
        //设备意外启动次数
        public string StartCount { get; set; }
        //间隔时间-追车模式
        public string IntervalTime { get; set; }
        //硬件版本号
        public string HardwareVersion { get; set; }
        //软件版本号
        public string SoftVersion { get; set; }
        //协议版本号version
        public string ProtocolVersion { get; set; }

    }
}
