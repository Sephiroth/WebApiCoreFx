using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiCoreFx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class FileTransmittalController : ControllerBase
    {
        public FileTransmittalController() { }

        //[HttpPost("Upload")]
        //public async Task<ActionResult<string>> UploadAsync()
        //{
        //    string a = Request.Form[""].ToString();
        //    string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "file");
        //    foreach (IFormFile file in Request.Form.Files)
        //    {
        //        if (!Convert.IsDBNull(file))
        //        {
        //            string name = Path.Combine(folder, file.FileName);
        //            using (FileStream fs = new FileStream(name, FileMode.Create))
        //            {
        //                foreach (var part in files.OrderBy(x => x.Length).ThenBy(x => x))//排一下序，保证从0-N Write
        //                {
        //                }
        //            }
        //        }
        //    }
        //}

    }
}