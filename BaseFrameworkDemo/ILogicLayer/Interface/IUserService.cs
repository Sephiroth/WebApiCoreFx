using AopDLL;
using DBModel.Entity;
using ILogicLayer.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ILogicLayer.Interface
{
    public interface IUserService
    {
        Task<bool> AddAsync(List<TbUser> list);

        [DothingBeforeInterceptor]
        Task<List<TbUser>> GetAsync(string nickName);

        Task<bool> UpdateAsync(TbUser user);

        Task<ResultDTO<TbUser>> GetAll(int pageIndex, int pageSize);

        Task<bool> DeleteAsync(string id);
    }
}