using Surging.Core.CPlatform.Ioc;
using Surging.EFCore.DBModel.SqlSugar;
using Surging.IDepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Surging.SqlSugar.DbRepository.Impl
{
    public class DbRepository<T> : BaseRepository, IDbRepository<T> where T : class, new()
    {
        public async Task<bool> AddEntityAsync(T t)
        {
            bool rs = false;
            using (var client = SqlClient.GetInstance())
            {
                rs = await client.Insertable(t).ExecuteCommandAsync() == 1;
            }
            return rs;
        }

        public async Task<bool> AddListAsync(List<T> t)
        {
            bool rs = false;
            using (var client = SqlClient.GetInstance())
            {
                rs = await client.Insertable(t).ExecuteCommandAsync() == t.Count;
            }
            return rs;
        }

        public async Task<bool> DeleteAsync(List<T> t)
        {
            bool rs = false;
            using (var client = SqlClient.GetInstance())
            {
                rs = await client.Deleteable(t).ExecuteCommandAsync() == t.Count;
            }
            return rs;
        }

        public Task<int> ExecuteSqlAsync(string sql)
        {
            int rs = 0;
            using (var client = SqlClient.GetInstance())
            {
                rs = client.Ado.ExecuteCommand(sql);
            }
            return Task.FromResult(rs);
        }

        public async Task<T> GetEntityAsync(Expression<Func<T, bool>> predicate)
        {
            T entity = null;
            using (var client = SqlClient.GetInstance())
            {
                entity = await client.Queryable<T>().FirstAsync(predicate);
            }
            return entity;
        }

        public async Task<List<T>> GetListAsync<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> key, int firstRow = 0, int pageSize = 20)
        {
            List<T> list = null;
            using (var client = SqlClient.GetInstance())
            {
                list = await client.Queryable<T>().Where(predicate).OrderBy(key.ToString()).ToPageListAsync(firstRow, pageSize);
            }
            return list;
        }

        public async Task<bool> ModifyEntityAsync(T t)
        {
            bool rs = false;
            using (var client = SqlClient.GetInstance())
            {
                rs = await client.Updateable(t).ExecuteCommandAsync() == 1;
                //client.Ado.BeginTran();
                //client.Ado.ExecuteCommand(client.Updateable());
                //client.Ado.CommitTran();
                //client.Ado.RollbackTran();
            }
            return rs;
        }

        public async Task<bool> ModifyListAsync(List<T> list)
        {
            bool rs = false;
            using (var client = SqlClient.GetInstance())
            {
                rs = await client.Updateable(list).ExecuteCommandAsync() == list.Count;
            }
            return rs;
        }

        public async Task<List<T>> QuerySqlAsync(string sql)
        {
            List<T> list = null;
            using (var client = SqlClient.GetInstance())
            {
                list = await client.SqlQueryable<T>(sql).ToListAsync();
            }
            return list;
        }
    }
}