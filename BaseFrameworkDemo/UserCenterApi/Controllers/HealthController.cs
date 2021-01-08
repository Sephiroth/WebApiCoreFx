using Microsoft.AspNetCore.Mvc;
using System;

namespace UserCenterApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Check()
        {
            return Ok(DateTime.Now.ToString());
        }
    }
}