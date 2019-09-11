using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Test.Serivce
{
    [ServiceBundle("api/{Service}")]
    public interface ITestService : IServiceKey
    {

        Task<string> SayHello(string name);

    }
}
