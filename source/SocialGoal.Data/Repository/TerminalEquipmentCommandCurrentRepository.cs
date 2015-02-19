using SocialGoal.Data.Infrastructure;
using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Data.Repository
{

    public class TerminalEquipmentCommandCurrentRepository : RepositoryBase<TerminalEquipmentCommandCurrent>, ITerminalEquipmentCommandCurrentRepository
    {
        public TerminalEquipmentCommandCurrentRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }


    }
    public interface ITerminalEquipmentCommandCurrentRepository : IRepository<TerminalEquipmentCommandCurrent>
    {
    }
}
