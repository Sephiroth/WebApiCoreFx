using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using Surging.DBModel.Models;
using Surging.TestDemo.IModuleService;
using Surging.TestDemo.ModuleService.Repository;
using System.Threading.Tasks;

namespace Surging.TestDemo.ModuleService
{
    [ModuleName("User")]
    public class UserService : ProxyServiceBase, IUserService
    {
        private readonly UserRepository resp;

        public UserService(UserRepository resp)
        {
            this.resp = resp;
        }

        public async Task<User> GetUserAsync(string name)
        {
            return await resp.GetEntityAsync(s => s.Username.Equals(name));
        }

        public Task<string> SayHello(string name)
        {
            return Task.FromResult($"Hello,{name}");
        }
    }
}