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
                rows = (from item in data
                        select new
                        {
                            EquipmentId = item.EquipmentId,
                            EquipmentNum = item.EquipmentNum,
                            EquipmentName = item.EquipmentName,
                            EquipmentCreatTime = item.EquipmentCreatTime,
                            EquipmentUpDateTime = item.EquipmentUpDateTime
                        }).ToArray()
            };

            return Json(result, JsonRequestBehavior.AllowGet);
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

