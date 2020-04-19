using DBModel.Entity;
using ILogicLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiCoreFx.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TestController : ControllerBase
    {
        private readonly IUserService userServ;
        private readonly IDistributedCache cache;
        private readonly IMemoryCache memCache;

        public TestController(IUserService userServ, IDistributedCache cache, IMemoryCache memCache)
        {
            this.userServ = userServ;
            this.cache = cache;
            this.memCache = memCache;
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        [HttpGet]
        [AopDLL.Filter.CustomizeFilter]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<TbUser>>> Get(string nickName)
        {
            string key = $"{Request.Path}_{nickName}";
            List<TbUser> list = memCache.Get<List<TbUser>>(key);
            if (list == null || list.Count < 1)
            {
                list = await userServ.GetAsync(nickName);
                memCache.Set(key, list);
            }
            return list;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Post([FromBody]List<TbUser> list)
        {
            return await userServ.AddAsync(list);
        }
    }
}