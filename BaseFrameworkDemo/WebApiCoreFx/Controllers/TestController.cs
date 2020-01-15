using DBModel.Entity;
using ILogicLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
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
        private readonly IUserService userServ;
        public readonly IDistributedCache cache;

        public TestController(IUserService userServ, IDistributedCache cache)
        {
            this.userServ = userServ;
            this.cache = cache;
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Get")]
        [AopDLL.Filter.CustomizeFilter]
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