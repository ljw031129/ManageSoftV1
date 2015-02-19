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


        public void UpdateEquipmentId(string TerminalEquipmentId, string EquipmentIds)
        {
            string sql = @"UPDATE TerminalEquipments
                                SET EquipmentId=@EquipmentId
                                 WHERE TerminalEquipmentId=@TerminalEquipmentId";
            try
            {

                this.DataContext.Database.ExecuteSqlCommand(sql, new DbParameter[] {                     
                    new SqlParameter("EquipmentId",EquipmentIds),
                    new SqlParameter("TerminalEquipmentId",TerminalEquipmentId)
                                   });

            }
            catch (Exception)
            {

                throw;
            }
        }


        public IEnumerable<TerminalEquipment> GetTerminalEquipmentByEquipmentId(string id)
        {
            return this.DataContext.TerminalEquipments.Where(m => m.EquipmentId == id);
        }


        public void UpdateSetTerminalEquipment(string id)
        {
            string sql = @"UPDATE TerminalEquipments
                                SET EquipmentId=null
                                 WHERE EquipmentId=@EquipmentId";
            try
            {

                this.DataContext.Database.ExecuteSqlCommand(sql, new DbParameter[] {                     
                    new SqlParameter("EquipmentId",id)                  
                                   });

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

        void UpdateEquipmentId(string TerminalEquipmentId, string EquipmentIds);

        IEnumerable<TerminalEquipment> GetTerminalEquipmentByEquipmentId(string id);

        void UpdateSetTerminalEquipment(string id);
    }
}
