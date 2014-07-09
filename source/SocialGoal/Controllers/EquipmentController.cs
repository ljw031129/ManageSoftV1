using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using SocialGoal.Model.Models;
using SocialGoal.Models;
using SocialGoal.Service;
using SocialGoal.Web.Core.Extensions;
using SocialGoal.Web.ViewModels;

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
            var allEquipments = _equipmentService.GetEquipments();
            ViewBag.JsonData = JsonConvert.SerializeObject(allEquipments);
            return View(allEquipments);
        }

        public string Get()
        {
            var allEquipments = _equipmentService.GetEquipments();
            return JsonConvert.SerializeObject(allEquipments);
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

