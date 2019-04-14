using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreDemo.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Message"] = "MyTestPage";
            return View();
        }
    }
}