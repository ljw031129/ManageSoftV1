using SocialGoal.Core.Common;
using SocialGoal.Core.DynamicLINQ;
using SocialGoal.Core.xFilter.Expressions;
using SocialGoal.Data.Infrastructure;
using SocialGoal.Data.Repository;
using SocialGoal.Model.Models;
using SocialGoal.Model.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xFilter.Expressions;

namespace SocialGoal.Service
{
    public interface ITerminalEquipmentService
    {
        Task<IEnumerable<TerminalEquipment>> GetOrgStructures(JqGridSetting jqGridSetting, out int count);
        void Save();

        Task CreateAsync(TerminalEquipment terminalEquipment);

        Task UpdateAsync(TerminalEquipment terminalEquipment);

        Task<bool> DeleteAsync(string p);

        void UpdateTerminalEquipmentOrgEnterpriseId(string OrgEnterpriseId, string TerminalEquipmentIds);

        Task<IEnumerable<TerminalEquipment>> GetSubGridByEquipmentId(Core.xFilter.Expressions.JqGridSetting jqGridSetting, out int count);

        Task<Core.Common.Select2PagedResult> GetSelect2PagedResult(List<String> orgLs, string searchTerm, int pageSize, int pageNum);

        void UpdateEquipmentId(string TerminalEquipmentId, string EquipmentIds);

        IEnumerable<ValidationResult> Validate(Model.ViewModels.TerminalEquipmentViewModel newTerminalEquipment);

        Task<TerminalEquipment> FindById(string id);

        Task<IEnumerable<TerminalEquipment>> GetSubGridByEquipmentId(string id);

        List<string> GetCurrentUserTerminalEquipments(string[] orgId);

        Task<IEnumerable<TerminalEquipment>> GetTerminalEquipments(JqSearchIn jqGridSetting, List<string> al, out int count);

        List<Model.ViewModels.SelectIdText> GetSelect2DefaultByEquipmentId(string id);

        void UpdateEquipmentId(string TerminalEquipmentId);

        Task<IEnumerable<TerminalEquipment>> GetTerminalEquipmentDataHistory(string terminalEquipmentNum, JqSearchIn jqGridSetting, out int count);
    }
    public class TerminalEquipmentService : ITerminalEquipmentService
    {
        private readonly IReceiveDataLastRepository _receiveDataLastRepository;
        private readonly ITerminalEquipmentRepository _terminalEquipmentRepository;
        private readonly ITerminalSimCardRepository _terminalSimCardRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TerminalEquipmentService(ITerminalSimCardRepository terminalSimCardRepository, ITerminalEquipmentRepository terminalEquipmentRepository, IUnitOfWork unitOfWork, IReceiveDataLastRepository receiveDataLastRepository)
        {
            this._terminalSimCardRepository = terminalSimCardRepository;
            this._terminalEquipmentRepository = terminalEquipmentRepository;
            this._receiveDataLastRepository = receiveDataLastRepository;
            this._unitOfWork = unitOfWork;
        }
        public Task<IEnumerable<TerminalEquipment>> GetOrgStructures(JqGridSetting jqGridSetting, out int count)
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
            //更新卡状态
            _terminalSimCardRepository.UpdateStatue(terminalEquipment.TerminalSimCardId, "2");
            string guid = Guid.NewGuid().ToString();
            terminalEquipment.ReceiveDataLastId = guid;
            _terminalEquipmentRepository.Add(terminalEquipment);
            ReceiveDataLast reLat = new ReceiveDataLast();
            reLat.ReceiveDataLastId = guid;
            reLat.IMEI = terminalEquipment.TerminalEquipmentNum;
            _receiveDataLastRepository.Add(reLat);
            Save();
            return Task.FromResult(true);
        }

        public Task UpdateAsync(TerminalEquipment terminalEquipment)
        {
            TerminalEquipment oldData = _terminalEquipmentRepository.GetById(terminalEquipment.TerminalEquipmentId);
            if (oldData.TerminalSimCardId != terminalEquipment.TerminalSimCardId)
            {
                //弃用旧卡，使用新卡
                _terminalSimCardRepository.UpdateStatue(oldData.TerminalSimCardId, "3");
                _terminalSimCardRepository.UpdateStatue(terminalEquipment.TerminalSimCardId, "2");
            }
            try
            {
                _terminalEquipmentRepository.Update(terminalEquipment);
                ReceiveDataLast reLat = _receiveDataLastRepository.GetById(terminalEquipment.ReceiveDataLastId);
                reLat.IMEI = terminalEquipment.TerminalEquipmentNum;
                _receiveDataLastRepository.Update(reLat);
                Save();
            }
            catch (Exception ex)
            {

                throw;
            }

            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(string id)
        {
            var orgEnterprise = _terminalEquipmentRepository.GetById(id);
            //更新SIM卡状态
            _terminalSimCardRepository.UpdateStatue(orgEnterprise.TerminalSimCardId, "4");
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
            xFilter.Expressions.Group g = new xFilter.Expressions.Group();
            Rule dr = new Rule();
            dr.Data = jqGridSetting.subRowId;
            dr.Field = "EquipmentId";
            dr.Operator = xFilter.Expressions.RuleOperator.Equals;
            g.Rules.Add(dr);
            jqGridSetting.Where = g;
            IEnumerable<TerminalEquipment> terminalEquipment = _terminalEquipmentRepository.GetPageJqGrid<TerminalEquipment>(jqGridSetting, out count);
            return Task.FromResult(terminalEquipment.Where(p => p.EquipmentId.Contains(jqGridSetting.subRowId)));
        }


        public Task<Core.Common.Select2PagedResult> GetSelect2PagedResult(List<String> orgLs, string searchTerm, int pageSize, int pageNum)
        {
            int reTotal = 0;
            IEnumerable<TerminalEquipment> terminalEquipment = _terminalEquipmentRepository.GetSelect2(t => orgLs.Contains(t.OrgEnterpriseId) && t.TerminalEquipmentNum.Contains(searchTerm) && t.EquipmentId == null, "TerminalEquipmentUpdateTime", true, pageSize, pageNum, out reTotal);

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


        public IEnumerable<ValidationResult> Validate(Model.ViewModels.TerminalEquipmentViewModel newTerminalEquipment)
        {
            TerminalEquipment eExists = null;
            switch (newTerminalEquipment.oper)
            {
                case "add":
                    eExists = _terminalEquipmentRepository.Get(c => c.TerminalEquipmentNum == newTerminalEquipment.TerminalEquipmentNum);
                    break;
                case "edit":
                    eExists = _terminalEquipmentRepository.Get(c => c.TerminalEquipmentNum == newTerminalEquipment.TerminalEquipmentNum && c.TerminalEquipmentId != newTerminalEquipment.TerminalEquipmentId);
                    break;
                default:
                    break;
            }
            if (eExists != null)
            {
                yield return new ValidationResult("Name", "终端设备编号已存在");
            }
        }
        public Task<TerminalEquipment> FindById(string id)
        {
            TerminalEquipment te = _terminalEquipmentRepository.GetById(id);
            return Task.FromResult(te);
        }


        public Task<IEnumerable<TerminalEquipment>> GetSubGridByEquipmentId(string id)
        {
            IEnumerable<TerminalEquipment> terminalEquipment = _terminalEquipmentRepository.GetTerminalEquipmentByEquipmentId(id);
            return Task.FromResult(terminalEquipment);
        }


        public List<string> GetCurrentUserTerminalEquipments(string[] orgIds)
        {
            IEnumerable<TerminalEquipment> te = _terminalEquipmentRepository.GetMany(m => orgIds.Contains(m.OrgEnterpriseId));
            List<string> al = new List<string>();
            foreach (var item in te.ToList())
            {
                al.Add(item.TerminalEquipmentNum.Trim());
            }
            return al;
        }


        public Task<IEnumerable<TerminalEquipment>> GetTerminalEquipments(JqSearchIn jqGridSetting, List<string> al, out int count)
        {
            IEnumerable<TerminalEquipment> terminalEquipment = _terminalEquipmentRepository.GetJqGrid(jqGridSetting, al, out count);
            return Task.FromResult(terminalEquipment);
        }


        public List<Model.ViewModels.SelectIdText> GetSelect2DefaultByEquipmentId(string id)
        {
            List<TerminalEquipment> tls = _terminalEquipmentRepository.GetMany(t => t.EquipmentId == id).ToList();
            List<SelectIdText> lst = new List<SelectIdText>();
            foreach (var item in tls)
            {
                SelectIdText st = new SelectIdText();
                st.id = item.TerminalEquipmentId;
                st.text = item.TerminalEquipmentNum;
                lst.Add(st);
            }
            return lst;
        }


        public void UpdateEquipmentId(string TerminalEquipmentId)
        {
            _terminalEquipmentRepository.UpdateEquipmentId(TerminalEquipmentId);
        }


        public Task<IEnumerable<TerminalEquipment>> GetTerminalEquipmentDataHistory(string terminalEquipmentNum, JqSearchIn jqGridSetting, out int count)
        {
            IEnumerable<TerminalEquipment> terminalEquipment = _terminalEquipmentRepository.GetJqGridDataHistory(terminalEquipmentNum,jqGridSetting, out count);
            return Task.FromResult(terminalEquipment);
        }
    }
}
