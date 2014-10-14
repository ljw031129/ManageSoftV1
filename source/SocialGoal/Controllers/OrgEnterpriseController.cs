using SocialGoal.Core.Common;
using SocialGoal.Service;
using SocialGoal.Web.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}