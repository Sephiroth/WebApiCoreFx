using DBModel.Entity;
using ILogicLayer.DTO;
using ILogicLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCoreFx.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    //[Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TestController : ControllerBase
    {
        private readonly IUserService userServ;
        private readonly IDistributedCache cache;
        private readonly IMemoryCache memCache;
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userServ"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="cache"></param>
        /// <param name="memCache"></param>
        public TestController(IUserService userServ, IServiceProvider serviceProvider, IDistributedCache cache, IMemoryCache memCache)
        {
            this.userServ = userServ;
            this.cache = cache;
            this.memCache = memCache;
            this.serviceProvider = serviceProvider;
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
            bool getRs = memCache.TryGetValue(key, out List<TbUser> list);
            if (!getRs)
            {
                list = await userServ.GetAsync(nickName);
                memCache.Set(key, list, TimeSpan.FromMinutes(30));
            }
            return list;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Post([FromBody] List<TbUser> list)
        {
            return await userServ.AddAsync(list);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ResultDTO<TbUser>> GetAll(int pageIndex = 1, int pageSize = 10)
        {
            string key = $"{Request.Path}_{pageIndex}_{pageSize}";
            byte[] rs = await cache.GetAsync(key);
            ResultDTO<TbUser> rsdo = null;
            if (rs == null)
            {
                rsdo = await userServ.GetAll(pageIndex, pageSize);
                await cache.SetAsync(key,
                    System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(rsdo),
                    new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
            }
            else
            {
                rsdo = System.Text.Json.JsonSerializer.Deserialize<ResultDTO<TbUser>>(rs);
            }
            return rsdo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<bool> Test(string name)
        {
            using db_cdzContext dbContext = serviceProvider.CreateScope().ServiceProvider.GetService(typeof(db_cdzContext)) as db_cdzContext;
            return dbContext.Set<TbUser>().AsQueryable().Where(s => s.Name.Equals(name)).Any();
        }

    }
}