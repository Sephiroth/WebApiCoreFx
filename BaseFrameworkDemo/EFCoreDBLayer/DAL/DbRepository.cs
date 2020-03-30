using DBModel.Entity;
using IDBLayer.Interface;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EFCoreDBLayer.DAL
{
    public class DbRepository<T> : IRepository<T> where T : class, new()
    {
        private db_cdzContext context;

        public DbRepository(db_cdzContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddListAsync([NotNull] params T[] list)
        {
            await context.Set<T>().AddRangeAsync(list);
            return await context.SaveChangesAsync() == list.Length;
        }

        public async Task<bool> DeleteAsync([NotNull] params T[] list)
        {
            context.Set<T>().RemoveRange(list);
            return await context.SaveChangesAsync() == list.Length;
        }

        public async Task<int> ExecuteSqlAsync([NotNull] FormattableString sql, params object[] parameters)
        {
            return await context.Database.ExecuteSqlInterpolatedAsync(sql);
            //return await context.Database.ExecuteSqlRawAsync(sql, parameters);
            //return await context.Database.ExecuteSqlCommandAsync(sql, parameters);
        }

        public async Task<T> GetEntityAsync(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate, int firstRow, int pageSize, object sum)
        {
            List<T> list = null;
            var rs = context.Set<T>().Where(predicate);
            int count = await rs.CountAsync();
            if (count > 0)
            {
                list = await rs.Skip(firstRow).Take(pageSize).ToListAsync();
            }
            sum = count;
            return list ?? default;
        }

        public async Task<bool> ModifyAsync([NotNull] params T[] list)
        {
            context.Set<T>().UpdateRange(list);
            return await context.SaveChangesAsync() == list.Length;
        }

        public async Task<List<T>> QueryAsync([NotNull] string sql, params object[] parameters)
        {
            return await context.Set<T>().FromSqlRaw(sql, parameters).ToListAsync();
        }
    }
}