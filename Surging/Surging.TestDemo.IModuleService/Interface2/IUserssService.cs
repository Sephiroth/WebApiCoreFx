using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.DBModel.Models;
using System.Threading.Tasks;

namespace Surging.TestDemo.IModuleService
{
    /// <summary>
    /// 基于sqlSugar接口
    /// </summary>
    [ServiceBundle("api/{Service}")]
    public interface IUserssService : IServiceKey
    {
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Service(Name = "获取用户", Date = "2019-08-30", Director = "lutao")]
        Task<User> GetEntity(int id);

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        [Service(Name = "新增用户", Date = "2019-08-30", Director = "lutao")]
        Task<bool> AddUser(User u);
    }
}