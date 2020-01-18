using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace WebApiCoreFx.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<string>> Get()
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
        public string Post([FromQuery]string value)
        {
            return $"___{value}";
        }

        // PUT api/values/5
        [HttpPut]
        public string Put(int id, string value)
        {
            return $"{id}_{value}";
        }

        // DELETE api/values/5
        [HttpDelete]
        public void Delete(int id)
        {
        }
    }
}
