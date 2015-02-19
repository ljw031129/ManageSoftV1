using SocialGoal.Data.Infrastructure;
using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Data.Repository
{
    public class TerminalEquipmentCommandRepository : RepositoryBase<TerminalEquipmentCommand>, ITerminalEquipmentCommandRepository
    {
        public TerminalEquipmentCommandRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public IEnumerable<TerminalEquipmentCommand> GetTerminalEquipmentCommands(string IMEI, int currentPage, int numPerPage, out int count)
        {
            count = this.DataContext.TerminalEquipmentCommands.Where(m => m.IMEI == IMEI).Count();
            return this.DataContext.TerminalEquipmentCommands.OrderByDescending(m=>m.OperateTime).Where(m => m.IMEI == IMEI).Skip((currentPage - 1) * numPerPage).Take(numPerPage);

        }
    }
    public interface ITerminalEquipmentCommandRepository : IRepository<TerminalEquipmentCommand>
    {
        IEnumerable<TerminalEquipmentCommand> GetTerminalEquipmentCommands(string IMEI, int currentPage, int numPerPage, out int count);
    }
}
