using PagedList;
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
    }
    public class OrgEnterpriseService : IOrgEnterpriseService
    {
        private readonly IOrgEnterpriseRepository _orgEnterpriseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrgEnterpriseService(IOrgEnterpriseRepository orgEnterpriseRepository, IUnitOfWork unitOfWork)
        {
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
    }
}
