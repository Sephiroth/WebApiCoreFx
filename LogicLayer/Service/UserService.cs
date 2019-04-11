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

        public bool Add(List<TbUser> list)
        {
            Task<bool> t = rep.AddListAsync(list);
            return t.Result;
        }

        public List<TbUser> Get(string nickName)
        {
            var list = rep.GetListAsync(w => w.Nickname.Equals(nickName));
            return list.Result;
        }
    }
}