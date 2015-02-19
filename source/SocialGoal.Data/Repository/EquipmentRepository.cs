using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialGoal.Data.Infrastructure;
using SocialGoal.Model.Models;
using System.Data.Entity;
using Newtonsoft.Json.Linq;
using xFilter.Expressions;

namespace SocialGoal.Data.Repository
{

    public class EquipmentRepository : RepositoryBase<Equipment>, IEquipmentRepository
    {
        private readonly IDbSet<Equipment> dbset;
        public EquipmentRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
            dbset = DataContext.Set<Equipment>();
        }

        public IEnumerable<Equipment> GetEquipmentsJqGridByCurrentUser(Core.xFilter.Expressions.JqGridSetting jqGridSetting, List<string> currentT, out int count)
        {
            //JSON字符串转化 
            bool sortOrder = true;
            switch (jqGridSetting.sord)
            {
                case "asc":
                    sortOrder = true;
                    break;
                case "desc":
                    sortOrder = false;
                    break;
                default:
                    sortOrder = false;
                    break;
            }
            if (jqGridSetting._search)
            {
                JObject container = JObject.Parse(jqGridSetting.filters);
                xFilter.Expressions.Group g = WebHelper.DeserializeGroupFromJSON(container);
                //补充Where条件
                if (jqGridSetting.Where != null)
                {
                    foreach (var item in jqGridSetting.Where.Rules)
                    {
                        g.Rules.Add(item);
                    }

                }
                var results = dbset.OrderByExtensions(jqGridSetting.sidx, sortOrder).Where(p => currentT.Contains(p.EquipmentId)).Where<Equipment>(g.ToExpressionTree<Equipment>().Compile()).Skip((jqGridSetting.page - 1) * jqGridSetting.rows).Take(jqGridSetting.rows);
                count = dbset.Where(p => currentT.Contains(p.EquipmentId)).Where<Equipment>(g.ToExpressionTree<Equipment>().Compile()).Count();
                return results;
            }
            else
            {
                if (jqGridSetting.Where != null)
                {
                    count = dbset.Where(p => currentT.Contains(p.EquipmentId)).Where<Equipment>(jqGridSetting.Where.ToExpressionTree<Equipment>().Compile()).Count();
                    var results = dbset.OrderByExtensions(jqGridSetting.sidx, sortOrder).Where(p => currentT.Contains(p.EquipmentId)).Where<Equipment>(jqGridSetting.Where.ToExpressionTree<Equipment>().Compile()).Skip((jqGridSetting.page - 1) * jqGridSetting.rows).Take(jqGridSetting.rows);

                    return results;
                }
                else
                {
                    var results = dbset.OrderByExtensions(jqGridSetting.sidx, sortOrder).Where(p => currentT.Contains(p.EquipmentId.Trim())).Skip((jqGridSetting.page - 1) * jqGridSetting.rows).Take(jqGridSetting.rows).ToList();
                    count = dbset.Where(p => currentT.Contains(p.EquipmentId.Trim())).Count();
                    return results;
                }
            }
        }
    }
    public interface IEquipmentRepository : IRepository<Equipment>
    {
        IEnumerable<Equipment> GetEquipmentsJqGridByCurrentUser(Core.xFilter.Expressions.JqGridSetting jqGridSetting, List<string> st, out int count);
    }
}
