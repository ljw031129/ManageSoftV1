using AutoMapper;
using SocialGoal.Core.Common;
using SocialGoal.Core.xFilter.Expressions;
using SocialGoal.Model.Models;
using SocialGoal.Model.ViewModels;
using SocialGoal.Service;
using SocialGoal.Web.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SocialGoal.Controllers
{
    public class OrgEnterpriseController : Controller
    {
        private readonly IOrgEnterpriseService _orgEnterpriseService;
        public OrgEnterpriseController(IOrgEnterpriseService orgEnterpriseService)
        {
            this._orgEnterpriseService = orgEnterpriseService;
        }
        // GET: OrgEnterprise
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 采用JSONP方式加载select2数据
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNum"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async Task<JsonpResult> GetOrgEnterprises(int pageSize, int pageNum, string searchTerm)
        {
            Select2PagedResult orgEnterprises = await _orgEnterpriseService.GetSelect2PagedResult(searchTerm, pageSize, pageNum);
            //Return the data as a jsonp result
            return new JsonpResult
            {
                Data = orgEnterprises,
                JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet
            };
        }
        public async Task<string> GetAll()
        {
            StringBuilder st = new StringBuilder();
            IEnumerable<OrgEnterprise> re = await _orgEnterpriseService.GetAll();
            st.Append("<select>");
            foreach (var item in re)
            {
                st.Append("<option value='" + item.OrgEnterpriseId + "'>" + item.OrgEnterpriseName + "</option>");

            }
            st.Append("</select>"); ;
            return st.ToString();
        }
        public async Task<ActionResult> Get(JqGridSetting jqGridSetting)
        {
            int count = 0;
            IEnumerable<OrgEnterprise> orgEnterprise = await _orgEnterpriseService.GetOrgEnterprises(jqGridSetting, out count);
            var result = new
            {
                total = (int)Math.Ceiling((double)count / jqGridSetting.rows),
                page = jqGridSetting.page,
                records = count,
                rows = (from item in orgEnterprise
                        select new
                        {
                            OrgEnterpriseId = item.OrgEnterpriseId,
                            OrgEnterprisePId=item.OrgEnterprisePId,
                            OrgEnterpriseNum = item.OrgEnterpriseNum,
                            OrgEnterpriseName = item.OrgEnterpriseName,
                            OrgEnterpriseDescribe = item.OrgEnterpriseDescribe,
                            OrgEnterpriseUpdateTime = item.OrgEnterpriseUpdateTime,
                            OrgEnterpriseCreateTime = item.OrgEnterpriseCreateTime
                        }).ToArray()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult> Post(OrgEnterpriseViewModel newOrgEnterprise)
        {
            if (ModelState.IsValid)
            {

                OrgEnterprise orgEnterprise = Mapper.Map<OrgEnterpriseViewModel, OrgEnterprise>(newOrgEnterprise);
                switch (newOrgEnterprise.oper)
                {
                    case "add":
                       // orgEnterprise.OrgEnterpriseId = Guid.NewGuid().ToString();
                        orgEnterprise.OrgEnterpriseUpdateTime = DateTime.Now;
                        orgEnterprise.OrgEnterpriseCreateTime = DateTime.Now;
                        // var errors = _orgEnterpriseService.CanAdd(equipment).ToList();
                        await _orgEnterpriseService.CreateAsync(orgEnterprise);
                        return Json(new { success = true });

                    case "edit":
                        orgEnterprise.OrgEnterpriseUpdateTime = DateTime.Now;
                        orgEnterprise.OrgEnterpriseCreateTime = DateTime.Now;
                        await _orgEnterpriseService.UpdateAsync(orgEnterprise);
                        return Json(new { success = true });

                    case "del":
                        bool rec = await _orgEnterpriseService.DeleteAsync(newOrgEnterprise.id);
                        if (rec)
                        {
                            return Json(new { success = true });
                        }
                        break;

                }

            }
            // ModelState.AddModelErrors(errors);
            return Json(new { errors = GetErrorsFromModelState() });
        }
        [HttpPost]
        public async Task<ActionResult> PostUpdate(OrgEnterpriseViewModel newOrgEnterprise)
        {
            if (ModelState.IsValid)
            {

                OrgEnterprise orgEnterprise = Mapper.Map<OrgEnterpriseViewModel, OrgEnterprise>(newOrgEnterprise);
                switch (newOrgEnterprise.oper)
                {
                    case "add":
                        // orgEnterprise.OrgEnterpriseId = Guid.NewGuid().ToString();
                        orgEnterprise.OrgEnterpriseUpdateTime = DateTime.Now;
                        orgEnterprise.OrgEnterpriseCreateTime = DateTime.Now;
                        // var errors = _orgEnterpriseService.CanAdd(equipment).ToList();
                        await _orgEnterpriseService.CreateAsync(orgEnterprise);
                        return Json(new { success = true });

                    case "edit":
                        orgEnterprise.OrgEnterpriseUpdateTime = DateTime.Now;
                        orgEnterprise.OrgEnterpriseCreateTime = DateTime.Now;
                        await _orgEnterpriseService.UpdateAsync(orgEnterprise);
                        return Content("true");

                    case "del":
                        bool rec = await _orgEnterpriseService.DeleteAsync(newOrgEnterprise.id);
                        if (rec)
                        {
                            return Json(new { success = true });
                        }
                        break;

                }

            }
            // ModelState.AddModelErrors(errors);
            return Content("false");
        }
        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }
        [HttpPost]
        public async Task<Object> GetOrgEnterpriseZtree()
        {
            string userId = Request.Params["userId"];
            List<ZtreeEntity> orgStructure = await _orgEnterpriseService.GetOrgEnterpriseZtree(userId);


            return Json(orgStructure, JsonRequestBehavior.AllowGet);
        }

    }
}