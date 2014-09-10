using SocialGoal.Data.Infrastructure;
using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Data.Repository
{
    public class PmDataBytesRepository : RepositoryBase<PmDataByte>, IPmDataBytesRepository
    {
        public PmDataBytesRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public bool CanUpdate(string p)
        {
            if (this.DataContext.PmDataBytes.Where(pb => pb.PmDataByteId == p).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public interface IPmDataBytesRepository : IRepository<PmDataByte>
    {

        bool CanUpdate(string p);
    }
}
