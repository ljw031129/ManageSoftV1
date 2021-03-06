﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PagedList;
using SocialGoal.Data.Infrastructure;
using SocialGoal.Model.Models;
using SocialGoal.Service;
using SocialGoal.Web.Core.Extensions;
using SocialGoal.Web.Core.Models;
using xFilter.Expressions;
using Newtonsoft.Json.Linq;
using SocialGoal.Core.xFilter.Expressions;
using SocialGoal.Model.ViewModels;

namespace SocialGoal.Web.API.Controllers
{
    /// <summary>
    /// 设备管理控制器
    /// </summary>
    public class ApiEquipmentController : ApiController
    {
        private readonly IEquipmentService _equipmentService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="equipmentService"></param>
        public ApiEquipmentController(IEquipmentService equipmentService)
        {
            this._equipmentService = equipmentService;
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="jqGridSetting"></param>
        /// <returns></returns>             
        public async Task<Object> Get([FromUri]JqGridSetting jqGridSetting)
        {
            int count = 0;
            IEnumerable<Equipment> equipments = await _equipmentService.GetEquipmentsJqGrid(jqGridSetting, out count);
            var data = Mapper.Map<IEnumerable<Equipment>, IEnumerable<EquipmentViewModel>>(equipments).ToArray();
            var result = new
            {
                total = (int)Math.Ceiling((double)count / jqGridSetting.rows),
                page = jqGridSetting.page,
                records = count,
                rows = (from item in data.ToList()
                        select new
                        {
                            EquipmentId = item.EquipmentId,
                            EquipmentTypeId = item.EquipmentTypeId,
                            EquipmentNum = item.EquipmentNum,
                            EquipmentName = item.EquipmentName,
                            EquipmentCreatTime = item.EquipmentCreatTime,
                            EquipmentUpDateTime = item.EquipmentUpDateTime
                        }).ToArray()
            };

            return result;
        }



        // POST api/<controller>
        /// <summary>
        /// 添加修改删除
        /// </summary>
        /// <param name="newEquipment"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> Post(EquipmentViewModel newEquipment)
        {
            if (ModelState.IsValid)
            {
                Equipment equipment = Mapper.Map<EquipmentViewModel, Equipment>(newEquipment);
                switch (newEquipment.oper)
                {
                    case "add":
                        equipment.EquipmentId = Guid.NewGuid().ToString();
                        equipment.EquipmentUpDateTime = DateTime.Now;
                        equipment.EquipmentCreatTime = DateTime.Now;
                        var errors = _equipmentService.CanAddEquipment(equipment).ToList();
                        await _equipmentService.CreateEquipmentAsync(equipment, "");
                        return Ok();

                    case "edit":
                        equipment.EquipmentUpDateTime = DateTime.Now;
                        await _equipmentService.UpdateEquipmentAsync(equipment);
                        return Ok();

                    case "del":
                        bool rec = await _equipmentService.DeleteEquipmentAsync(newEquipment.id);
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

        // PUT api/<controller>/5 UPDATE
        /// <summary>
        /// 暂未使用
        /// </summary>
        /// <param name="newEquipment"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> Put(EquipmentViewModel newEquipment)
        {
            Equipment equipment = Mapper.Map<EquipmentViewModel, Equipment>(newEquipment);
            var errors = _equipmentService.CanAddEquipment(equipment).ToList();

            if (ModelState.IsValid)
            {
                //group.UserId = ((SocialGoalUser)(User.Identity)).UserId;
                var createdGroup = await _equipmentService.CreateEquipmentAsync(equipment, "");
                //var createdGroup = groupService.GetGroup(newGroup.GroupName);
                //var groupAdmin = new GroupUser { GroupId = createdGroup.GroupId, UserId = ((SocialGoalUser)(User.Identity)).UserId, Admin = true };
                //groupUserService.CreateGroupUser(groupAdmin, groupInvitationService);
                return Ok();
            }
            ModelState.AddModelError("error", "发生异常");
            return BadRequest(ModelState);
        }

        // DELETE api/<controller>/5
        /// <summary>
        /// 暂未使用
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> Delete(string equipmentId)
        {
            bool rec = await _equipmentService.DeleteEquipmentAsync(equipmentId);
            if (rec)
            {
                return Ok();
            }
            ModelState.AddModelError("error", "发生异常");
            return BadRequest(ModelState);
        }
    }
}