using SocialGoal.Core.Common;
using SocialGoal.Core.xFilter.Expressions;
using SocialGoal.Model.Models;
using SocialGoal.Model.ViewModels;
using SocialGoal.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SocialGoal.Controllers
{
    public class OrgStructureController : Controller
    {
        private readonly IOrgStructureService _orgStructureService;
        public OrgStructureController(IOrgStructureService orgStructureService)
        {
            this._orgStructureService = orgStructureService;
        }
        public async Task<ActionResult> Get(JqGridSetting jqGridSetting)
        {
            int count = 0;
            IEnumerable<OrgStructure> orgStructure = await _orgStructureService.GetOrgStructures(jqGridSetting, out count);
            var result = new
            {
                total = (int)Math.Ceiling((double)count / jqGridSetting.rows),
                page = jqGridSetting.page,
                records = count,
                rows = (from item in orgStructure
                        select new
                        {
                           
                            OrgStructureId = item.OrgStructureId,
                            OrgStructurePId = item.OrgStructurePId,
                            OrgStructureNum = item.OrgStructureNum,
                            OrgStructureName = item.OrgStructureName,
                            OrgStructureDescribe = item.OrgStructureDescribe,
                            OrgStructureUpdateTime = item.OrgStructureUpdateTime,
                            OrgStructureCreateTime = item.OrgStructureCreateTime
                            
                        }).ToArray()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<Object> GetOrgStructureTree(string userId)
        {
            List<DynatreeNode> orgStructure = await _orgStructureService.GetOrgStructuresByUserId(userId);
            return orgStructure;
        }


        public async Task<ActionResult> GetOrgStructureZtree(string userId)
        {
            IEnumerable<OrgStructure> orgStructure = await _orgStructureService.GetOrgStructuresZtree(userId);

            var result = (from item in orgStructure
                          select new
                          {
                              OrgStructureId = item.OrgStructureId,
                              OrgStructurePId = item.OrgStructurePId,
                              OrgStructureNum = item.OrgStructureNum,
                              OrgStructureName = item.OrgStructureName,
                              OrgStructureDescribe = item.OrgStructureDescribe,
                              OrgStructureUpdateTime = item.OrgStructureUpdateTime,
                              OrgStructureCreateTime = item.OrgStructureCreateTime
                             
                          }).ToArray();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: OrgStructure
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Create(OrgStructureViewModel org)
        {
            return Content("true");
        }
        public ActionResult Edit(string id)
        {
            ViewBag.Id = id;
            return PartialView();
        }
        [HttpPost]
        public JsonResult Edit(OrgStructureViewModel org)
        {
            return Json(true);
        }

        private ActionResult ContextDependentView()
        {
            string actionName = ControllerContext.RouteData.GetRequiredString("action");
            if (Request.QueryString["content"] != null)
            {
                ViewBag.FormAction = "Json" + actionName;
                return PartialView();
            }
            else
            {
                ViewBag.FormAction = actionName;
                return View();
            }
        }
    }
}