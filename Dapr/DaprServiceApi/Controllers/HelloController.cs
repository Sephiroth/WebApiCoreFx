using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaprServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        private readonly ILogger _logger;
        public HelloController(ILogger<HelloController> logger) => _logger = logger;

        [HttpGet]
        public string SayHello(string name)
        {
            _logger.LogInformation(name);
            return $"hello {name},now is {DateTime.Now}";
        }

    }
}