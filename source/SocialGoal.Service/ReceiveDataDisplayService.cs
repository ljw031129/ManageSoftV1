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
    public interface IReceiveDataDisplayService
    {
        void Delete(ReceiveDataDisplay data);
        IEnumerable<ReceiveDataDisplay> GetDataByPmFInterpreterId(string id);
        IEnumerable<ReceiveDataDisplay> GetDataByPmFInterpreterByDevid(string devId);
        void Save();
        Task<bool> UpdateReceiveData(ReceiveDataDisplay rd);
    }
    public class ReceiveDataDisplayService : IReceiveDataDisplayService
    {
        private readonly IReDataDisplayFormatRepository _reDataDisplayFormatRepository;
        private readonly IReceiveDataDisplayRepository _receiveDataDisplayRepository;
        private readonly ITerminalEquipmentRepository _terminalEquipmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ReceiveDataDisplayService(ITerminalEquipmentRepository terminalEquipmentRepository, IReDataDisplayFormatRepository reDataDisplayFormatRepository, IReceiveDataDisplayRepository receiveDataDisplayRepository, IUnitOfWork unitOfWork)
        {
            this._terminalEquipmentRepository = terminalEquipmentRepository;
            this._receiveDataDisplayRepository = receiveDataDisplayRepository;
            this._reDataDisplayFormatRepository = reDataDisplayFormatRepository;
            this._unitOfWork = unitOfWork;
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public void Delete(ReceiveDataDisplay data)
        {
            _receiveDataDisplayRepository.Delete(_receiveDataDisplayRepository.GetById(data.ReceiveDataDisplayId));
            if (data.ReDataDisplayFormats != null)
            {
                foreach (var item in data.ReDataDisplayFormats)
                {
                    _reDataDisplayFormatRepository.Delete(_reDataDisplayFormatRepository.GetById(item.ReDataDisplayFormatId));

                }
            }

            Save();
        }


        public IEnumerable<ReceiveDataDisplay> GetDataByPmFInterpreterId(string id)
        {
            return _receiveDataDisplayRepository.GetDataByPmFInterpreterId(id);
        }


        public Task<bool> UpdateReceiveData(ReceiveDataDisplay rd)
        {
            if (_receiveDataDisplayRepository.CanUpdate(rd.ReceiveDataDisplayId))
            {
                _receiveDataDisplayRepository.Update(rd);
            }
            else
            {
                _receiveDataDisplayRepository.Add(rd);
            }
            if (rd.ReDataDisplayFormats != null && rd.ReDataDisplayFormats.Count() > 0)
            {
                List<ReDataDisplayFormat> rdList = rd.ReDataDisplayFormats.ToList();
                foreach (var item in rdList)
                {
                    if (_reDataDisplayFormatRepository.CanUpDate(item.ReDataDisplayFormatId))
                    {
                        _reDataDisplayFormatRepository.Update(item);
                    }
                    else
                    {
                        _reDataDisplayFormatRepository.Add(item);
                    }
                }
            }
            Save();
            return Task.FromResult(true);
        }


        public IEnumerable<ReceiveDataDisplay> GetDataByPmFInterpreterByDevid(string devId)
        {
            string id = _terminalEquipmentRepository.Get(t => t.TerminalEquipmentNum == devId).PmFInterpreterId;
            IEnumerable<ReceiveDataDisplay> rd = _receiveDataDisplayRepository.GetDataByPmFInterpreterId(id);
            return rd;
        }
    }
}
