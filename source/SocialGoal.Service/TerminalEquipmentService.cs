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
    public interface ITerminalEquipmentService
    {
        Task<IEnumerable<TerminalEquipment>> GetOrgStructures(Core.xFilter.Expressions.JqGridSetting jqGridSetting, out int count);
        void Save();

        Task CreateAsync(TerminalEquipment terminalEquipment);

        Task UpdateAsync(TerminalEquipment terminalEquipment);

        Task<bool> DeleteAsync(string p);

        void UpdateTerminalEquipmentOrgEnterpriseId(string OrgEnterpriseId, string TerminalEquipmentIds);

        Task<IEnumerable<TerminalEquipment>> GetSubGridByEquipmentId(Core.xFilter.Expressions.JqGridSetting jqGridSetting, out int count);
    }
    public class TerminalEquipmentService : ITerminalEquipmentService
    {
        private readonly ITerminalEquipmentRepository _terminalEquipmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TerminalEquipmentService(ITerminalEquipmentRepository terminalEquipmentRepository, IUnitOfWork unitOfWork)
        {
            this._terminalEquipmentRepository = terminalEquipmentRepository;
            this._unitOfWork = unitOfWork;
        }
        public Task<IEnumerable<TerminalEquipment>> GetOrgStructures(Core.xFilter.Expressions.JqGridSetting jqGridSetting, out int count)
        {
            IEnumerable<TerminalEquipment> terminalEquipment = _terminalEquipmentRepository.GetPageJqGrid<TerminalEquipment>(jqGridSetting, out count);
            return Task.FromResult(terminalEquipment);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }


        public Task CreateAsync(TerminalEquipment terminalEquipment)
        {
            _terminalEquipmentRepository.Add(terminalEquipment);
            Save();
            return Task.FromResult(true);
        }

        public Task UpdateAsync(TerminalEquipment terminalEquipment)
        {
            _terminalEquipmentRepository.Update(terminalEquipment);
            Save();
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(string id)
        {
            var orgEnterprise = _terminalEquipmentRepository.GetById(id);
            _terminalEquipmentRepository.Delete(orgEnterprise);
            Save();
            return Task.FromResult(true);
        }


        public void UpdateTerminalEquipmentOrgEnterpriseId(string OrgEnterpriseId, string TerminalEquipmentIds)
        {
            _terminalEquipmentRepository.UpdateTerminalEquipmentOrgEnterpriseId(OrgEnterpriseId, TerminalEquipmentIds);
        }


        public Task<IEnumerable<TerminalEquipment>> GetSubGridByEquipmentId(Core.xFilter.Expressions.JqGridSetting jqGridSetting, out int count)
        {
            //动态补充条件
            jqGridSetting._search = true;
            jqGridSetting.filters = "{\"groupOp\":\"AND\",\"rules\":[{\"field\":\"EquipmentId\",\"op\":\"eq\",\"data\":\"" + jqGridSetting.subRowId + "\"}]}";
            IEnumerable<TerminalEquipment> terminalEquipment = _terminalEquipmentRepository.GetPageJqGrid<TerminalEquipment>(jqGridSetting, out count);
            return Task.FromResult(terminalEquipment.Where(p => p.EquipmentId.Contains(jqGridSetting.subRowId)));
        }
    }
}
