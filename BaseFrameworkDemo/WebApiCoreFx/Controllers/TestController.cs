using DBModel.Entity;
using ILogicLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiCoreFx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {
        private IUserService userServ;

        public TestController(IUserService userServ)
        {
            this.userServ = userServ;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TbUser>>> Get(string nickName)
        {
            //string ss = "sss";
            //int a = Convert.ToInt32(ss);
            return await userServ.GetAsync(nickName);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Post([FromBody]List<TbUser> list)
        {
            return await userServ.AddAsync(list);
        }
    }
}