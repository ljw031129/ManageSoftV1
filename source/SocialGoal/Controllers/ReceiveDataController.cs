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

namespace SocialGoal.Controllers
{
    [Authorize]
    public class ReceiveDataController : Controller
    {
        private readonly IOrgEnterpriseService _orgEnterpriseService;
        private readonly IReceiveDataService _receiveDataService;
        private readonly ITerminalEquipmentService _terminalEquipmentService;
        public ReceiveDataController(IReceiveDataService receiveDataService, IOrgEnterpriseService orgEnterpriseService, ITerminalEquipmentService terminalEquipmentService)
        {
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
                            ReceiveTime = item.ReceiveTime,
                            GpsPlat = item.GpsPlat,
                            GpsPlog = item.GpsPlog,
                            GpsPos = item.GpsPos
                        }).ToArray()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetReceiveDataLasts(JqGridSetting jqGridSetting)
        {
            //得到当前用户可见企业id
            string userId = User.Identity.GetUserId();
            string[] al = await _orgEnterpriseService.GetOrgEnterpriseArraylist(userId);

            List<string> st = _terminalEquipmentService.GetCurrentUserTerminalEquipments(al);

            int count = 0;
            IEnumerable<ReceiveDataLast> orgStructure = await _receiveDataService.GetReceiveDataLasts(jqGridSetting, st, out count);
            var result = new
            {
                total = (int)Math.Ceiling((double)count / jqGridSetting.rows),
                page = jqGridSetting.page,
                records = count,
                rows = (from item in orgStructure.ToList()
                        select new
                        {

                            IMEI = item.IMEI,
                            GpsPlat = item.GpsPlat,
                            GpsPlog = item.GpsPlog,
                            GpsPos = item.GpsPos,
                            AccStatus = item.AccStatus,
                            AntennaStatus=item.AntennaStatus,//电池联通状态
                            ReceiveTime = DateUtils.GetTime(item.ReceiveTime.ToString()).ToString("yyyy-MM-dd HH:mm:ss")
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