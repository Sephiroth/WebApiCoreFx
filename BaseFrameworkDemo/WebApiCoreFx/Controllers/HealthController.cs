﻿using Microsoft.AspNetCore.Mvc;

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