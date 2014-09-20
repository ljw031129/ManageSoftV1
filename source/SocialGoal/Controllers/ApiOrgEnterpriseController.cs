using AutoMapper;
using PagedList;
using SocialGoal.Core.xFilter.Expressions;
using SocialGoal.Model.Models;
using SocialGoal.Service;
using SocialGoal.Web.Core.Models;
using SocialGoal.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SocialGoal.Controllers
{
    public class ApiOrgEnterpriseController : ApiController
    {
        private readonly IOrgEnterpriseService _orgEnterpriseService;
        public ApiOrgEnterpriseController(IOrgEnterpriseService orgEnterpriseService)
        {
            this._orgEnterpriseService = orgEnterpriseService;
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
