﻿using PagedList;
using SocialGoal.Core.Common;
using SocialGoal.Data.Infrastructure;
using SocialGoal.Data.Repository;
using SocialGoal.Model.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Service
{
    public interface IOrgEnterpriseService
    {
        Task<PagedList.IPagedList<OrgEnterprise>> GetOrgEnterprisesAsync(string gridSettings);

        IEnumerable<ValidationResult> CanAdd(OrgEnterprise equipment);

        Task CreateAsync(OrgEnterprise equipment);

        Task UpdateAsync(OrgEnterprise equipment);

        Task<bool> DeleteAsync(string p);

        Task<IEnumerable<OrgEnterprise>> GetOrgEnterprises(Core.xFilter.Expressions.JqGridSetting jqGridSetting, out int count);
        void Save();

        OrgEnterprise GetById(string id);

        Task<Select2PagedResult> GetSelect2PagedResult(List<String> al, string searchTerm, int pageSize, int pageNum);

        Task<IEnumerable<OrgEnterprise>> GetAll();

        Task<List<ZtreeEntity>> GetOrgEnterpriseZtree(string userId);
        Task<string[]> GetOrgEnterpriseArraylist(string userId);
        Task<List<OrgEnterprise>> GetOrgEnterprisesList(string userId);

        Task<string> GetAllTree();

        OrgEnterprise GetOrgEnterpriseByUserId(string userId);
    }
    public class OrgEnterpriseService : IOrgEnterpriseService
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IOrgEnterpriseRepository _orgEnterpriseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrgEnterpriseService(IOrgEnterpriseRepository orgEnterpriseRepository, IApplicationUserRepository applicationUserRepository, IUnitOfWork unitOfWork)
        {
            this._applicationUserRepository = applicationUserRepository;
            this._orgEnterpriseRepository = orgEnterpriseRepository;
            this._unitOfWork = unitOfWork;
        }

        public Task<PagedList.IPagedList<OrgEnterprise>> GetOrgEnterprisesAsync(string gridSettings)
        {
            IPagedList<OrgEnterprise> ipagPagedList = _orgEnterpriseRepository.GetPage<OrgEnterprise>(gridSettings);
            return Task.FromResult(ipagPagedList);
        }


        public IEnumerable<ValidationResult> CanAdd(OrgEnterprise orgEnterprise)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(OrgEnterprise orgEnterprise)
        {
            _orgEnterpriseRepository.Add(orgEnterprise);
            Save();
            return Task.FromResult(true);
        }

        public Task UpdateAsync(OrgEnterprise orgEnterprise)
        {
            _orgEnterpriseRepository.Update(orgEnterprise);
            Save();
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(string id)
        {
            var orgEnterprise = _orgEnterpriseRepository.GetById(id);
            _orgEnterpriseRepository.Delete(orgEnterprise);
            _orgEnterpriseRepository.Delete(gu => gu.OrgEnterpriseId == id);
            Save();
            return Task.FromResult(true);
        }


        public Task<IEnumerable<OrgEnterprise>> GetOrgEnterprises(Core.xFilter.Expressions.JqGridSetting jqGridSetting, out int count)
        {
            IEnumerable<OrgEnterprise> equipments = _orgEnterpriseRepository.GetPageJqGrid<OrgEnterprise>(jqGridSetting, out count);
            return Task.FromResult(equipments);
        }


        public void Save()
        {
            _unitOfWork.Commit();
        }


        public OrgEnterprise GetById(string id)
        {
            return _orgEnterpriseRepository.GetById(id);
        }


        public Task<Select2PagedResult> GetSelect2PagedResult(List<String> al, string searchTerm, int pageSize, int pageNum)
        {
            int reTotal = 0;
            IEnumerable<OrgEnterprise> orgEnterprise = _orgEnterpriseRepository.GetSelect2((t => al.Contains(t.OrgEnterpriseId) && t.OrgEnterpriseName.Contains(searchTerm)), "OrgEnterpriseUpdateTime", true, pageSize, pageNum, out reTotal);
            Select2PagedResult jsonAttendees = new Select2PagedResult();
            jsonAttendees.Results = new List<Select2Result>();

            //Loop through our attendees and translate it into a text value and an id for the select list
            foreach (OrgEnterprise a in orgEnterprise.ToList())
            {
                jsonAttendees.Results.Add(new Select2Result { id = a.OrgEnterpriseId.ToString(), text = a.OrgEnterpriseName });
            }
            //Set the total count of the results from the query.
            jsonAttendees.Total = reTotal;
            return Task.FromResult(jsonAttendees);
        }


        public Task<IEnumerable<OrgEnterprise>> GetAll()
        {
            IEnumerable<OrgEnterprise> orgEnterprise = _orgEnterpriseRepository.GetAll();
            return Task.FromResult(orgEnterprise);
        }


        public Task<List<ZtreeEntity>> GetOrgEnterpriseZtree(string userId)
        {
            string orgId = _applicationUserRepository.Get(m => m.Id == userId).OrgEnterpriseId;
            //当前用户所在企业ID           
            List<OrgEnterprise> orgList = _orgEnterpriseRepository.GetAll().ToList();
            List<ZtreeEntity> dList = new List<ZtreeEntity>();
            OrgEnterpriseTree(orgList, orgId, dList, orgId);
            return Task.FromResult(dList);
        }
        private void OrgEnterpriseTree(List<OrgEnterprise> orgList, string parentId, List<ZtreeEntity> node, string corgStructureId)
        {
            List<OrgEnterprise> rows;
            if (string.IsNullOrEmpty(parentId))
            {
                rows = orgList.Where(t => t.OrgEnterprisePId == "null").ToList(); //过滤
            }
            else
            {
                rows = parentId == corgStructureId ? orgList.Where(t => t.OrgEnterpriseId == parentId).ToList() : orgList.Where(t => t.OrgEnterprisePId == parentId).ToList();
            }
            // rows = ds.Tables[0].Select("ID='" + parentId + "'"); //过滤
            foreach (OrgEnterprise row in rows)
            {
                List<OrgEnterprise> childern = orgList.Where(t => t.OrgEnterpriseId == row.OrgEnterpriseId).ToList();//用于判断是否有子节点            

                if (childern.Count != 0 || parentId == "")//是父节点            
                {
                    ZtreeEntity nodeList = new ZtreeEntity();
                    nodeList.name = row.OrgEnterpriseName;
                    nodeList.id = row.OrgEnterpriseId;
                    nodeList.PID = row.OrgEnterprisePId;
                    nodeList.OrgEnterpriseCreateTime = row.OrgEnterpriseCreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    nodeList.OrgEnterpriseUpdateTime = row.OrgEnterpriseCreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    nodeList.OrgEnterpriseNum = row.OrgEnterpriseNum;
                    nodeList.OrgEnterpriseDescribe = row.OrgEnterpriseDescribe;
                    nodeList.open = false;
                    nodeList.title = row.OrgEnterpriseName;
                    nodeList.children = new List<ZtreeEntity>();
                    OrgEnterpriseTree(orgList, row.OrgEnterpriseId, nodeList.children, "");
                    node.Add(nodeList);
                }
                else
                {
                    ZtreeEntity nodeCh = new ZtreeEntity();
                    nodeCh.name = row.OrgEnterpriseName;
                    nodeCh.id = row.OrgEnterpriseId;
                    nodeCh.PID = row.OrgEnterprisePId;
                    nodeCh.open = false;
                    nodeCh.title = row.OrgEnterpriseName;
                    node.Add(nodeCh);
                }
                GC.Collect();
            }
        }


        public Task<string[]> GetOrgEnterpriseArraylist(string userId)
        {
            string orgId = _applicationUserRepository.Get(m => m.Id == userId).OrgEnterpriseId;
            //当前用户所在企业ID           
            List<OrgEnterprise> orgList = _orgEnterpriseRepository.GetAll().ToList();
            List<ZtreeEntity> dList = new List<ZtreeEntity>();
            ArrayList al = new ArrayList();
            OrgEnterpriseArrayList(orgList, orgId, dList, orgId, al);
            string[] array = al.ToArray(typeof(string)) as string[];
            return Task.FromResult(array);
        }
        private void OrgEnterpriseArrayList(List<OrgEnterprise> orgList, string parentId, List<ZtreeEntity> node, string corgStructureId, ArrayList al)
        {
            List<OrgEnterprise> rows;
            if (string.IsNullOrEmpty(parentId))
            {
                rows = orgList.Where(t => t.OrgEnterprisePId == "null").ToList(); //过滤
            }
            else
            {
                rows = parentId == corgStructureId ? orgList.Where(t => t.OrgEnterpriseId == parentId).ToList() : orgList.Where(t => t.OrgEnterprisePId == parentId).ToList();
            }
            // rows = ds.Tables[0].Select("ID='" + parentId + "'"); //过滤
            foreach (OrgEnterprise row in rows)
            {
                List<OrgEnterprise> childern = orgList.Where(t => t.OrgEnterpriseId == row.OrgEnterpriseId).ToList();//用于判断是否有子节点            

                if (childern.Count != 0 || parentId == "")//是父节点            
                {
                    ZtreeEntity nodeList = new ZtreeEntity();
                    nodeList.name = row.OrgEnterpriseName;
                    nodeList.id = row.OrgEnterpriseId;
                    nodeList.PID = row.OrgEnterprisePId;
                    nodeList.OrgEnterpriseCreateTime = row.OrgEnterpriseCreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    nodeList.OrgEnterpriseUpdateTime = row.OrgEnterpriseCreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    nodeList.OrgEnterpriseNum = row.OrgEnterpriseNum;
                    nodeList.OrgEnterpriseDescribe = row.OrgEnterpriseDescribe;
                    nodeList.open = false;
                    nodeList.title = row.OrgEnterpriseName;
                    nodeList.children = new List<ZtreeEntity>();
                    OrgEnterpriseArrayList(orgList, row.OrgEnterpriseId, nodeList.children, "", al);
                    node.Add(nodeList);
                    al.Add(row.OrgEnterpriseId);
                }
                else
                {
                    ZtreeEntity nodeCh = new ZtreeEntity();
                    nodeCh.name = row.OrgEnterpriseName;
                    nodeCh.id = row.OrgEnterpriseId;
                    nodeCh.PID = row.OrgEnterprisePId;
                    nodeCh.open = false;
                    nodeCh.title = row.OrgEnterpriseName;
                    node.Add(nodeCh);
                    al.Add(row.OrgEnterpriseId);
                }
                GC.Collect();
            }
        }



        public Task<List<OrgEnterprise>> GetOrgEnterprisesList(string userId)
        {
            string orgId = _applicationUserRepository.Get(m => m.Id == userId).OrgEnterpriseId;
            //当前用户所在企业ID           
            List<OrgEnterprise> orgList = _orgEnterpriseRepository.GetAll().ToList();
            List<ZtreeEntity> dList = new List<ZtreeEntity>();
            List<OrgEnterprise> al = new List<OrgEnterprise>();
            OrgEnterpriseList(orgList, orgId, dList, orgId, al);
            return Task.FromResult(al);
        }
        private void OrgEnterpriseList(List<OrgEnterprise> orgList, string parentId, List<ZtreeEntity> node, string corgStructureId, List<OrgEnterprise> al)
        {
            List<OrgEnterprise> rows;
            if (string.IsNullOrEmpty(parentId))
            {
                rows = orgList.Where(t => t.OrgEnterprisePId == "null").ToList(); //过滤
            }
            else
            {
                rows = parentId == corgStructureId ? orgList.Where(t => t.OrgEnterpriseId == parentId).ToList() : orgList.Where(t => t.OrgEnterprisePId == parentId).ToList();
            }
            // rows = ds.Tables[0].Select("ID='" + parentId + "'"); //过滤
            foreach (OrgEnterprise row in rows)
            {
                List<OrgEnterprise> childern = orgList.Where(t => t.OrgEnterpriseId == row.OrgEnterpriseId).ToList();//用于判断是否有子节点            

                if (childern.Count != 0 || parentId == "")//是父节点            
                {
                    ZtreeEntity nodeList = new ZtreeEntity();
                    nodeList.name = row.OrgEnterpriseName;
                    nodeList.id = row.OrgEnterpriseId;
                    nodeList.PID = row.OrgEnterprisePId;
                    nodeList.OrgEnterpriseCreateTime = row.OrgEnterpriseCreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    nodeList.OrgEnterpriseUpdateTime = row.OrgEnterpriseCreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    nodeList.OrgEnterpriseNum = row.OrgEnterpriseNum;
                    nodeList.OrgEnterpriseDescribe = row.OrgEnterpriseDescribe;
                    nodeList.open = false;
                    nodeList.title = row.OrgEnterpriseName;
                    nodeList.children = new List<ZtreeEntity>();
                    OrgEnterpriseList(orgList, row.OrgEnterpriseId, nodeList.children, "", al);
                    node.Add(nodeList);
                    OrgEnterprise org = new OrgEnterprise();
                    org.OrgEnterpriseId = row.OrgEnterpriseId;
                    org.OrgEnterpriseName = row.OrgEnterpriseName;
                    org.OrgEnterpriseNum = row.OrgEnterpriseNum;
                    al.Add(org);
                }
                else
                {
                    ZtreeEntity nodeCh = new ZtreeEntity();
                    nodeCh.name = row.OrgEnterpriseName;
                    nodeCh.id = row.OrgEnterpriseId;
                    nodeCh.PID = row.OrgEnterprisePId;
                    nodeCh.open = false;
                    nodeCh.title = row.OrgEnterpriseName;
                    node.Add(nodeCh);
                    OrgEnterprise org = new OrgEnterprise();
                    org.OrgEnterpriseId = row.OrgEnterpriseId;
                    org.OrgEnterpriseName = row.OrgEnterpriseName;
                    org.OrgEnterpriseNum = row.OrgEnterpriseNum;
                    al.Add(org);
                }
                GC.Collect();
            }
        }




        public Task<string> GetAllTree()
        {
            throw new NotImplementedException();
        }


        public OrgEnterprise GetOrgEnterpriseByUserId(string userId)
        {
            string orgId = _applicationUserRepository.Get(m => m.Id == userId).OrgEnterpriseId;
            return _orgEnterpriseRepository.Get(p => p.OrgEnterpriseId == orgId);
        }
    }
}
