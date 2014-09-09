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

            return pmFInterpreter.PmDataBodys;
        }
    }
    public interface IPmDataBodiesRepository : IRepository<PmDataBody>
    {
        IEnumerable<PmDataBody> GePmDataBody(string pmId);
    }
}
