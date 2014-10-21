using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PagedList;
using SocialGoal.Data.Models;
using xFilter.Expressions;
using Newtonsoft.Json.Linq;
using SocialGoal.Core.xFilter.Expressions;
using System.Data.Entity.Infrastructure;

namespace SocialGoal.Data.Infrastructure
{
    public abstract class RepositoryBase<T> where T : class
    {
        private SocialGoalEntities dataContext;
        private readonly IDbSet<T> dbset;
        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            dbset = DataContext.Set<T>();
        }

        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }

        protected SocialGoalEntities DataContext
        {
            get { return dataContext ?? (dataContext = DatabaseFactory.Get()); }
        }
        public virtual void Add(T entity)
        {
            dbset.Add(entity);
        }
        public virtual void Update(T entity)
        {
            RemoveHoldingEntityInContext(entity);
            dbset.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }
        public virtual void Delete(T entity)
        {
            //  RemoveHoldingEntityInContext(entity);
            dbset.Remove(entity);
        }
        //用于监测Context中的Entity是否存在，如果存在，将其Detach，防止出现问题。
        private Boolean RemoveHoldingEntityInContext(T entity)
        {
            var objContext = ((IObjectContextAdapter)dataContext).ObjectContext;
            var objSet = objContext.CreateObjectSet<T>();
            var entityKey = objContext.CreateEntityKey(objSet.EntitySet.Name, entity);

            Object foundEntity;
            var exists = objContext.TryGetObjectByKey(entityKey, out foundEntity);

            if (exists)
            {
                objContext.Detach(foundEntity);
            }

            return (exists);
        }
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbset.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                dbset.Remove(obj);
        }
        public virtual T GetById(long id)
        {
            return dbset.Find(id);
        }
        public virtual T GetById(string id)
        {

            return dbset.Find(id);
        }
        public virtual IEnumerable<T> GetAll()
        {
            return dbset.ToList();
        }

        public virtual IQueryable<T> GetIQueryableAll()
        {
            return dbset;
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where).ToList();
        }

        /// <summary>
        /// Return a paged list of entities
        /// </summary>
        /// <typeparam name="TOrder"></typeparam>
        /// <param name="page">Which page to retrieve</param>
        /// <param name="where">Where clause to apply</param>
        /// <param name="order">Order by to apply</param>
        /// <returns></returns>
        public virtual IPagedList<T> GetPage<TOrder>(Page page, Expression<Func<T, bool>> where, Expression<Func<T, TOrder>> order)
        {
            var results = dbset.OrderBy(order).Where(where).GetPage(page).ToList();
            var total = dbset.Count(where);
            return new StaticPagedList<T>(results, page.PageNumber, page.PageSize, total);
        }


        public virtual IPagedList<T> GetPage<TOrder>(string gridSettings)
        {
            JObject container = JObject.Parse(gridSettings);
            //JSON字符串转化        

            int currentPage = Convert.ToInt32(container["PageIndex"]);
            int pageSize = Convert.ToInt32(container["PageSize"]);
            string sortColumn = container["SortColumn"].ToString();
            bool sortOrder = true;
            switch (container["SortOrder"].ToString())
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
            if (container["Where"].ToString() != "")
            {
                xFilter.Expressions.Group g = WebHelper.DeserializeGroupFromJSON(container["Where"]);
                var results = dbset.OrderByExtensions(sortColumn, sortOrder).Where<T>(g.ToExpressionTree<T>().Compile()).Skip(currentPage).Take(pageSize).ToList();
                var total = dbset.Where<T>(g.ToExpressionTree<T>().Compile()).Count();
                return new StaticPagedList<T>(results, currentPage, pageSize, total);
            }
            else
            {
                var results = dbset.OrderByExtensions(sortColumn, sortOrder).Skip(currentPage).Take(pageSize).ToList();
                var total = dbset.Count();
                return new StaticPagedList<T>(results, currentPage, pageSize, total);

            }
        }


        public T Get(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where).FirstOrDefault<T>();
        }

        //select2返回结果
        public IEnumerable<T> GetSelect2(Expression<Func<T, bool>> where, string sortColumn, bool sortOrder, int pageSize, int pageNum, out int reTotal)
        {
            reTotal = dbset.Where(where).Count();
            return dbset.OrderByExtensions(sortColumn, sortOrder).Where(where).Skip((pageNum - 1) * pageSize).Take(pageSize);
        }

        public virtual IEnumerable<T> GetPageJqGrid<TOrder>(JqGridSetting jqGridSetting, out int count)
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
                var results = dbset.OrderByExtensions(jqGridSetting.sidx, sortOrder).Where<T>(g.ToExpressionTree<T>().Compile()).Skip((jqGridSetting.page - 1) * jqGridSetting.rows).Take(jqGridSetting.rows);
                count = dbset.Where<T>(g.ToExpressionTree<T>().Compile()).Count();
                return results;
            }
            else
            {
                if (jqGridSetting.Where != null)
                {
                    count = dbset.Where<T>(jqGridSetting.Where.ToExpressionTree<T>().Compile()).Count();
                    var results = dbset.OrderByExtensions(jqGridSetting.sidx, sortOrder).Where<T>(jqGridSetting.Where.ToExpressionTree<T>().Compile()).Skip((jqGridSetting.page - 1) * jqGridSetting.rows).Take(jqGridSetting.rows);
                  
                    return results;
                }
                else
                {
                    var results = dbset.OrderByExtensions(jqGridSetting.sidx, sortOrder).Skip((jqGridSetting.page - 1) * jqGridSetting.rows).Take(jqGridSetting.rows).ToList();
                    count = dbset.Count();
                    return results;
                }
            }
        }
    }
}
