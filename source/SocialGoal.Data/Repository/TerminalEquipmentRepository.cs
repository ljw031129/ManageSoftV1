using SocialGoal.Core.DynamicLINQ;
using SocialGoal.Data.Infrastructure;
using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;

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


        public IEnumerable<TerminalEquipment> GetJqGrid(JqSearchIn jqGridSetting, List<string> al, out int count)
        {
            IQueryable<TerminalEquipment> terminalEquipment;
            if (jqGridSetting._search && !String.IsNullOrEmpty(jqGridSetting.filters))
            {
                var wc = jqGridSetting.GenerateWhereClause(typeof(TerminalEquipment));
                terminalEquipment = this.DataContext.TerminalEquipments.Where(t => al.Contains(t.OrgEnterpriseId)).Where(wc.Clause, wc.FormatObjects);
                count = terminalEquipment.Count();
                terminalEquipment = terminalEquipment
                    .OrderBy(jqGridSetting.sidx + " " + jqGridSetting.sord)
                    .Skip((jqGridSetting.page - 1) * jqGridSetting.rows)
                    .Take(jqGridSetting.rows);
            }
            else
            {
                terminalEquipment = this.DataContext.TerminalEquipments.Where(t => al.Contains(t.OrgEnterpriseId));
                count = terminalEquipment.Count();
                terminalEquipment = terminalEquipment
                    .OrderBy(jqGridSetting.sidx + " " + jqGridSetting.sord)
                    .Skip((jqGridSetting.page - 1) * jqGridSetting.rows)
                    .Take(jqGridSetting.rows);
            }
            return terminalEquipment;
        }


        public void UpdateEquipmentId(string TerminalEquipmentId)
        {
            string sql = @"UPDATE TerminalEquipments
                                SET EquipmentId=NULL
                                 WHERE TerminalEquipmentId=@TerminalEquipmentId";
            try
            {

                this.DataContext.Database.ExecuteSqlCommand(sql, new DbParameter[] { 
                    new SqlParameter("TerminalEquipmentId",TerminalEquipmentId)
                                   });

            }
            catch (Exception)
            {

                throw;
            }
        }


        public IEnumerable<TerminalEquipment> GetJqGridDataHistory(string terminalEquipmentNum, JqSearchIn jqGridSetting, out int count)
        {
            IQueryable<TerminalEquipment> terminalEquipment;
            if (jqGridSetting._search && !String.IsNullOrEmpty(jqGridSetting.filters))
            {
                var wc = jqGridSetting.GenerateWhereClause(typeof(TerminalEquipment));
                terminalEquipment = this.DataContext.TerminalEquipments.Where(t => t.TerminalEquipmentNum == terminalEquipmentNum).Where(wc.Clause, wc.FormatObjects);
                count = terminalEquipment.Count();
                terminalEquipment = terminalEquipment
                    .OrderBy(jqGridSetting.sidx + " " + jqGridSetting.sord)
                    .Skip((jqGridSetting.page - 1) * jqGridSetting.rows)
                    .Take(jqGridSetting.rows);
            }
            else
            {
                terminalEquipment = this.DataContext.TerminalEquipments.Where(t => t.TerminalEquipmentNum == terminalEquipmentNum);
                count = terminalEquipment.Count();
                terminalEquipment = terminalEquipment
                    .OrderBy(jqGridSetting.sidx + " " + jqGridSetting.sord)
                    .Skip((jqGridSetting.page - 1) * jqGridSetting.rows)
                    .Take(jqGridSetting.rows);
            }
            return terminalEquipment;
        }
    }
    public interface ITerminalEquipmentRepository : IRepository<TerminalEquipment>
    {
        void UpdateTerminalEquipmentOrgEnterpriseId(string OrgEnterpriseId, string TerminalEquipmentIds);

        void UpdateEquipmentId(string TerminalEquipmentId, string EquipmentIds);

        IEnumerable<TerminalEquipment> GetTerminalEquipmentByEquipmentId(string id);

        void UpdateSetTerminalEquipment(string id);

        IEnumerable<TerminalEquipment> GetJqGrid(JqSearchIn jqGridSetting, List<string> al, out int count);

        void UpdateEquipmentId(string TerminalEquipmentId);

        IEnumerable<TerminalEquipment> GetJqGridDataHistory(string terminalEquipmentNum, JqSearchIn jqGridSetting, out int count);
    }
}
