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
    //[Authorize]
    public class TestController : ControllerBase
    {
        private IUserService userServ;

        public TestController(IUserService userServ)
        {
            this.userServ = userServ;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult<IEnumerable<TbUser>>> Get(string nickName)
        {
            return await userServ.GetAsync(nickName);
        }

        [HttpPost]
        [Route("Post")]
        public async Task<ActionResult<bool>> Post([FromBody]List<TbUser> list)
        {
            return await userServ.AddAsync(list);
        }
    }
}