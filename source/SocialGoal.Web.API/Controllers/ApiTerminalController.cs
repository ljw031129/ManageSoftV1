﻿using SocialGoal.Core.xFilter.Expressions;
using SocialGoal.Model.Models;
using SocialGoal.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SocialGoal.Web.API.Controllers
{
    public class ApiTerminalController : ApiController
    {
        private readonly ITerminalEquipmentService _terminalEquipmentService;
        public ApiTerminalController(ITerminalEquipmentService terminalEquipmentService)
        {
            this._terminalEquipmentService = terminalEquipmentService;
        }

        [Route("api/ApiTerminal/GetTerminalEquipmentDetail")]
        public async Task<Object> GetTerminalEquipmentDetail([FromUri]JqGridSetting jqGridSetting)
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
                            // TotalWorkTime = item.ReceiveDataLast != null ? item.ReceiveDataLast.TotalWorkTime.ToString() : "",
                            // ReceiveTime = item.ReceiveDataLast.ReceiveTime != null ? item.ReceiveDataLast.ReceiveTime.ToString() : "",
                            //  AccStatus = item.ReceiveDataLast.AccStatus != null ? item.ReceiveDataLast.AccStatus.ToString() : "",
                            // GpsPos = item.ReceiveDataLast.GpsPos != null ? item.ReceiveDataLast.GpsPos.ToString() : ""
                        }).ToArray()
            };
            return result;

        }
    }
}
