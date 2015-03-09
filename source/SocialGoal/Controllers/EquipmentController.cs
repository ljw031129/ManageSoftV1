using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using SocialGoal.Model.Models;
using SocialGoal.Service;
using SocialGoal.Web.Core.Extensions;
using SocialGoal.Model.ViewModels;
using SocialGoal.Core.xFilter.Expressions;
using SocialGoal.Web.Core.Models;
using PagedList;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Web.Script.Serialization;

namespace SocialGoal.Controllers
{
    [Authorize]
    public class EquipmentController : Controller
    {
        private readonly IEquipmentService _equipmentService;
        private readonly IOrgEnterpriseService _orgEnterpriseService;
        private readonly ITerminalEquipmentService _terminalEquipmentService;
        // If you are using Dependency Injection, you can delete the following constructor
        public EquipmentController(ITerminalEquipmentService terminalEquipmentService, IEquipmentService equipmentService, IOrgEnterpriseService orgEnterpriseService)
        {
            this._orgEnterpriseService = orgEnterpriseService;
            this._equipmentService = equipmentService;
            this._terminalEquipmentService = terminalEquipmentService;
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
            //得到当前用户可见企业id
            string userId = User.Identity.GetUserId();
            string[] al = await _orgEnterpriseService.GetOrgEnterpriseArraylist(userId);
            List<string> st = _equipmentService.GetCurrentUserEquipments(al);
            int count = 0;
            IEnumerable<Equipment> equipments = await _equipmentService.GetEquipmentsJqGridByCurrentUser(jqGridSetting, st, out count);
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
                            TerminalEquipmentCount = _terminalEquipmentService.GetSelect2DefaultByEquipmentId(item.EquipmentId).Count(),
                            EquipmentTypeId = item.EquipmentTypeId,
                            EquipmentNum = item.EquipmentNum,
                            EquipmentName = item.EquipmentName,
                            EngineNum = item.EngineNum,
                            OwnerName = item.OwnerName,
                            OwnerPhone = item.OwnerPhone,
                            OwnerAddress = item.OwnerAddress,
                            InstallTime = item.InstallTime,
                            InstallUser = item.InstallUser,
                            InstallUserPhone = item.InstallUserPhone,
                            InstallPlace = item.InstallPlace,
                            InstallSite = item.InstallSite,
                            EquipmentCreatTime = item.EquipmentCreatTime,
                            EquipmentUpDateTime = item.EquipmentUpDateTime,



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
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            bool rec = await _equipmentService.DeleteEquipmentAsync(id);
            if (rec)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }

        public ActionResult Create()
        {
            string userId = User.Identity.GetUserId();
            OrgEnterprise al = _orgEnterpriseService.GetOrgEnterpriseByUserId(userId);
            return View(new EquipmentViewModel()
            {
                OrgEnterpriseId = al.OrgEnterpriseId,
                OrgEnterpriseName = al.OrgEnterpriseName,
            });
        }
        [HttpPost]
        public ActionResult Create(EquipmentViewModel newEquipment)
        {

            Equipment equipment = Mapper.Map<EquipmentViewModel, Equipment>(newEquipment);
            equipment.EquipmentId = Guid.NewGuid().ToString();
            equipment.OrgEnterpriseId = newEquipment.OrgEnterpriseIdSelect2;
            equipment.EquipmentCreatTime = DateTime.Now;
            equipment.EquipmentUpDateTime = DateTime.Now;
            var errors = _equipmentService.CanAddEquipment(equipment).ToList();
            ModelState.AddModelErrors(errors);
            if (ModelState.IsValid)
            {
                var createdGroup = _equipmentService.CreateEquipment(equipment, "");

                var teList = newEquipment.TerminalEquipmentIdSelect2.Split(',');
                foreach (var item in teList)
                {
                    _terminalEquipmentService.UpdateEquipmentId(item, equipment.EquipmentId);
                }

                return RedirectToAction("Index");
            }
            string userId = User.Identity.GetUserId();
            OrgEnterprise al = _orgEnterpriseService.GetOrgEnterpriseByUserId(userId);
            newEquipment.OrgEnterpriseId = al.OrgEnterpriseId;
            newEquipment.OrgEnterpriseName = al.OrgEnterpriseName;
            return View("Create", newEquipment);
        }
        public async Task<ActionResult> Edit(string id)
        {
            //Get the list of Roles
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = await _equipmentService.FindById(id);
            List<SelectIdText> ls = _terminalEquipmentService.GetSelect2DefaultByEquipmentId(id);
            ViewBag.TerminalEquipmentIdDefault = new JavaScriptSerializer().Serialize(ls);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(new EquipmentViewModel()
            {
                EquipmentId = item.EquipmentId,
                EquipmentTypeId = item.EquipmentTypeId,
                EquipmentNum = item.EquipmentNum,
                EquipmentName = item.EquipmentName,
                OwnerName = item.OwnerName,
                OwnerPhone = item.OwnerPhone,
                OwnerAddress = item.OwnerAddress,
                InstallTime = item.InstallTime,
                InstallUser = item.InstallUser,
                InstallUserPhone = item.InstallUserPhone,
                InstallPlace = item.InstallPlace,
                InstallSite = item.InstallSite,
                //企业信息
                OrgEnterpriseId = item.OrgEnterprise.OrgEnterpriseId,
                OrgEnterpriseIdSelect2 = item.OrgEnterprise.OrgEnterpriseId,
                OrgEnterpriseName = item.OrgEnterprise.OrgEnterpriseName,
                EquipmentCreatTime = item.EquipmentCreatTime,
                EquipmentUpDateTime = item.EquipmentUpDateTime
            });
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EquipmentViewModel newEquipment)
        {
            Equipment equipment = Mapper.Map<EquipmentViewModel, Equipment>(newEquipment);
            equipment.EquipmentUpDateTime = DateTime.Now;
            equipment.OrgEnterpriseId = newEquipment.OrgEnterpriseIdSelect2;
            var errors = _equipmentService.CanAddEquipment(equipment).ToList();
            ModelState.AddModelErrors(errors);
            if (ModelState.IsValid)
            {
                var createdGroup = _equipmentService.UpdateEquipmentAsync(equipment);
                var teList = newEquipment.TerminalEquipmentIdSelect2.Split(',');
                foreach (var item in teList)
                {
                    _terminalEquipmentService.UpdateEquipmentId(item, equipment.EquipmentId);
                }
                return RedirectToAction("Index");
            }
            var equipmentModel = await _equipmentService.FindById(newEquipment.EquipmentId);

            List<SelectIdText> ls = _terminalEquipmentService.GetSelect2DefaultByEquipmentId(newEquipment.EquipmentId);
            ViewBag.TerminalEquipmentIdDefault = new JavaScriptSerializer().Serialize(ls);
            newEquipment.OrgEnterpriseId = equipmentModel.OrgEnterprise.OrgEnterpriseId;
            newEquipment.OrgEnterpriseIdSelect2 = equipmentModel.OrgEnterprise.OrgEnterpriseId;
            newEquipment.OrgEnterpriseName = equipmentModel.OrgEnterprise.OrgEnterpriseName;
            return View("Edit", newEquipment);
        }

        public ActionResult Monitor()
        {

            return View();
        }
        public ActionResult Map(string id)
        {
            ViewBag.DevId = id;
            return View();
        }

    }
}

