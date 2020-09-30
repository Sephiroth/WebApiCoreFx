using Microsoft.AspNetCore.Mvc;
using System;

namespace WebApiCoreFx.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Ok");
        }

    }
}