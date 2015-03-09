using SocialGoal.Model.Models;
using SocialGoal.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SocialGoal.Model.ViewModels;
using Web.Utilities;
using AutoMapper;
using ProtocolUtils.Models;

namespace SocialGoal.Controllers
{
    [Authorize]
    public class TerminalEquipmentCommandController : Controller
    {
        private readonly ITerminalEquipmentCommandService _terminalEquipmentCommandService;
        private readonly IOrgEnterpriseService _orgEnterpriseService;
        private readonly ITerminalEquipmentService _terminalEquipmentService;
        public TerminalEquipmentCommandController(ITerminalEquipmentService terminalEquipmentService, IOrgEnterpriseService orgEnterpriseService, ITerminalEquipmentCommandService terminalEquipmentCommandService)
        {
            this._orgEnterpriseService = orgEnterpriseService;
            this._terminalEquipmentCommandService = terminalEquipmentCommandService;
            this._terminalEquipmentService = terminalEquipmentService;
        }
        // GET: TerminalEquipmentCommand
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult InsertSendData(string IMEI, string MsgJson)
        {
            TerminalEquipmentCommand te = new TerminalEquipmentCommand();
            te.IMEI = IMEI;
            te.OperateDataHex = "";
            te.OperateStatue = "1";
            te.CommandFromTo = "";
            te.CommandJsonData = MsgJson;
            te.UserId = User.Identity.GetUserId();
            _terminalEquipmentCommandService.CreateAsync(te);
            return Json("true");
        }
        /// <summary>
        /// 批量下发指令
        /// </summary>
        /// <param name="IMEI"></param>
        /// <param name="MsgJson"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsertSendDataMultiselect(string IMEIS, string MsgJson)
        {
            int count = 0;
            foreach (var item in IMEIS.Trim(',').Split(','))
            {
                TerminalEquipmentCommand te = new TerminalEquipmentCommand();
                te.IMEI = item.Trim();
                te.OperateDataHex = "";
                te.OperateStatue = "1";
                te.CommandFromTo = "";
                te.CommandJsonData = MsgJson;
                te.UserId = User.Identity.GetUserId();
                _terminalEquipmentCommandService.CreateAsync(te);
                count += count;
            }
            if (count > 0)
            {
                return Json(count + "条命令写入成功！");
            }
            else
            {
                return Json("命令写入失败！");
            }

        }
        /// <summary>
        /// 当前用户可见的带发送命令List
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="numPerPage"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetTerminalEquipmentCurrentCommand(int currentPage, int numPerPage)
        {
            int count = 0;
            string userId = User.Identity.GetUserId();
            string[] al = await _orgEnterpriseService.GetOrgEnterpriseArraylist(userId);
            List<string> st = _terminalEquipmentService.GetCurrentUserTerminalEquipments(al);
            IEnumerable<TerminalEquipmentCommandCurrent> orgStructure = await _terminalEquipmentCommandService.GetTerminalEquipmentCurrentCommands(st, currentPage, numPerPage, out count);
            var data = Mapper.Map<IEnumerable<TerminalEquipmentCommandCurrent>, IEnumerable<TerminalEquipmentCommandCurrentViewModel>>(orgStructure).ToArray();
            var result = new
            {
                total = (int)Math.Ceiling((double)count / numPerPage),
                page = currentPage,
                records = count,
                rows = (from item in data.ToList()
                        select new
                        {
                            TerminalEquipmentCommandId = item.TerminalEquipmentCommandId,
                            CommandFromTo = item.CommandFromTo,
                            CommandJsonData = item.CommandJsonData != null ? JsonConvert.DeserializeObject<CommandJsonData>(item.CommandJsonData) : new CommandJsonData(),
                            ReceiveTData = item.ReceiveTData != null ? JsonConvert.DeserializeObject<PostionModel>(item.ReceiveTData) : new PostionModel(),
                            Dtype = item.Dtype,
                            IMEI = item.IMEI,
                            OperateDataHex = item.OperateDataHex != null ? item.OperateDataHex : "",
                            OperateStatue = item.OperateStatue != null ? item.OperateStatue : "",
                            OperateTime = DateUtils.GetPrettyDate(item.OperateTime),
                            OperateDateTime = DateUtils.GetTime(item.OperateTime).ToString("yyyy-MM-dd HH:mm:ss"),
                            UserName = item.UserId != null ? item.User.UserName : ""
                        }).ToArray()
            };
            return Json(result);
        }
        [HttpPost]
        public async Task<ActionResult> GetTerminalEquipmentCommand(string IMEI, int currentPage, int numPerPage)
        {
            int count = 0;

            IEnumerable<TerminalEquipmentCommand> orgStructure = await _terminalEquipmentCommandService.GetTerminalEquipmentCommands(IMEI, currentPage, numPerPage, out count);
            var data = Mapper.Map<IEnumerable<TerminalEquipmentCommand>, IEnumerable<TerminalEquipmentCommandViewModel>>(orgStructure).ToArray();
            var result = new
            {
                total = (int)Math.Ceiling((double)count / numPerPage),
                page = currentPage,
                records = count,
                rows = (from item in data.ToList()
                        select new
                        {
                            TerminalEquipmentCommandId = item.TerminalEquipmentCommandId,
                            CommandFromTo = item.CommandFromTo,
                            CommandJsonData = item.CommandJsonData != null ? JsonConvert.DeserializeObject<CommandJsonData>(item.CommandJsonData) : new CommandJsonData(),
                            ReceiveTData = item.ReceiveTData != null ? JsonConvert.DeserializeObject<PostionModel>(item.ReceiveTData) : new PostionModel(),
                            Dtype = item.Dtype,
                            IMEI = item.IMEI,
                            OperateDataHex = item.OperateDataHex != null ? item.OperateDataHex : "",
                            OperateStatue = item.OperateStatue != null ? item.OperateStatue : "",
                            OperateTime = DateUtils.GetPrettyDate(item.OperateTime),
                            OperateDateTime = DateUtils.GetTime(item.OperateTime).ToString("yyyy-MM-dd HH:mm:ss"),
                            UserName = item.UserId != null ? item.User.UserName : ""
                        }).ToArray()
            };
            return Json(result);
        }
    }
}