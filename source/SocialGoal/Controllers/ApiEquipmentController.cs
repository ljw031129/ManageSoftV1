using System;
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
using xFilter.Expressions;
using Newtonsoft.Json.Linq;

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

        public async Task<Object> Get(string gridSettings)
        {

            //{"IsSearch":true,"PageSize":2,"PageIndex":1,"SortColumn":"EquipmentUpDateTime","SortOrder":"ASC","Where":""}
            //可后台自动添加查询条件
            //xFilter.Expressions.Group g = new xFilter.Expressions.Group() { Operator = GroupOperator.And };
            //g.Rules.Add(new Rule() { Field = "Continent.Name", Operator = RuleOperator.Equals, Data = "E" });

            // Get a paged list of groups
            IPagedList<Equipment> equipments = await _equipmentService.GetEquipmentsAsync(gridSettings);

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
                Equipment equipment = Mapper.Map<EquipmentViewModel, Equipment>(newEquipment);
                switch (newEquipment.oper)
                {
                    case "add":
                        newEquipment.EquipmentId = Guid.NewGuid().ToString();
                        newEquipment.EquipmentUpDateTime = DateTime.Now;
                        newEquipment.EquipmentCreatTime = DateTime.Now;                       
                        var errors = _equipmentService.CanAddEquipment(equipment).ToList();
                        await _equipmentService.CreateEquipmentAsync(equipment, "");
                        return Ok();

                    case "edit":
                        newEquipment.EquipmentUpDateTime = DateTime.Now;                       
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