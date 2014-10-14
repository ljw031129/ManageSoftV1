using AutoMapper;
using PagedList;
using SocialGoal.Core.Common;
using SocialGoal.Core.xFilter.Expressions;
using SocialGoal.Model.Models;
using SocialGoal.Service;
using SocialGoal.Web.Core.Extensions;
using SocialGoal.Web.Core.Models;
using SocialGoal.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SocialGoal.Web.API.Controllers
{
    public class ApiOrgEnterpriseController : ApiController
    {
        private readonly IOrgEnterpriseService _orgEnterpriseService;
        public ApiOrgEnterpriseController(IOrgEnterpriseService orgEnterpriseService)
        {
            this._orgEnterpriseService = orgEnterpriseService;
        }
        [HttpGet]
        //[Route("api/ApiTerminalSimCard/{terminalEquipmentId}")]
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
        [Route("api/ApiOrgEnterprise/GetAll")]
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
        /// <summary>
        /// 采用JSON方式加载select2数据
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNum"></param>
        /// <param name="searchTerm"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("api/ApiOrgEnterprise/GetOrgEnterprisesSelect")]
        public async Task<Object> GetOrgEnterprisesSelect(int pageSize, int pageNum, string searchTerm, string userId)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) searchTerm = "";
            Select2PagedResult orgEnterprises = await _orgEnterpriseService.GetSelect2PagedResult(searchTerm, pageSize, pageNum);
            //Return the data as a jsonp result

            return orgEnterprises;
        }
        public async Task<Object> Get([FromUri]JqGridSetting jqGridSetting)
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
                            OrgEnterpriseNum = item.OrgEnterpriseNum,
                            OrgEnterpriseName = item.OrgEnterpriseName,
                            OrgEnterpriseDescribe = item.OrgEnterpriseDescribe,
                            OrgEnterpriseUpdateTime = item.OrgEnterpriseUpdateTime,
                            OrgEnterpriseCreateTime = item.OrgEnterpriseCreateTime
                        }).ToArray()
            };
            return result;
        }



        // POST api/<controller>
        [HttpPost]
        public async Task<IHttpActionResult> Post(OrgEnterpriseViewModel newOrgEnterprise)
        {
            if (ModelState.IsValid)
            {

                OrgEnterprise orgEnterprise = Mapper.Map<OrgEnterpriseViewModel, OrgEnterprise>(newOrgEnterprise);
                switch (newOrgEnterprise.oper)
                {
                    case "add":
                        orgEnterprise.OrgEnterpriseId = Guid.NewGuid().ToString();
                        orgEnterprise.OrgEnterpriseUpdateTime = DateTime.Now;
                        orgEnterprise.OrgEnterpriseCreateTime = DateTime.Now;
                        // var errors = _orgEnterpriseService.CanAdd(equipment).ToList();
                        await _orgEnterpriseService.CreateAsync(orgEnterprise);
                        return Ok();

                    case "edit":
                        orgEnterprise.OrgEnterpriseUpdateTime = DateTime.Now;
                        await _orgEnterpriseService.UpdateAsync(orgEnterprise);
                        return Ok();

                    case "del":
                        bool rec = await _orgEnterpriseService.DeleteAsync(newOrgEnterprise.id);
                        if (rec)
                        {
                            return Ok();
                        }
                        break;

                }

            }
            // ModelState.AddModelErrors(errors);
            return BadRequest(ModelState);
        }
    }
}
