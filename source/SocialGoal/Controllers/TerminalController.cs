using SocialGoal.Core.xFilter.Expressions;
using SocialGoal.Model.Models;
using SocialGoal.Model.ViewModels;
using SocialGoal.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SocialGoal.Controllers
{
    public class TerminalController : Controller
    {
        private readonly ITerminalEquipmentService _terminalEquipmentService;
        private readonly IReceiveDataLastService _receiveDataLastService;
        private readonly IReceiveDataDisplayService _receiveDataDisplayService;
        private readonly IReceiveDataService _receiveDataService;

        public TerminalController(IReceiveDataService receiveDataService, IReceiveDataDisplayService receiveDataDisplayService, IReceiveDataLastService eceiveDataLastService, ITerminalEquipmentService terminalEquipmentService)
        {
            this._receiveDataService = receiveDataService;
            this._receiveDataDisplayService = receiveDataDisplayService;
            this._receiveDataLastService = eceiveDataLastService;
            this._terminalEquipmentService = terminalEquipmentService;
        }
        // GET: Terminal
        public ActionResult Index()
        {
            ViewBag.DevId = "";
            return View();
        }
        public ActionResult Detail(string Id)
        {
            ViewBag.DevId = Id;
            ReceiveDataLast rdl = _receiveDataLastService.GetReceiveDataLastByTerminalNum(Id);
            return View(rdl);
        }
        public ActionResult Map(string id)
        {
            ViewBag.DevId = id;
            return View();
        }
        public ActionResult History(string id)
        {
            ViewBag.DevId = id;

            return View();
        }
        public ActionResult HistoryTableData(string id)
        {
            ViewBag.DevId = id;

            return View();
        }
        public JsonResult HistoryTableSet(string id)
        {
            ViewBag.DevId = id;
            ArrayList ColNs = new ArrayList();
            List<ReceiveDataDisplay> tdLs = _receiveDataDisplayService.GetDataByPmFInterpreterByDevid(id).ToList();
            IList<object> ColMs = new List<object>();
            foreach (var item in tdLs)
            {
                ColNs.Add(item.DictionaryValue);
                var colms = new
                {

                    name = item.DictionaryKey,
                    index = item.DictionaryKey//,
                   // width = 100
                };
                ColMs.Add(colms);
            }
            var resultObj = new
            {
                ColNs = ColNs,    // 总页数
                ColMs = ColMs
            };
            return Json(resultObj, JsonRequestBehavior.AllowGet);
        }
       
        public async Task<ActionResult> GetTerminalEquipmentDetail(JqGridSetting jqGridSetting)
        {
            int count = 0;
            IEnumerable<TerminalEquipment> orgStructure = await _terminalEquipmentService.GetOrgStructures(jqGridSetting, out count);



            var result = new
            {
                total = (int)Math.Ceiling((double)count / jqGridSetting.rows),
                page = jqGridSetting.page,
                records = count,
                rows = (from item in orgStructure.ToList()
                        select new
                        {
                            EquipmentTypeId = item.Equipment != null ? item.Equipment.EquipmentTypeId : "",
                            EquipmentNum = item.Equipment != null ? item.Equipment.EquipmentNum : "",
                            EquipmentName = item.Equipment != null ? item.Equipment.EquipmentName : "",
                            TerminalEquipmentId = item.TerminalEquipmentId,
                            TerminalEquipmentNum = item.TerminalEquipmentNum,
                            TerminalEquipmentType = item.TerminalEquipmentType,
                            //最新信息
                            TotalWorkTime = item.ReceiveDataLast != null ? item.ReceiveDataLast.TotalWorkTime.ToString() : "",
                            ReceiveTime = item.ReceiveDataLast.ReceiveTime != null ? item.ReceiveDataLast.ReceiveTime.ToString() : "",
                            AccStatus = item.ReceiveDataLast.AccStatus != null ? item.ReceiveDataLast.AccStatus.ToString() : "",
                            GpsPos = item.ReceiveDataLast.GpsPos != null ? item.ReceiveDataLast.GpsPos.ToString() : ""
                        }).ToArray()
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public async Task<ActionResult> GetTerminalEquipmentHistory(JqGridSetting jqGridSetting)
        {
            int count = 0;
            IEnumerable<ReceiveData> orgStructure = await _receiveDataService.GetReceiveDataHistory(jqGridSetting, out count);
            IList<object> ColMs = new List<object>();
            ArrayList rowArray = new ArrayList();
            foreach (var item in orgStructure.ToList())
            {
                rowArray.Add(item);

            }
            var result = new
            {
                total = (int)Math.Ceiling((double)count / jqGridSetting.rows),
                page = jqGridSetting.page,
                records = count,
                rows = rowArray
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }
    }
}