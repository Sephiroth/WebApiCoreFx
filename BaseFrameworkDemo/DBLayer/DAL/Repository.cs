using DBModel.Entity;
using IDBLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DBLayer.DAL
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        public async Task<bool> AddListAsync(List<T> t)
        {
            bool rs = false;
            using (var db = new db_cdzContext())
            {
                await db.Set<T>().AddRangeAsync(t);
                rs = await db.SaveChangesAsync() == t.Count;
            }
            return rs;
        }

        public async Task<bool> DeleteAsync(List<T> t)
        {
            bool rs = false;
            using (var db = new db_cdzContext())
            {
                db.Set<T>().RemoveRange(t);
                rs = await db.SaveChangesAsync() == t.Count;
            }
            return rs;
            //return await Task.Run(() =>
            // {
            //     bool rs = false;
            //     using (var db = new db_cdzContext())
            //     {
            //         db.Set<T>().RemoveRange(t);
            //         rs = db.SaveChanges() == t.Count;
            //     }
            //     return rs;
            // });
        }

        public async Task<T> GetEntityAsync(Expression<Func<T, bool>> predicate)
        {
            return await Task.Run(() =>
            {
                T obj = null;
                using (var db = new db_cdzContext())
                {
                    obj = db.Set<T>().Where(predicate).FirstOrDefault();
                }
                return obj;
            });
        }

        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate)
        {
            return await Task.Run(() =>
            {
                List<T> obj = null;
                using (var db = new db_cdzContext())
                {
                    var rs = db.Set<T>().Where(predicate);
                    if (rs.Count() > 0)
                    {
                        obj = rs.ToList();
                    }
                }
                return obj;
            });
        }

        public async Task<bool> ModifyAsync(List<T> list)
        {
            bool rs = false;
            using (var db = new db_cdzContext())
            {
                db.Set<T>().UpdateRange(list);
                rs = await db.SaveChangesAsync() == list.Count;
            }
            return rs;
            //return await Task.Run(() =>
            //{
            //    bool rs = false;
            //    using (var db = new db_cdzContext())
            //    {
            //        db.Set<T>().UpdateRange(list);
            //        rs = db.SaveChanges() == list.Count;
            //    }
            //    return rs;
            //});
        }
    }
}