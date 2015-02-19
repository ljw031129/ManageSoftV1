using SocialGoal.Data.Infrastructure;
using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Utilities;

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

                long startTime = DateUtils.ConvertDateTimeIntInt(DateTime.Parse(dataTime[0]));
                long endTime = DateUtils.ConvertDateTimeIntInt(DateTime.Parse(dataTime[1]));
                total = this.DataContext.ReceiveDatas.OrderBy(d => d.GpsTime).Where(p => p.IMEI.Contains(devid) && p.GpsTime >= startTime && p.GpsTime < endTime).Count();
                return this.DataContext.ReceiveDatas.OrderBy(d => d.GpsTime).Where(p => p.IMEI.Contains(devid) && p.GpsTime >= startTime && p.GpsTime < endTime).Skip((pageNum - 1) * pageSize).Take(pageSize);
            }
            else
            {
                total = this.DataContext.ReceiveDatas.OrderBy(d => d.GpsTime).Where(p => p.IMEI.Contains(devid)).Count();
                return this.DataContext.ReceiveDatas.OrderBy(d => d.GpsTime).Where(p => p.IMEI.Contains(devid)).Skip((pageNum - 1) * pageSize).Take(pageSize);
            }
        }


    }
    public interface IReceiveDataRepository : IRepository<ReceiveData>
    {
        IQueryable<ReceiveData> GetReceiveDataMapata(string devid, string dataRange, int pageNum, int pageSize, out int total);
    }
}
