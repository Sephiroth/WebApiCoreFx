using DBModel.Entity;
using IDBLayer.Interface;
using ILogicLayer.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogicLayer.Service
{
    public class UserService : IUserService
    {
        private IRepository<TbUser> rep;

        public UserService(IRepository<TbUser> repo)
        {
            rep = repo;
        }

        public async Task<bool> AddAsync(List<TbUser> list)
        {
            return await rep.AddListAsync(list);
        }

        public async Task<List<TbUser>> GetAsync(string nickName)
        {
            return await rep.GetListAsync(w => w.Nickname.Equals(nickName));
        }
    }
}