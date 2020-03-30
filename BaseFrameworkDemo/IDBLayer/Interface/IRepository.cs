using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IDBLayer.Interface
{
    public interface IRepository<T> where T : class, new()
    {
        /// <summary>
        /// 批量新增(异步)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<bool> AddListAsync([NotNull]params T[] list);

        /// <summary>
        /// 获取单个实例(异步)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<T> GetEntityAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 获取多个实例(异步)
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="firstRow"></param>
        /// <param name="pageSize"></param>
        /// <param name="sum">异步不支持ref,in,out</param>
        /// <returns></returns>
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate, int firstRow, int pageSize, object sum);

        /// <summary>
        /// 批量删除(异步)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync([NotNull]params T[] list);

        /// <summary>
        /// sql执行增/删/改
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<int> ExecuteSqlAsync([NotNull]FormattableString sql, params object[] parameters);

        /// <summary>
        /// 修改(异步)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<bool> ModifyAsync([NotNull]params T[] list);

        /// <summary>
        /// sql执行查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<List<T>> QueryAsync([NotNull]string sql, params object[] parameters);
    }
}