using AutoMapper;
using SocialGoal.Core.xFilter.Expressions;
using SocialGoal.Model.Models;
using SocialGoal.Service;
using SocialGoal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SocialGoal.Controllers
{
    public class ApiTerminalEquipmentController : ApiController
    {
        private readonly ITerminalEquipmentService _terminalEquipmentService;
        public ApiTerminalEquipmentController(ITerminalEquipmentService terminalEquipmentService)
        {
            this._terminalEquipmentService = terminalEquipmentService;
        }

        public async Task<Object> Get([FromUri]JqGridSetting jqGridSetting)
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
            return result;

        }
        [Route("api/ApiTerminalEquipment/GetSubGrid")]
        public async Task<Object> GetSubGrid([FromUri]JqGridSetting jqGridSetting)
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
            return result;

        }
        [HttpPost]
        [Route("api/ApiTerminalEquipment/UpdateEquipmentId/{TerminalEquipmentId}/{EquipmentIds}")]
        public string UpdateEquipmentId(string TerminalEquipmentId, string EquipmentIds)
        {

            _terminalEquipmentService.UpdateEquipmentId(TerminalEquipmentId, EquipmentIds);
            return "true";
        }

        [HttpPost]
        [Route("api/ApiTerminalEquipment/UpdateOrgEnterprise/{OrgEnterpriseId}/{TerminalEquipmentIds}")]
        public string UpdateTerminalEquipmentOrgEnterprise(string OrgEnterpriseId, string TerminalEquipmentIds)
        {
            _terminalEquipmentService.UpdateTerminalEquipmentOrgEnterpriseId(OrgEnterpriseId, TerminalEquipmentIds);
            return "true";
        }
        // POST api/<controller>
        [HttpPost]
        public async Task<IHttpActionResult> Post(TerminalEquipmentViewModel newTerminalEquipment)
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
                        return Ok();

                    case "edit":
                        terminalEquipment.TerminalEquipmentUpdateTime = DateTime.Now;
                        await _terminalEquipmentService.UpdateAsync(terminalEquipment);
                        return Ok();

                    case "del":
                        bool rec = await _terminalEquipmentService.DeleteAsync(newTerminalEquipment.id);
                        if (rec)
                        {
                            return Ok();
                        }
                        break;

                }

            }
            // ModelState.AddModelErrors(errors);
            return BadRequest(ModelState);
        }
    }
}
