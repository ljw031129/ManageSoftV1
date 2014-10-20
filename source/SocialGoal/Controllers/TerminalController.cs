using SocialGoal.Core.xFilter.Expressions;
using SocialGoal.Model.Models;
using SocialGoal.Model.ViewModels;
using SocialGoal.Service;
using System;
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
        public TerminalController(IReceiveDataLastService eceiveDataLastService, ITerminalEquipmentService terminalEquipmentService)
        {
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
        public ActionResult HistoryTableSet(string id)
        {
            ViewBag.DevId = id;
            return View();
        }
        public JsonResult GetreceiveDataLast(string terminalNum)
        {
            List<TerminalDataViewModel> tdLs = _receiveDataLastService.GetTerminalDataByTerminalNum(terminalNum);
            return Json(tdLs, JsonRequestBehavior.AllowGet);
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
            return Json(result,JsonRequestBehavior.AllowGet);

        }
    }
}