using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Surging.IDepository.Interface
{
    public interface IDbRepository<T> where T : class
    {
        /// <summary>
        /// 获取单个实例(异步)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<T> GetEntityAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 获取多个实例(异步)
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="firstRow"></param>
        /// <param name="pageSize"></param>
        /// <param name="predicate"></param>
        /// <param name="key">排序字段</param>
        /// <returns></returns>
        Task<List<T>> GetListAsync<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> key, int firstRow = 0, int pageSize = 20);

        /// <summary>
        /// 批量新增(异步)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<bool> AddListAsync(List<T> t);

        Task<bool> AddEntityAsync(T t);

        /// <summary>
        /// 批量删除(异步)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(List<T> t);

        /// <summary>
        /// 修改(异步)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<bool> ModifyListAsync(List<T> list);

        /// <summary>
        /// 修改(异步)
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<bool> ModifyEntityAsync(T t);

        Task<int> ExecuteSqlAsync(string sql);

        Task<List<T>> QuerySqlAsync(string sql);
    }
}
