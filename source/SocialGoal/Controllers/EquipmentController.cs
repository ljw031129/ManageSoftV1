using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using SocialGoal.Model.Models;
using SocialGoal.Models;
using SocialGoal.Service;
using SocialGoal.Web.Core.Extensions;
using SocialGoal.Model.ViewModels;
using SocialGoal.Core.xFilter.Expressions;
using SocialGoal.Web.Core.Models;
using PagedList;
using System.Net;
using System.Threading.Tasks;

namespace SocialGoal.Controllers
{
    public class EquipmentController : Controller
    {
        private readonly IEquipmentService _equipmentService;


        // If you are using Dependency Injection, you can delete the following constructor
        public EquipmentController(IEquipmentService equipmentService)
        {
            this._equipmentService = equipmentService;
        }
        //
        // GET: /Equipment/

        public ViewResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="jqGridSetting"></param>
        /// <returns></returns>             
        public async Task<JsonResult> Get(JqGridSetting jqGridSetting)
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

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        // POST api/<controller>
        /// <summary>
        /// 添加修改删除
        /// </summary>
        /// <param name="newEquipment"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post(EquipmentViewModel newEquipment)
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
                        return Json(new { success = true });

                    case "edit":
                        equipment.EquipmentUpDateTime = DateTime.Now;
                        await _equipmentService.UpdateEquipmentAsync(equipment);
                        return Json(new { success = true });

                    case "del":
                        bool rec = await _equipmentService.DeleteEquipmentAsync(newEquipment.id);
                        if (rec)
                        {
                            return Json(new { success = true });
                        }
                        break;
                }
            }
            // ModelState.AddModelErrors(errors);
            HttpContext.Response.StatusCode = 400;
            return Json(new { success = false, errors = GetErrorsFromModelState() });
        }
        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }

        public PartialViewResult Create()
        {
            var euipmentViewModel = new EquipmentViewModel();
            return PartialView(euipmentViewModel);
        }
        [HttpPost]
        public ActionResult Create(EquipmentViewModel newEquipment)
        {

            Equipment equipment = Mapper.Map<EquipmentViewModel, Equipment>(newEquipment);
            var errors = _equipmentService.CanAddEquipment(equipment).ToList();
            ModelState.AddModelErrors(errors);
            if (ModelState.IsValid)
            {
                //group.UserId = ((SocialGoalUser)(User.Identity)).UserId;
                var createdGroup = _equipmentService.CreateEquipment(equipment, "");
                //var createdGroup = groupService.GetGroup(newGroup.GroupName);
                //var groupAdmin = new GroupUser { GroupId = createdGroup.GroupId, UserId = ((SocialGoalUser)(User.Identity)).UserId, Admin = true };
                //groupUserService.CreateGroupUser(groupAdmin, groupInvitationService);
                return RedirectToAction("Index");
            }
            return View("Create", newEquipment);
        }


    }
}

