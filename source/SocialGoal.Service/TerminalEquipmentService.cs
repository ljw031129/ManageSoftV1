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
    public interface ITerminalEquipmentService
    {
        Task<IEnumerable<TerminalEquipment>> GetOrgStructures(Core.xFilter.Expressions.JqGridSetting jqGridSetting, out int count);
        void Save();

        Task CreateAsync(TerminalEquipment terminalEquipment);

        Task UpdateAsync(TerminalEquipment terminalEquipment);

        Task<bool> DeleteAsync(string p);

        void UpdateTerminalEquipmentOrgEnterpriseId(string OrgEnterpriseId, string TerminalEquipmentIds);

        Task<IEnumerable<TerminalEquipment>> GetSubGridByEquipmentId(Core.xFilter.Expressions.JqGridSetting jqGridSetting, out int count);

        Task<Core.Common.Select2PagedResult> GetSelect2PagedResult(string searchTerm, int pageSize, int pageNum);

        void UpdateEquipmentId(string TerminalEquipmentId, string EquipmentIds);
    }
    public class TerminalEquipmentService : ITerminalEquipmentService
    {
        private readonly IReceiveDataLastRepository _receiveDataLastRepository;
         private readonly ITerminalEquipmentRepository _terminalEquipmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TerminalEquipmentService(ITerminalEquipmentRepository terminalEquipmentRepository, IUnitOfWork unitOfWork, IReceiveDataLastRepository receiveDataLastRepository)
        {
            this._terminalEquipmentRepository = terminalEquipmentRepository;
            this._receiveDataLastRepository = receiveDataLastRepository;
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
            string guid = Guid.NewGuid().ToString();
            terminalEquipment.ReceiveDataLastId = guid;
            _terminalEquipmentRepository.Add(terminalEquipment);
            ReceiveDataLast reLat = new ReceiveDataLast();
            reLat.ReceiveDataLastId = guid;
            reLat.DevId = terminalEquipment.TerminalEquipmentNum;
            _receiveDataLastRepository.Add(reLat);
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
            _receiveDataLastRepository.Delete(_receiveDataLastRepository.GetById(orgEnterprise.ReceiveDataLastId));
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


        public Task<Core.Common.Select2PagedResult> GetSelect2PagedResult(string searchTerm, int pageSize, int pageNum)
        {
            int reTotal = 0;
            IEnumerable<TerminalEquipment> terminalEquipment = _terminalEquipmentRepository.GetSelect2(t => t.TerminalEquipmentNum.Contains(searchTerm), "TerminalEquipmentUpdateTime", true, pageSize, pageNum, out reTotal);

            Select2PagedResult jsonAttendees = new Select2PagedResult();
            jsonAttendees.Results = new List<Select2Result>();

            //Loop through our attendees and translate it into a text value and an id for the select list
            foreach (TerminalEquipment a in terminalEquipment.ToList())
            {
                jsonAttendees.Results.Add(new Select2Result { id = a.TerminalEquipmentId, text = a.TerminalEquipmentNum });
            }
            //Set the total count of the results from the query.
            jsonAttendees.Total = reTotal;
            return Task.FromResult(jsonAttendees);
        }


        public void UpdateEquipmentId(string TerminalEquipmentId, string EquipmentIds)
        {
            _terminalEquipmentRepository.UpdateEquipmentId(TerminalEquipmentId, EquipmentIds);
        }
    }
}
