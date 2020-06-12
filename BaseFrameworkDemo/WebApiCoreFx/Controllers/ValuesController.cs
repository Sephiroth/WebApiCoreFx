using DBModel.Entity;
using ILogicLayer.DTO;
using ILogicLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiCoreFx.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class ValuesController : ControllerBase
    {
        private readonly IUserService userServ;

        public ValuesController(IUserService userServ)
        {
            this.userServ = userServ;
        }

        // GET api/values
        [HttpGet]
        [AllowAnonymous]
        public string[] Get()
        {
            return new string[] { "value1", "value2", $"(MiddlewareTest输入处理:{Request.Headers["TestMiddleware"]})" };
        }

        /// <summary>
        /// ValidateAntiForgeryToken特性(Request.Header包含X-XSRF-TOKEN才能正常请求)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [AopDLL.Filter.CustomizeFilter]
        [ValidateAntiForgeryToken]
        public string Post([FromQuery] string value)
        {
            return $"___{value}";
        }

        // PUT api/values/5
        [HttpPut]
        public async Task<ResultDTO<TbUser>> Put(uint pageIndex, uint pageSize)
        {
            return await userServ.GetAll((int)pageIndex, (int)pageSize);
        }

        // DELETE api/values/5
        [HttpDelete]
        public void Delete(int id)
        {
        }
    }
}
