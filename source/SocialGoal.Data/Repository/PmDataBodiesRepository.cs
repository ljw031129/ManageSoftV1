using SocialGoal.Data.Infrastructure;
using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Data.Repository
{
    public class PmDataBodiesRepository : RepositoryBase<PmDataBody>, IPmDataBodiesRepository
    {
        public PmDataBodiesRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IEnumerable<PmDataBody> GePmDataBody(string pmId)
        {

            PmFInterpreter pmFInterpreter = this.DataContext.PmFInterpreters.Find(pmId);
            if (pmFInterpreter != null)
            {
                return pmFInterpreter.PmDataBodys;
            }
            return null;

        }


        public bool CanUpdate(string p)
        {
            if (this.DataContext.PmDataBodys.Where(pb => pb.PmDataBodyId == p).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public PmFInterpreter GetPmFInterpreterById(string pmId)
        {
           return this.DataContext.PmFInterpreters.Find(pmId);
        }
    }
    public interface IPmDataBodiesRepository : IRepository<PmDataBody>
    {
        IEnumerable<PmDataBody> GePmDataBody(string pmId);

        bool CanUpdate(string p);

        PmFInterpreter GetPmFInterpreterById(string pmId);
    }
}
