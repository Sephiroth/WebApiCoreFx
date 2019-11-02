using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IDBLayer.Interface
{
    public interface IRepository<T> where T : class, new()
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
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 批量新增(异步)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<bool> AddListAsync(List<T> t);

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
        Task<bool> ModifyAsync(List<T> list);
    }
}