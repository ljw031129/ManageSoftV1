using SocialGoal.Data.Infrastructure;
using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Data.Repository
{
    public class TerminalSimCardRepository : RepositoryBase<TerminalSimCard>, ITerminalSimCardRepository
    {
        public TerminalSimCardRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface ITerminalSimCardRepository : IRepository<TerminalSimCard>
    {
    }
}
