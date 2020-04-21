﻿using DBModel.Entity;
using IDBLayer.Interface;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DBLayer.DAL
{
    /// <summary>
    /// EF Core实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private DbContext db;

        public Repository(db_cdzContext context)
        {
            db = context;
        }

        public async Task<bool> AddListAsync([NotNull]params T[] list)
        {
            bool rs = false;
            //using var db = new db_cdzContext();
            await db.Set<T>().AddRangeAsync(list);
            rs = await db.SaveChangesAsync() == list.Length;
            return rs;
        }

        public async Task<bool> DeleteAsync([NotNull]params T[] list)
        {
            bool rs = false;
            //using var db = new db_cdzContext();
            db.Set<T>().RemoveRange(list);
            rs = await db.SaveChangesAsync() == list.Length;
            return rs;
        }

        public async Task<T> GetEntityAsync(Expression<Func<T, bool>> predicate)
        {
            T obj = null;
            //using var db = new db_cdzContext();
            obj = await db.Set<T>().Where(predicate).FirstOrDefaultAsync();
            return obj;
        }

        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate, int firstRow, int pageSize, object sum)
        {
            List<T> obj = null;
            IQueryable<T> rs = db.Set<T>().AsQueryable();
            if (predicate != null)
            {
                rs = rs.Where(predicate);
            }
            int count = await rs.CountAsync();
            if (count > 0)
            {
                obj = await rs.Skip(firstRow).Take(pageSize).ToListAsync();
            }
            sum = count;
            return obj ?? default;
        }

        public async Task<bool> ModifyAsync([NotNull]params T[] list)
        {
            bool rs = false;
            //using var db = new db_cdzContext();
            foreach (T t in list)
            {
                db.Set<T>().Update(t);
            }
            rs = await db.SaveChangesAsync() == list.Length;
            return rs;
        }

        public async Task<int> ExecuteSqlAsync([NotNull]FormattableString sql, params object[] parameters)
        {
            //using var db = new db_cdzContext();
            return await db.Database.ExecuteSqlInterpolatedAsync(sql);
            //return await db.Database.ExecuteSqlRawAsync(sql, parameters);
            //return await db.Database.ExecuteSqlCommandAsync(sql, parameters);
        }

        public async Task<List<T>> QueryAsync([NotNull]string sql, params object[] parameters)
        {
            //using var db = new db_cdzContext();
            return await db.Set<T>().FromSqlRaw(sql, parameters).ToListAsync();
        }

    }
}