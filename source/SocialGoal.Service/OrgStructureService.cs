using SocialGoal.Core.Common;
using SocialGoal.Data.Infrastructure;
using SocialGoal.Data.Repository;
using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Service
{   
    public interface IOrgStructureService
    {
        Task<IEnumerable<OrgStructure>> GetOrgStructures(Core.xFilter.Expressions.JqGridSetting jqGridSetting, out int count);
        Task<List<DynatreeNode>> GetOrgStructuresByUserId(string userId);
        void Save();
    }
    public class OrgStructureService : IOrgStructureService
    {
        private readonly IOrgStructureRepository _orgStructureRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrgStructureService(IOrgStructureRepository orgStructureRepository, IUnitOfWork unitOfWork)
        {
            this._orgStructureRepository = orgStructureRepository;
            this._unitOfWork = unitOfWork;
        }
        public Task<IEnumerable<OrgStructure>> GetOrgStructures(Core.xFilter.Expressions.JqGridSetting jqGridSetting, out int count)
        {
            IEnumerable<OrgStructure> orgStructure = _orgStructureRepository.GetPageJqGrid<OrgStructure>(jqGridSetting, out count);
            return Task.FromResult(orgStructure);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }


        public Task<List<DynatreeNode>> GetOrgStructuresByUserId(string userId)
        {
            string orgStructureId = "0";
            List<OrgStructure> orgList = _orgStructureRepository.GetAll().ToList();
            List<DynatreeNode> dList = new List<DynatreeNode>();
            OrgStructureTree(orgList, orgStructureId, dList, orgStructureId);
            return Task.FromResult(dList);
        }

        private void OrgStructureTree(List<OrgStructure> orgList, string parentId, List<DynatreeNode> node, string corgStructureId)
        {
            List<OrgStructure> rows;
            if (string.IsNullOrEmpty(parentId))
            {

                rows = orgList.Where(t => t.OrgStructurePId == "null").ToList(); //过滤
            }
            else
            {
                rows = parentId == corgStructureId ? orgList.Where(t => t.OrgStructureId == parentId).ToList() : orgList.Where(t => t.OrgStructurePId == parentId).ToList();
            }
            // rows = ds.Tables[0].Select("ID='" + parentId + "'"); //过滤
            foreach (OrgStructure row in rows)
            {
                List<OrgStructure> childern = orgList.Where(t => t.OrgStructurePId == row.OrgStructureId).ToList();//用于判断是否有子节点            

                if (childern.Count != 0 || parentId == "")//是父节点            
                {
                    DynatreeNode nodeList = new DynatreeNode();
                    nodeList.title = row.OrgStructureName;
                    nodeList.tooltip = row.OrgStructureName;
                    nodeList.key = row.OrgStructureId;


                    nodeList.children = new List<DynatreeNode>();
                    OrgStructureTree(orgList, row.OrgStructureId, nodeList.children, "");
                    node.Add(nodeList);
                }
                else
                {
                    DynatreeNode nodeCh = new DynatreeNode();
                    nodeCh.title = row.OrgStructureName;
                    nodeCh.tooltip = row.OrgStructureName;
                    nodeCh.key = row.OrgStructureId;

                    node.Add(nodeCh);
                }               
            }
        }
    }
}
