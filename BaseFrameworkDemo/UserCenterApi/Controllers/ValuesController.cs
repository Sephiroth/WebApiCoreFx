using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace UserCenterApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserTestController : ControllerBase
    {
        // GET api/values/5
        [HttpGet]
        public ActionResult<string> Get(int id)
        {
            return $"apiTestResult:{id}";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        public void Delete(int id)
        {
        }
    }
}