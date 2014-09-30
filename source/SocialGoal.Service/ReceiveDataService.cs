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


    public interface IReceiveDataService
    {
        Task<IQueryable<ReceiveData>> GetReceiveDataMapata(string devid, string dataRange, int pageNum, int pageSize, out int total);
    }
    public class ReceiveDataService : IReceiveDataService
    {
        private readonly IReceiveDataRepository _receiveDataRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ReceiveDataService(IReceiveDataRepository receiveDataRepository, IUnitOfWork unitOfWork)
        {
            this._receiveDataRepository = receiveDataRepository;
            this._unitOfWork = unitOfWork;
        }

        public Task<IQueryable<ReceiveData>> GetReceiveDataMapata(string devid, string dataRange, int pageNum, int pageSize, out int total)
        {
            IQueryable<ReceiveData> re = _receiveDataRepository.GetReceiveDataMapata(devid, dataRange, pageNum, pageSize, out  total);
            return Task.FromResult(re);
        }


    }
}
