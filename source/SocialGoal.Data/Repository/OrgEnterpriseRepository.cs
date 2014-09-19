using SocialGoal.Data.Infrastructure;
using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Data.Repository
{
    public class OrgEnterpriseRepository : RepositoryBase<OrgEnterprise>, IOrgEnterpriseRepository
    {
        public OrgEnterpriseRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface IOrgEnterpriseRepository : IRepository<OrgEnterprise>
    {
    }
}
