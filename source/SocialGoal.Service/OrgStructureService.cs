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
    }
}
