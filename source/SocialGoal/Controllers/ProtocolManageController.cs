
using PagedList;
using SocialGoal.Model.Models;
using SocialGoal.Model.ViewModels;
using SocialGoal.Service;
using SocialGoal.Web.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SocialGoal.Controllers
{
    [Authorize]
    public class ProtocolManageController : Controller
    {

        private readonly IPmDataBitsService _pmDataBitsService;
        private readonly IProtocolManageService _protocolManageService;
        private readonly IPmDataBodiesService _pmDataBodiesServic;
        private readonly IReceiveDataDisplayService _receiveDataDisplayService;
        private readonly IReDataDisplayFormatService _reDataDisplayFormatService;
        public ProtocolManageController(IReDataDisplayFormatService reDataDisplayFormatService, IProtocolManageService protocolManageService, IPmDataBodiesService pmDataBodiesServic, IPmDataBitsService pmDataBitsService, IReceiveDataDisplayService receiveDataDisplayService)
        {
            this._reDataDisplayFormatService = reDataDisplayFormatService;
            this._protocolManageService = protocolManageService;
            this._pmDataBodiesServic = pmDataBodiesServic;
            this._pmDataBitsService = pmDataBitsService;
            this._receiveDataDisplayService = receiveDataDisplayService;
        }

        // GET: ProtocolManage
        public ActionResult Index()
        {
           

            return View();
        }
        public string GetAllList()
        {
            StringBuilder st = new StringBuilder();
            IEnumerable<PmFInterpreter> re = _protocolManageService.GetPmFInterpreter();
            st.Append("<select>");
            foreach (var item in re)
            {
                st.Append("<option value='" + item.PmFInterpreterId + "'>" + item.ProtocolName + "</option>");

            }
            st.Append("</select>"); ;
            return st.ToString();
        }
        public JsonResult GetAll()
        {
            var prList = _protocolManageService.GetPmFInterpreter();

            return Json(prList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPropertyInfoArrayByReceiveDataLast()
        {
            string[] rec = new string[66];
            var proPerty = _protocolManageService.GetPropertyInfoArray();
            for (int i = 0; i < proPerty.Count(); i++)
            {
                rec[i] = proPerty[i].Name;
            }

            return Json(rec, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateDataBody(PmDataBody pmdataBody)
        {
            var rec = _pmDataBodiesServic.UpdatePmDataBodyAsync(pmdataBody);
            return Json(rec.Result);
        }

        [HttpPost]
        public JsonResult UpdateReceiveData(ReceiveDataDisplay rd)
        {
            var rec = _receiveDataDisplayService.UpdateReceiveData(rd);
            return Json(rec.Result);
        }
        //解析协议记录
        public JsonResult GetPmDataBodyById(string pmId)
        {
            var prList = _pmDataBodiesServic.GePmDataBody(pmId);

            return Json(prList, JsonRequestBehavior.AllowGet);
        }
        //显示协议记录
        public JsonResult GetReceiveDataDisplayByPmFInterpreterId(string pmId)
        {
            var prList = _receiveDataDisplayService.GetDataByPmFInterpreterId(pmId);

            return Json(prList.ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeletePmDataBits(PmDataBit pBit)
        {
            _pmDataBitsService.Delete(pBit);

            return Json(true);
        }

        [HttpPost]
        public JsonResult DeleteReDataDisplayFormat(ReDataDisplayFormat rf)
        {
            _reDataDisplayFormatService.Delete(rf);
            return Json(true);
        }
        [HttpPost]
        public JsonResult DeletePmDataBody(PmDataBody pDataBody)
        {
            _pmDataBodiesServic.Delete(pDataBody);

            return Json(true);
        }
        public JsonResult DeleteReceiveDataDisplay(ReceiveDataDisplay pd)
        {
            _receiveDataDisplayService.Delete(pd);

            return Json(true);
        }
        public JsonResult TestProtocol(string pmId, string sendData)
        {
            ProtocolTestViewModel pv = _protocolManageService.TestProtocol(pmId, sendData);
            return Json(pv, JsonRequestBehavior.AllowGet);
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
            //  var allEquipments = _equipmentService.GetIQueryableAll();


            return Json("");
        }
        public async Task<JsonResult> Get(JQueryDataTableParamModel param)
        {
            string gridSettings = "{\"IsSearch\":true,\"PageSize\":2,\"PageIndex\":1,\"SortColumn\":\"EquipmentUpDateTime\",\"SortOrder\":\"ASC\",\"Where\":\"\"}";
            //可后台自动添加查询条件
            //xFilter.Expressions.Group g = new xFilter.Expressions.Group() { Operator = GroupOperator.And };
            //g.Rules.Add(new Rule() { Field = "Continent.Name", Operator = RuleOperator.Equals, Data = "E" });

            // Get a paged list of groups
            //  IPagedList<Equipment> equipments = await _equipmentService.GetEquipmentsAsync(gridSettings);
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = 50,
                iTotalDisplayRecords = 50,
                //   aaData = equipments
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
