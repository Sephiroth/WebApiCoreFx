using EFCore.BulkExtensions;
using IDBLayer.Interface;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EFCoreDBLayer.MySQL.DAL
{
    public class DbRepository<TDbContext, T> : IRepository<TDbContext, T> where T : class, new() where TDbContext : DbContext
    {
        private readonly DbContext context;

        public DbRepository(TDbContext context)
        {
            this.context = context;
        }

        public async Task<int> AddListAsync([NotNull] params T[] list)
        {
            //await context.Set<T>().AddRangeAsync(list);
            await context.BulkInsertAsync(list);
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync([NotNull] params T[] list)
        {
            context.Set<T>().RemoveRange(list);
            return await context.SaveChangesAsync();
        }

        public async Task<int> ExecuteSqlAsync([NotNull] FormattableString sql, params object[] parameters)
        {
            return await context.Database.ExecuteSqlInterpolatedAsync(sql);
            //return await context.Database.ExecuteSqlRawAsync(sql, parameters);
            //return await context.Database.ExecuteSqlCommandAsync(sql, parameters);
        }

        public async Task<T> GetEntityAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> selector = null)
        {
            return await context.Set<T>().Where(predicate).SelectEntity(selector).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetListAsync<Key>(Expression<Func<T, bool>> predicate, int firstRow, int count, object sum,
            Expression<Func<T, Key>> keyExpression = null, string sequenceDirection = "ESC", Expression<Func<T, T>> selector = null)
        {
            List<T> list = null;
            IQueryable<T> query = context.Set<T>().AsQueryable();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            sum = await query.CountAsync();
            if (count > 0)
            {
                if (keyExpression != null)
                {
                    if (string.Equals(sequenceDirection, "ESC"))
                        query = query.OrderBy(keyExpression);
                    else
                        query = query.OrderByDescending(keyExpression);
                }
                list = await query.SelectEntity(selector).Skip(firstRow).Take(count).AsNoTracking().ToListAsync();
            }
            return list ?? default;
        }

        public async Task<int> ModifyAsync([NotNull] params T[] list)
        {
            context.Set<T>().UpdateRange(list);
            return await context.SaveChangesAsync();
        }

        public async Task<int> ModifyAsync([NotNull] T t, Expression<Func<T, object>>[] updateProperties = null)
        {
            EntityEntry<T> entity = context.Attach(t);
            if (updateProperties.Any())
            {
                foreach (var property in updateProperties)
                {
                    entity.Property(property).IsModified = true;
                }
            }
            else
            {
                entity.State = EntityState.Modified;
            }
            return await context.SaveChangesAsync();
        }

        public async Task<List<T>> QueryAsync([NotNull] string sql, params object[] parameters)
        {
            return await context.Set<T>().FromSqlRaw(sql, parameters).ToListAsync();
        }

    }

    /// <summary>
    /// 自定义拓展
    /// </summary>
    public static class QueryableExtension
    {
        public static IQueryable<T> SelectEntity<T>(this IQueryable<T> query, Expression<Func<T, T>> selector = null)
        {
            if (selector != null) { query = query.Select(selector); }
            return query;
        }
    }
}