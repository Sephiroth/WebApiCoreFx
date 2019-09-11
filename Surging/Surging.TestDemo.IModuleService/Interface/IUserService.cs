using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.DBModel.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Surging.TestDemo.IModuleService.Interface
{
    [ServiceBundle("api/{Service}")]
    public interface IUserService : IServiceKey
    {
        /// <summary>
        /// SayHello
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Service(Name = "SayHello", Date = "2019-08-30", Director = "Sephiroth")]
        Task<string> SayHello(string name);

        Task<User> GetUserAsync(string name);
    }
}