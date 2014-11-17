using SocialGoal.Core.Common;
using SocialGoal.Core.xFilter.Expressions;
using SocialGoal.Model.Models;
using SocialGoal.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SocialGoal.Web.API.Controllers
{
    public class ApiOrgStructureController : ApiController
    {
        private readonly IOrgStructureService _orgStructureService;
        public ApiOrgStructureController(IOrgStructureService orgStructureService)
        {
            this._orgStructureService = orgStructureService;
        }

        public async Task<Object> Get([FromUri]JqGridSetting jqGridSetting)
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
                            OrgStructureCreateTime = item.OrgStructureCreateTime,
                            
                        }).ToArray()
            };
            return result;
        }
        [Route("api/ApiOrgStructure/GetOrgStructureTree/{userId}")]
        public async Task<Object> GetOrgStructureTree(string userId)
        {
            List<DynatreeNode> orgStructure = await _orgStructureService.GetOrgStructuresByUserId(userId);
            return orgStructure;
        }

        [Route("api/ApiOrgStructure/GetOrgStructureZtree/{userId}")]
        public async Task<Object> GetOrgStructureZtree(string userId)
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
                              OrgStructureCreateTime = item.OrgStructureCreateTime,
                              //level = item.level,
                              //parent = item.parent,
                              //isLeaf = item.isLeaf,
                              //expanded = item.expanded,
                              //loaded = item.loaded,
                              //icon = item.icon
                          }).ToArray();
            return result;
        }
    }
}
