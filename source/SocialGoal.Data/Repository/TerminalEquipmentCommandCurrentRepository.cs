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

        public IEnumerable<TerminalEquipmentCommandCurrent> GetTerminalEquipmenttCurrentCommands(List<String> ls, int currentPage, int numPerPage, out int count)
        {
            count = this.DataContext.TerminalEquipmentCommandCurrents.Where(t=>ls.Contains(t.IMEI)).Count();
            return this.DataContext.TerminalEquipmentCommandCurrents.Where(t => ls.Contains(t.IMEI)).OrderByDescending(m => m.OperateTime).Skip((currentPage - 1) * numPerPage).Take(numPerPage);
        }

    }
    public interface ITerminalEquipmentCommandCurrentRepository : IRepository<TerminalEquipmentCommandCurrent>
    {
        IEnumerable<TerminalEquipmentCommandCurrent> GetTerminalEquipmenttCurrentCommands(List<String> ls,int currentPage, int numPerPage, out int count);
    }
}
