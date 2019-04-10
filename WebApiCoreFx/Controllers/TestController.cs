using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBLayer.DAL;
using DBModel.Entity;
using IDBLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiCoreFx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private IRepository<TbUser> rep;

        public TestController(IRepository<TbUser> repo)
        {
            rep = repo;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TbUser>> Get(string nickName)
        {
            var list = rep.GetListAsync(w => w.Nickname.Equals(nickName));
            return list.Result;
        }
    }
}