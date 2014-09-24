using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PagedList;
using System.Linq;
using SocialGoal.Core.xFilter.Expressions;

namespace SocialGoal.Data.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        T GetById(long id);
        T GetById(string id);
        T Get(Expression<Func<T, bool>> where);

        IQueryable<T> GetIQueryableAll();

        IEnumerable<T> GetAll();
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        IPagedList<T> GetPage<TOrder>(Page page, Expression<Func<T, bool>> where, Expression<Func<T, TOrder>> order);

        IPagedList<T> GetPage<TOrder>(string gridSettings);
        IEnumerable<T> GetPageJqGrid<TOrder>(JqGridSetting jqGridSetting, out int count);
        //Select2数据
        IEnumerable<T> GetSelect2(Expression<Func<T, bool>> where,string sortColumn,bool sortOrder, int pageSize, int pageNum, out int reTotal);
    }
}
