using AopDLL;
using DBModel.Entity;
using IDBLayer.Interface;
using ILogicLayer.DTO;
using ILogicLayer.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogicLayer.Service
{
    public class UserService : IUserService
    {
        private readonly IRepository<db_cdzContext, TbUser> rep;

        public UserService(IRepository<db_cdzContext, TbUser> repo)
        {
            rep = repo;
        }

        public async Task<bool> AddAsync(List<TbUser> list)
        {
            return await rep.AddListAsync(list.ToArray());
        }

        public Task<bool> DeleteAsync(string id)
        {
            return rep.DeleteAsync(new TbUser { Id = id });
        }

        public async Task<ResultDTO<TbUser>> GetAll(int pageIndex, int pageSize)
        {
            object sum = null;
            List<TbUser> list = await rep.GetListAsync(null, pageIndex, pageSize, sum);
            return new ResultDTO<TbUser>(list, sum, pageIndex, pageSize);
        }

        [DothingAfterInterceptor]
        public async Task<List<TbUser>> GetAsync(string nickName)
        {
            object num = new object();
            return await rep.GetListAsync(w => w.Nickname.Equals(nickName), 0, byte.MaxValue, num);
        }

        public async Task<bool> UpdateAsync(TbUser user)
        {
            return await rep.ModifyAsync(user);
        }

    }
}