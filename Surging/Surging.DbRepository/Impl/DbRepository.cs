using Microsoft.EntityFrameworkCore;
using Surging.Core.CPlatform.Ioc;
using Surging.DBModel.Models;
using Surging.IDepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Surging.DbRepository.Impl
{
    public class DbRepository<T> : BaseRepository, IDbRepository<T> where T : class
    {
        public async Task<bool> AddEntityAsync(T t)
        {
            bool rs = false;
            using (var db = new SurgingtestContext())
            {
                await db.Set<T>().AddAsync(t);
                rs = await db.SaveChangesAsync() == 1;
            }
            return rs;
        }

        public async Task<bool> AddListAsync(List<T> t)
        {
            bool rs = false;
            using (var db = new SurgingtestContext())
            {
                await db.Set<T>().AddRangeAsync(t);
                rs = await db.SaveChangesAsync() == t.Count;
            }
            return rs;
        }

        public async Task<bool> DeleteAsync(List<T> t)
        {
            bool rs = false;
            using (var db = new SurgingtestContext())
            {
                db.Set<T>().RemoveRange(t);
                rs = await db.SaveChangesAsync() == t.Count;
            }
            return rs;
        }

        public async Task<int> ExecuteSqlAsync(string sql)
        {
            return await Task.Factory.StartNew(() =>
            {
                int rs;
                using (var db = new SurgingtestContext())
                {
                    rs = db.Database.ExecuteSqlCommand(sql);
                }
                return rs;
            });
        }

        public async Task<T> GetEntityAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return await Task.Run(() =>
            {
                T obj = null;
                using (var db = new SurgingtestContext())
                {
                    obj = db.Set<T>().Where(predicate).FirstOrDefault();
                }
                return obj;
            });
        }

        public async Task<List<T>> GetListAsync<TKey>(System.Linq.Expressions.Expression<Func<T, bool>> predicate, System.Linq.Expressions.Expression<Func<T, TKey>> key, int firstRow = 0, int pageSize = 20)
        {
            return await Task.Run(() =>
            {
                List<T> obj = null;
                using (var db = new SurgingtestContext())
                {
                    var rs = db.Set<T>().Where(predicate).OrderByDescending(key).Skip(firstRow).Take(pageSize);
                    if (rs.Count() > 0)
                    {
                        obj = rs.ToList();
                    }
                }
                return obj;
            });
        }

        public async Task<bool> ModifyEntityAsync(T t)
        {
            bool rs = false;
            using (var db = new SurgingtestContext())
            {
                db.Set<T>().Update(t);
                rs = await db.SaveChangesAsync() == 1;
            }
            return rs;
        }

        public async Task<bool> ModifyListAsync(List<T> list)
        {
            bool rs = false;
            using (var db = new SurgingtestContext())
            {
                db.Set<T>().UpdateRange(list);
                rs = await db.SaveChangesAsync() == list.Count;
            }
            return rs;
        }

        public async Task<List<T>> QuerySqlAsync(string sql)
        {
            return await Task.Factory.StartNew(() =>
            {
                List<T> list = null;
                using (var db = new SurgingtestContext())
                {
                    IQueryable<T> rs = db.Set<T>().FromSql(sql);
                    list = rs.ToList();
                }
                return list;
            });
        }
    }
}