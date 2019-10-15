using Surging.DBModel.Models;
using Surging.EFCore.DBModel.SqlSugar;
using Surging.SqlSugar.DbRepository.Impl;
using System.Threading.Tasks;

namespace Surging.TestDemo.ModuleService.SqlSugarRepository
{
    public class UserRep : DbRepository<User>
    {
        public UserRep() { }

        public new async Task<bool> ModifyEntityAsync(User t)
        {
            bool rs = false;
            using (var client = SqlClient.GetInstance())
            {
                rs = await client.Updateable(t).ExecuteCommandAsync() == 1;
                client.Ado.BeginTran();
                rs = await client.Updateable(t).UpdateColumns(s=>new {t.Username }).ExecuteCommandAsync()==1;
                client.Ado.CommitTran();
                client.Ado.RollbackTran();
            }
            return rs;
        }
    }
}