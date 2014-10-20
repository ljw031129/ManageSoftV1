using SocialGoal.Data.Infrastructure;
using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
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

        public void UpdateStatue(string id, string statue)
        {
            string sql = @"UPDATE TerminalSimCards
                                SET TerminalSimCardState=@TerminalSimCardState
                                 WHERE TerminalSimCardId=@TerminalSimCardId";
            try
            {

                this.DataContext.Database.ExecuteSqlCommand(sql, new DbParameter[] {                     
                    new SqlParameter("TerminalSimCardId",id),
                    new SqlParameter("TerminalSimCardState",statue)
                                   });

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
    public interface ITerminalSimCardRepository : IRepository<TerminalSimCard>
    {
        void UpdateStatue(string id, string statue);

    }

}
