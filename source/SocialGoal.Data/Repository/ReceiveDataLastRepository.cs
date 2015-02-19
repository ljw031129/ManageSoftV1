using Newtonsoft.Json.Linq;
using SocialGoal.Data.Infrastructure;
using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xFilter.Expressions;

namespace SocialGoal.Data.Repository
{

    public class ReceiveDataLastRepository : RepositoryBase<ReceiveDataLast>, IReceiveDataLastRepository
    {

        private readonly IDbSet<ReceiveDataLast> dbset;
        public ReceiveDataLastRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
            dbset = DataContext.Set<ReceiveDataLast>();
        }
        public ReceiveDataLast GetReceiveDataLastByTerminalNum(string p)
        {
            return this.DataContext.ReceiveDataLasts.Where(l => l.IMEI == p).FirstOrDefault();
        }


        public IEnumerable<ReceiveDataLast> GetCurrentUserReceiveDataLasts(Core.xFilter.Expressions.JqGridSetting jqGridSetting, List<string> currentT, out int count)
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

                var results = dbset.OrderByExtensions(jqGridSetting.sidx, sortOrder).Where(p => currentT.Contains(p.IMEI)).Where<ReceiveDataLast>(g.ToExpressionTree<ReceiveDataLast>().Compile()).Skip((jqGridSetting.page - 1) * jqGridSetting.rows).Take(jqGridSetting.rows);
                count = dbset.Where(p => currentT.Contains(p.IMEI)).Where<ReceiveDataLast>(g.ToExpressionTree<ReceiveDataLast>().Compile()).Count();
                return results;
            }
            else
            {
                if (jqGridSetting.Where != null)
                {
                    count = dbset.Where(p => currentT.Contains(p.IMEI)).Where<ReceiveDataLast>(jqGridSetting.Where.ToExpressionTree<ReceiveDataLast>().Compile()).Count();
                    var results = dbset.OrderByExtensions(jqGridSetting.sidx, sortOrder).Where(p => currentT.Contains(p.IMEI)).Where<ReceiveDataLast>(jqGridSetting.Where.ToExpressionTree<ReceiveDataLast>().Compile()).Skip((jqGridSetting.page - 1) * jqGridSetting.rows).Take(jqGridSetting.rows);

                    return results;
                }
                else
                {
                    var results = dbset.OrderByExtensions(jqGridSetting.sidx, sortOrder).Where(p => currentT.Contains(p.IMEI.Trim())).Skip((jqGridSetting.page - 1) * jqGridSetting.rows).Take(jqGridSetting.rows).ToList();
                    count = dbset.Where(p => currentT.Contains(p.IMEI.Trim())).Count();
                    return results;
                }
            }
        }
    }
    public interface IReceiveDataLastRepository : IRepository<ReceiveDataLast>
    {

        ReceiveDataLast GetReceiveDataLastByTerminalNum(string p);

        IEnumerable<ReceiveDataLast> GetCurrentUserReceiveDataLasts(Core.xFilter.Expressions.JqGridSetting jqGridSetting, List<string> currentT, out int count);
    }
}
