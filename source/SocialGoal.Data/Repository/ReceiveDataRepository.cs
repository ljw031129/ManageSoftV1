using SocialGoal.Data.Infrastructure;
using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Data.Repository
{


    public class ReceiveDataRepository : RepositoryBase<ReceiveData>, IReceiveDataRepository
    {
        public ReceiveDataRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
        public IQueryable<ReceiveData> GetReceiveDataMapata(string devid, string dataRange, int pageNum, int pageSize, out int total)
        {
            total = 0;
            string[] dataTime = dataRange.Split('$');
            if (dataTime.Length == 2)
            {
                DateTime startTime = DateTime.Parse(dataTime[0]);
                DateTime endTime = DateTime.Parse(dataTime[1]);
                return this.DataContext.ReceiveDatas.OrderBy(d => d.ReceiveTime).Where(p => p.DevId.Contains(devid) && p.ReceiveTime >= startTime && p.ReceiveTime < endTime).Skip((pageNum - 1) * pageSize).Take(pageSize);
            }
            else
            {
                return this.DataContext.ReceiveDatas.OrderBy(d => d.ReceiveTime).Where(p => p.DevId.Contains(devid)).Skip((pageNum - 1) * pageSize).Take(pageSize);

            }


        }


    }
    public interface IReceiveDataRepository : IRepository<ReceiveData>
    {
        IQueryable<ReceiveData> GetReceiveDataMapata(string devid, string dataRange, int pageNum, int pageSize, out int total);
    }
}
