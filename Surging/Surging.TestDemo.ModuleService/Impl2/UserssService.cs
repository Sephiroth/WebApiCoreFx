using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using Surging.DBModel.Models;
using Surging.TestDemo.IModuleService;
using Surging.TestDemo.ModuleService.SqlSugarRepository;
using System.Threading.Tasks;

namespace Surging.TestDemo.ModuleService
{
    [ModuleName("Userss")]
    public class UserssService : ProxyServiceBase, IUserssService
    {
        private UserRep resp;

        public UserssService(UserRep resp)
        {
            this.resp = resp;
        }

        public async Task<bool> AddUser(User u)
        {
            return await resp.AddEntityAsync(u);
        }

        public async Task<User> GetEntity(int id)
        {
            return await resp.GetEntityAsync(s => s.Id == id);
        }
    }
}