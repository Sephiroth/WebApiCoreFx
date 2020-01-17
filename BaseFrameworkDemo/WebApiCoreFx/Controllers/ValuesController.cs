using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace WebApiCoreFx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet][AllowAnonymous]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2", $"(MiddlewareTest输入处理:{Request.Headers["TestMiddleware"]})" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return Convert.ToString(id);
        }

        // POST api/values
        [HttpPost("{value}")]
        [AopDLL.Filter.CustomizeFilter]
        public string Post(string value)
        {
            return $"___{value}";
        }

        // PUT api/values/5
        [HttpPut]
        [Authorize]
        public string Put(int id, string value)
        {
            return $"{id}_{value}";
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
