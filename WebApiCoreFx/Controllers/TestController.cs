using DBModel.Entity;
using ILogicLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebApiCoreFx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private IUserService userServ;

        public TestController(IUserService userServ)
        {
            this.userServ = userServ;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TbUser>> Get(string nickName)
        {
            return userServ.Get(nickName);
        }

        [HttpPost]
        public ActionResult<bool> Post([FromBody]List<TbUser> list)
        {
            return userServ.Add(list);
        }
    }
}