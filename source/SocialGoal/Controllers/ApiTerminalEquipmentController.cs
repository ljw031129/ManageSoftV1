using SocialGoal.Core.xFilter.Expressions;
using SocialGoal.Model.Models;
using SocialGoal.Service;
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
                rows = (from item in orgStructure
                        select new
                        {
                            TerminalEquipmentId = item.TerminalEquipmentId,
                            TerminalEquipmentNum = item.TerminalEquipmentNum,
                            TerminalEquipmentType = item.TerminalEquipmentType,
                            ProtocolName = item.PmFInterpreter.ProtocolName,
                            TerminalSimCardNum = item.TerminalSimCard.TerminalSimCardNum,
                            TerminalEquipmentCreateTime = item.TerminalEquipmentCreateTime,
                            TerminalEquipmentUpdateTime = item.TerminalEquipmentUpdateTime
                        }).ToArray()
            };
            return result;
        }
    }
}
