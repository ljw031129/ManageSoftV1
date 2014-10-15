using SocialGoal.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialGoal.Controllers
{
    public class OrgStructureController : Controller
    {
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