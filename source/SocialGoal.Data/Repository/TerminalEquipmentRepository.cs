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
    public class TerminalEquipmentRepository : RepositoryBase<TerminalEquipment>, ITerminalEquipmentRepository
    {
        public TerminalEquipmentRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void UpdateTerminalEquipmentOrgEnterpriseId(string OrgEnterpriseId, string TerminalEquipmentIds)
        {
            string sql = @"UPDATE TerminalEquipments
                                SET OrgEnterpriseId=@OrgEnterpriseId
                                 WHERE TerminalEquipmentId=@TerminalEquipmentId";
            string[] arr = TerminalEquipmentIds.Split(',');
            try
            {
                foreach (var item in arr)
                {
                    this.DataContext.Database.ExecuteSqlCommand(sql, new DbParameter[] {                     
                    new SqlParameter("OrgEnterpriseId",OrgEnterpriseId),
                    new SqlParameter("TerminalEquipmentId",item)
                                   });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
    public interface ITerminalEquipmentRepository : IRepository<TerminalEquipment>
    {
        void UpdateTerminalEquipmentOrgEnterpriseId(string OrgEnterpriseId, string TerminalEquipmentIds);
    }
}
