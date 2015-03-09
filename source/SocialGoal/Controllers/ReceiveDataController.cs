using ProtocolUtils.Lbs;
using SocialGoal.Core.xFilter.Expressions;
using SocialGoal.Model.Models;
using SocialGoal.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Utilities;
using Microsoft.AspNet.Identity;
using SocialGoal.Core.DynamicLINQ;

namespace SocialGoal.Controllers
{
    [Authorize]
    public class ReceiveDataController : Controller
    {
        private readonly IOrgEnterpriseService _orgEnterpriseService;
        private readonly IReceiveDataService _receiveDataService;
        private readonly ITerminalEquipmentService _terminalEquipmentService;
        private readonly IEquipmentService _equipmentService;
        public ReceiveDataController(IEquipmentService equipmentService, IReceiveDataService receiveDataService, IOrgEnterpriseService orgEnterpriseService, ITerminalEquipmentService terminalEquipmentService)
        {
            this._equipmentService = equipmentService;
            this._receiveDataService = receiveDataService;
            this._orgEnterpriseService = orgEnterpriseService;
            this._terminalEquipmentService = terminalEquipmentService;
        }
        // GET: ReceiveData
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> GetReceiveDataMap(string devId, string dateRange, int pageNum, int pageSize)
        {
            int count = 0;
            IEnumerable<ReceiveData> ReceiveDatas = await _receiveDataService.GetReceiveDataMapata(devId, dateRange, pageNum, pageSize, out count);


            var result = new
            {
                total = (int)Math.Ceiling((double)count / pageSize),
                page = pageNum,
                records = count,
                rows = (from item in ReceiveDatas.ToList()
                        select new
                        {
                            ReceiveTime = item.Rtime,
                            GpsPlat = item.Lat,
                            GpsPlog = item.Lng,
                            GpsPos = item.Pos
                        }).ToArray()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 设备最后数据更新状态信息
        /// </summary>
        /// <param name="jqGridSetting"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetReceiveDataLasts(JqGridSetting jqGridSetting)
        {
            //得到当前用户可见企业id
            string userId = User.Identity.GetUserId();
            string[] al = await _orgEnterpriseService.GetOrgEnterpriseArraylist(userId);

            List<string> st = _terminalEquipmentService.GetCurrentUserTerminalEquipments(al);

            int count = 0;
            IEnumerable<ReceiveDataLast> orgStructure = await _receiveDataService.GetReceiveDataLasts(jqGridSetting, st, out count);
            // IEnumerable<TerminalEquipment> orgStructures = await _terminalEquipmentService.GetTerminalEquipments(jqGridSetting,st, out count);
            var result = new
            {
                total = (int)Math.Ceiling((double)count / jqGridSetting.rows),
                page = jqGridSetting.page,
                records = count,
                rows = (from item in orgStructure.ToList()
                        select new
                        {
                            IMEI = item.IMEI,
                            GpsPlat = item.Lat,
                            GpsPlog = item.Lng,
                            GpsPos = item.Pos,
                            AccStatus = item.AccStatus,
                            AntennaStatus = item.AntennaStatus,//电池联通状态
                            ReceiveTime = DateUtils.GetTime(item.Rtime.ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                        }).ToArray()
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public async Task<ActionResult> GetEquipmentDataLasts(JqSearchIn jqGridSetting)
        {
            int count = 0;
            string userId = User.Identity.GetUserId();
            string[] al = await _orgEnterpriseService.GetOrgEnterpriseArraylist(userId);
            List<String> st = new List<System.String>(al);
            IEnumerable<TerminalEquipment> orgStructure = await _terminalEquipmentService.GetTerminalEquipments(jqGridSetting, st, out count);
            // IEnumerable<TerminalEquipment> orgStructures = await _terminalEquipmentService.GetTerminalEquipments(jqGridSetting,st, out count);
            var result = new
            {
                total = (int)Math.Ceiling((double)count / jqGridSetting.rows),
                page = jqGridSetting.page,
                records = count,
                rows = (from item in orgStructure.ToList()
                        select new
                        {
                            EquipmentNum = item.Equipment != null ? item.Equipment.EquipmentNum : "",//车号
                            EquipmentName = item.Equipment != null ? item.Equipment.EquipmentName : "",//车牌号
                            TerminalEquipmentNum = item.TerminalEquipmentNum,//终端号
                            GpsPlat = item.ReceiveDataLast.Lat,
                            GpsPlog = item.ReceiveDataLast.Lng,
                            GpsPos = item.ReceiveDataLast.Pos,
                            AccStatus = item.ReceiveDataLast.AccStatus,
                            AntennaStatus = item.ReceiveDataLast.AntennaStatus,//电池联通状态
                            ReceiveTime = DateUtils.GetTime(item.ReceiveDataLast.Rtime.ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                        }).ToArray()
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public async Task<ActionResult> ReceiveDataHistoryList(JqSearchIn jqGridSetting, string terminalEquipmentNum)
        {
            int count = 0;

            IEnumerable<ReceiveData> orgStructure = await _receiveDataService.GetreceiveDataHistory(terminalEquipmentNum, jqGridSetting, out count);
            var result = new
            {
                total = (int)Math.Ceiling((double)count / jqGridSetting.rows),
                page = jqGridSetting.page,
                records = count,
                rows = (from item in orgStructure.ToList()
                        select new
                        {
                            IMEI = item.IMEI,
                            //定位信息
                            Pos = item.Pos != null ? item.Pos.ToString() : "",
                            Lat = item.Lat != null ? item.Lat.ToString() : "0.000000",
                            Lng = item.Lng != null ? item.Lng.ToString() : "0.000000",
                            IsPos = item.IsPos != null ? item.IsPos.ToString() : "",
                            LAC = item.LAC != null ? item.LAC.ToString() : "",
                            CID = item.CID != null ? item.ProtocolVersion.ToString() : "",
                            SoftVersion = item.SoftVersion != null ? item.SoftVersion.ToString() : "",
                            HardwareVersion = item.HardwareVersion != null ? item.HardwareVersion.ToString() : "",


                            BatteryVoltage = item.BatteryVoltage != null ? item.BatteryVoltage.ToString() : "",
                            GsmSignal = item.GsmSignal != null ? item.GsmSignal.ToString() : "",
                            BlindSign = item.BlindSign != null ? item.BlindSign.ToString() : "",
                            BlindDataCount = item.BlindDataCount != null ? item.BlindDataCount.ToString() : "",
                            SeeSatelliteCount = item.SeeSatelliteCount != null ? item.SeeSatelliteCount.ToString() : "",
                            UseSatelliteCount = item.UseSatelliteCount != null ? item.UseSatelliteCount.ToString() : "",


                            WorkStatue = item.WorkStatue != null ? item.WorkStatue.ToString() : "",
                            WorkModel = item.WorkModel != null ? item.WorkModel.ToString() : "",
                            WorkTime = item.WorkTime != null ? item.WorkTime.ToString() : "",
                            SleepTime = item.SleepTime != null ? item.SleepTime.ToString() : "",
                            IntervalTime = item.IntervalTime != null ? item.IntervalTime.ToString() : "",
                            TootalWorkTime = item.TootalWorkTime != null ? item.TootalWorkTime.ToString() : "",
                            StartCount = item.StartCount != null ? item.StartCount.ToString() : "",


                            Ptime = DateUtils.GetTime(item.Ptime.ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                            Rtime = DateUtils.GetTime(item.Rtime.ToString()).ToString("yyyy-MM-dd HH:mm:ss")

                        }).ToArray()
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Monitor()
        {

            return View();
        }

    }
}