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
using SocialGoal.Web.ViewModels;

namespace SocialGoal.Controllers
{
    public class ApiEquipmentController : ApiController
    {
        private readonly IEquipmentService _equipmentService;
        public ApiEquipmentController(IEquipmentService equipmentService)
        {
            this._equipmentService = equipmentService;
        }
        // GET api/<controller>
        public IEnumerable<Equipment> Get()
        {
            var allEquipments = _equipmentService.GetEquipments();
            if (allEquipments == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return allEquipments;
        }

        public async Task<Object> Get(string page, string sort, string serach)
        {
            int currentPage = Convert.ToInt32(page.Split(':')[0]);
            int pageSize = Convert.ToInt32(page.Split(':')[1]);
            // Get a paged list of groups
            IPagedList<Equipment> equipments = await _equipmentService.GetEquipmentsAsync(new Page(currentPage, pageSize));

            // map it to a paged list of models.
            var equipmentsViewModel = Mapper.Map<IPagedList<Equipment>, IPagedList<EquipmentViewModel>>(equipments);

            if (equipmentsViewModel.Count == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }

            PagedListData pagedListMetaData = Mapper.Map<IPagedList, PagedListData>(equipmentsViewModel.GetMetaData());
            //处理分页对象
            List<PageNum> pageNumsList = new List<PageNum>();
            for (int i = 1; i <= pagedListMetaData.PageCount; i++)
            {
                var pageNum = new PageNum { PageCount = i };
                pageNumsList.Add(pageNum);

            }
            return new { pageModel = equipmentsViewModel, pager = pagedListMetaData, pageNums = pageNumsList };
        }



        // POST api/<controller>
        [HttpPost]
        public async Task<IHttpActionResult> Post(EquipmentViewModel newEquipment)
        {
            if (ModelState.IsValid)
            {
                if (newEquipment.EquipmentId == "")
                {
                    newEquipment.EquipmentId = Guid.NewGuid().ToString();
                    newEquipment.EquipmentUpDateTime = DateTime.Now;
                    newEquipment.EquipmentCreatTime = DateTime.Now;
                    Equipment equipment = Mapper.Map<EquipmentViewModel, Equipment>(newEquipment);
                    var errors = _equipmentService.CanAddEquipment(equipment).ToList();
                    await _equipmentService.CreateEquipmentAsync(equipment, "");
                    return Ok();
                }
                else
                {
                    newEquipment.EquipmentUpDateTime = DateTime.Now;
                    Equipment equipment = Mapper.Map<EquipmentViewModel, Equipment>(newEquipment);
                    await _equipmentService.UpdateEquipmentAsync(equipment);
                    return Ok();
                }
            }
            // ModelState.AddModelErrors(errors);
            return BadRequest(ModelState);
        }

        // PUT api/<controller>/5 UPDATE
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