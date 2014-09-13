using SocialGoal.Data.Infrastructure;
using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Data.Repository
{
    public class ReceiveDataDisplayRepository : RepositoryBase<ReceiveDataDisplay>, IReceiveDataDisplayRepository
    {
        public ReceiveDataDisplayRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }       
        public void DeleteByDisplayId(string p)
        {

            this.DataContext.Database.ExecuteSqlCommand("DELETE FROM ReDataDisplayFormats where ReceiveDataDisplayId='" + p + "'");
        }
        public IEnumerable<ReceiveDataDisplay> GetDataByPmFInterpreterId(string id)
        {
            return this.DataContext.ReceiveDataDisplays.Where(m => m.PmFInterpreterId == id);
        }


        public bool CanUpdate(string p)
        {
            if (this.DataContext.ReceiveDataDisplays.Where(pb => pb.ReceiveDataDisplayId == p).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public interface IReceiveDataDisplayRepository : IRepository<ReceiveDataDisplay>
    {
       
        void DeleteByDisplayId(string p);
        IEnumerable<ReceiveDataDisplay> GetDataByPmFInterpreterId(string id);

        bool CanUpdate(string p);
    }
}
