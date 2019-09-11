using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Surging.Core.CPlatform.Ioc;

namespace Service.Test.Serivce
{
    [ModuleName("Test")]
    public class TestService : ProxyServiceBase, ITestService
    {
        //private readonly UsersRepository _usersRepository;
        //public UserService(UsersRepository usersRepository)
        //{
        //    _usersRepository = usersRepository;
        //}
        public  Task<string> SayHello(string name)
        {
            return  Task.FromResult($"{name} say:hello");
        }
    }
}
