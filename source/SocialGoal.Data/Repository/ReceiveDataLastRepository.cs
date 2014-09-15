using SocialGoal.Data.Infrastructure;
using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Data.Repository
{

    public class ReceiveDataLastRepository : RepositoryBase<ReceiveDataLast>, IReceiveDataLastRepository
    {
        public ReceiveDataLastRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
        public ReceiveDataLast GetReceiveDataLastByTerminalNum(string p)
        {
            return this.DataContext.ReceiveDataLasts.Where(l => l.DevId == p).FirstOrDefault();
        }
    }
    public interface IReceiveDataLastRepository : IRepository<ReceiveDataLast>
    {

        ReceiveDataLast GetReceiveDataLastByTerminalNum(string p);


    }
}
