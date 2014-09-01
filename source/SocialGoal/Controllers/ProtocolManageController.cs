
using PagedList;
using SocialGoal.Model.Models;
using SocialGoal.Service;
using SocialGoal.Web.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SocialGoal.Controllers
{
    public class ProtocolManageController : Controller
    {
        private readonly IEquipmentService _equipmentService;
        public ProtocolManageController(IEquipmentService equipmentService)
        {
            this._equipmentService = equipmentService;
        }

        // GET: ProtocolManage
        public ActionResult Index()
        {


            return View();
        }

        // GET: ProtocolManage/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProtocolManage/Create
        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public JsonResult All()
        {
            var allEquipments = _equipmentService.GetIQueryableAll();


            return Json("");
        }
        public async Task<JsonResult> Get(JQueryDataTableParamModel param)
        {
            string gridSettings = "{\"IsSearch\":true,\"PageSize\":2,\"PageIndex\":1,\"SortColumn\":\"EquipmentUpDateTime\",\"SortOrder\":\"ASC\",\"Where\":\"\"}";
            //可后台自动添加查询条件
            //xFilter.Expressions.Group g = new xFilter.Expressions.Group() { Operator = GroupOperator.And };
            //g.Rules.Add(new Rule() { Field = "Continent.Name", Operator = RuleOperator.Equals, Data = "E" });

            // Get a paged list of groups
            IPagedList<Equipment> equipments = await _equipmentService.GetEquipmentsAsync(gridSettings);
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = 50,
                iTotalDisplayRecords = 50,
                aaData = equipments
            }, JsonRequestBehavior.AllowGet);
        }

        // POST: ProtocolManage/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ProtocolManage/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProtocolManage/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ProtocolManage/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProtocolManage/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
