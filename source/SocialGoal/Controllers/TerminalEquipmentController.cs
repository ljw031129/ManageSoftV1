using AutoMapper;
using SocialGoal.Core.Common;
using SocialGoal.Core.xFilter.Expressions;
using SocialGoal.Model.Models;
using SocialGoal.Model.ViewModels;
using SocialGoal.Service;
using SocialGoal.Web.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SocialGoal.Controllers
{
    public class TerminalEquipmentController : Controller
    {
        private readonly ITerminalEquipmentService _terminalEquipmentService;
        private readonly ITerminalSimCardService _terminalSimCardService;
        public TerminalEquipmentController(ITerminalSimCardService terminalSimCardService, ITerminalEquipmentService terminalEquipmentService)
        {
            this._terminalSimCardService = terminalSimCardService;
            this._terminalEquipmentService = terminalEquipmentService;
        }
        // GET: TerminalEquipment
        public ActionResult Index()
        {
            return View();
        }
        public async Task<string> Get(string terminalEquipmentId)
        {
            StringBuilder st = new StringBuilder();
            IEnumerable<TerminalSimCard> re = await _terminalSimCardService.GetAll();
            st.Append("<select>");
            foreach (var item in re)
            {
                st.Append("<option value='" + item.TerminalSimCardId + "'>" + item.TerminalSimCardNum + "</option>");

            }
            st.Append("</select>"); ;
            return st.ToString();
        }
        public async Task<JsonpResult> GetTerminalEquipment(int pageSize, int pageNum, string searchTerm)
        {
            Select2PagedResult orgEnterprises = await _terminalEquipmentService.GetSelect2PagedResult(searchTerm, pageSize, pageNum);
            //Return the data as a jsonp result
            return new JsonpResult
            {
                Data = orgEnterprises,
                JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet
            };
        }

        public async Task<ActionResult> GetList(JqGridSetting jqGridSetting)
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
                            EquipmentId = item.Equipment != null ? item.Equipment.EquipmentId : null,
                            ReceiveDataLastId = item.ReceiveDataLast != null ? item.ReceiveDataLast.ReceiveDataLastId : null,
                            TerminalEquipmentId = item.TerminalEquipmentId,
                            TerminalEquipmentNum = item.TerminalEquipmentNum,
                            TerminalEquipmentType = item.TerminalEquipmentType,
                            OrgEnterpriseId = !string.IsNullOrWhiteSpace(item.OrgEnterprise.ToString()) ? item.OrgEnterprise.OrgEnterpriseName : "",
                            PmFInterpreterId = item.PmFInterpreter.ProtocolName,
                            TerminalSimCardId = item.TerminalSimCard.TerminalSimCardNum,
                            TerminalEquipmentCreateTime = item.TerminalEquipmentCreateTime,
                            TerminalEquipmentUpdateTime = item.TerminalEquipmentUpdateTime
                        }).ToArray()
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> GetSubGrid(JqGridSetting jqGridSetting)
        {
            int count = 0;
            IEnumerable<TerminalEquipment> orgStructure = await _terminalEquipmentService.GetSubGridByEquipmentId(jqGridSetting, out count);
            var result = new
            {
                total = (int)Math.Ceiling((double)count / jqGridSetting.rows),
                page = jqGridSetting.page,
                records = count,
                rows = (from item in orgStructure.ToList()
                        select new
                        {
                            TerminalEquipmentId = item.TerminalEquipmentId,
                            TerminalEquipmentNum = item.TerminalEquipmentNum,
                            TerminalEquipmentType = item.TerminalEquipmentType,
                            OrgEnterpriseId = !string.IsNullOrWhiteSpace(item.OrgEnterprise.ToString()) ? item.OrgEnterprise.OrgEnterpriseName : "",
                            PmFInterpreterId = item.PmFInterpreter.ProtocolName,
                            TerminalSimCardId = item.TerminalSimCard.TerminalSimCardNum,
                            TerminalEquipmentCreateTime = item.TerminalEquipmentCreateTime,
                            TerminalEquipmentUpdateTime = item.TerminalEquipmentUpdateTime
                        }).ToArray()
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public string UpdateEquipmentId(string TerminalEquipmentId, string EquipmentIds)
        {
            _terminalEquipmentService.UpdateEquipmentId(TerminalEquipmentId, EquipmentIds);
            return "true";
        }

        [HttpPost]
        public string UpdateOrgEnterprise(string OrgEnterpriseId, string TerminalEquipmentIds)
        {
            _terminalEquipmentService.UpdateTerminalEquipmentOrgEnterpriseId(OrgEnterpriseId, TerminalEquipmentIds);
            return "true";
        }
        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult> Post(TerminalEquipmentViewModel newTerminalEquipment)
        {
            if (ModelState.IsValid)
            {

                TerminalEquipment terminalEquipment = Mapper.Map<TerminalEquipmentViewModel, TerminalEquipment>(newTerminalEquipment);
                switch (newTerminalEquipment.oper)
                {
                    case "add":
                        terminalEquipment.TerminalEquipmentId = Guid.NewGuid().ToString();
                        terminalEquipment.TerminalEquipmentUpdateTime = DateTime.Now;
                        terminalEquipment.TerminalEquipmentCreateTime = DateTime.Now;
                        // var errors = _orgEnterpriseService.CanAdd(equipment).ToList();
                        await _terminalEquipmentService.CreateAsync(terminalEquipment);
                        return Json(new { success = true });

                    case "edit":
                        terminalEquipment.TerminalEquipmentUpdateTime = DateTime.Now;
                        await _terminalEquipmentService.UpdateAsync(terminalEquipment);
                        return Json(new { success = true });

                    case "del":
                        bool rec = await _terminalEquipmentService.DeleteAsync(newTerminalEquipment.id);
                        if (rec)
                        {
                            return Json(new { success = true });
                        }
                        break;

                }

            }
            // ModelState.AddModelErrors(errors);
            return Json(new { errors = GetErrorsFromModelState() });
        }
        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }
    }
}