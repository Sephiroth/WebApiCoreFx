using DBModel.Entity;
using System.Collections.Generic;

namespace ILogicLayer.Interface
{
    public interface IUserService
    {
        bool Add(List<TbUser> list);

        List<TbUser> Get(string nickName);
    }
}