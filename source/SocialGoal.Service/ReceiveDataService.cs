using SocialGoal.Data.Infrastructure;
using SocialGoal.Data.Repository;
using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xFilter.Expressions;

namespace SocialGoal.Service
{


    public interface IReceiveDataService
    {
        Task<IQueryable<ReceiveData>> GetReceiveDataMapata(string devid, string dataRange, int pageNum, int pageSize, out int total);
        Task<IEnumerable<ReceiveData>> GetReceiveDataHistory(Core.xFilter.Expressions.JqGridSetting jqGridSetting, out int count);
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




        public Task<IEnumerable<ReceiveData>> GetReceiveDataHistory(Core.xFilter.Expressions.JqGridSetting jqGridSetting, out int count)
        {
            //动态补充条件           
            xFilter.Expressions.Group g = new xFilter.Expressions.Group();
            Rule dr = new Rule();
            dr.Data = jqGridSetting.devId;
            dr.Field = "DevId";
            dr.Operator = xFilter.Expressions.RuleOperator.Equals;
            g.Rules.Add(dr);
            jqGridSetting.Where = g;
            IEnumerable<ReceiveData> re = _receiveDataRepository.GetPageJqGrid<ReceiveData>(jqGridSetting, out count);
            return Task.FromResult(re);
        }
    }
}
