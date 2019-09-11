using Surging.DBModel.Models;
using Surging.DbRepository.Impl;

namespace Surging.TestDemo.ModuleService.Repository
{
    public class UserRepository : DbRepository<User>
    {
        public UserRepository() { }
    }
}