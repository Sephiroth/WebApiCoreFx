using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace WebApiCoreFx.Controllers
{
    /// <summary>
    /// 文件传输接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileTransmittalController : ControllerBase
    {
        /// <summary>
        /// buffer大小:100k
        /// </summary>
        const int BUFF_SIZE = 102400;

        public FileTransmittalController() { }

        /// <summary>
        /// 上传大文件
        /// </summary>
        /// <returns></returns>
        //[HttpPost("UploadAsync")]
        [HttpGet]
        public async Task<IActionResult> UploadAsync()
        {
            var data = Request.Form.Files["data"];
            string lastModified = Request.Form["lastModified"].ToString();
            var total = Request.Form["total"];
            var fileName = Request.Form["fileName"];
            var index = Request.Form["index"];
            string temporary = Path.Combine($"{Directory.GetCurrentDirectory()}/wwwroot/", lastModified);//临时保存分块的目录

            try
            {
                if (!Directory.Exists(temporary))
                    Directory.CreateDirectory(temporary);
                string filePath = Path.Combine(temporary, index.ToString());
                if (!Convert.IsDBNull(data))
                {
                    await Task.Run(() =>
                    {
                        FileStream fs = new FileStream(filePath, FileMode.Create);
                        data.CopyTo(fs);
                    });
                }
                bool mergeOk = false;
                if (total == index)
                {
                    mergeOk = await FileMerge(lastModified, fileName);
                }
            }
            catch (Exception ex)
            {
                Directory.Delete(temporary);//删除文件夹
                throw ex;
            }
            return Ok(true);
        }

        /// <summary>
        /// 下载大文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="respType"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DownloadFileAsync([FromQuery]string filename, [FromQuery]string respType)
        {
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"FileFolder/{filename}");
            if (!System.IO.File.Exists(file))
                return NotFound("filename");

            string contentDisposition = $"attachment;filename={HttpUtility.UrlEncode(filename)}";
            Response.ContentType = respType;
            Response.Headers.Add("Content-Disposition", new string[] { contentDisposition });
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                using (Response.Body)
                {
                    // 获取下载文件的大小
                    long contentLength = fs.Length;
                    // 在Response的Header中设置下载文件的大小，这样客户端浏览器才能正确显示下载的进度
                    Response.ContentLength = contentLength;
                    byte[] buffer;
                    long hasRead = 0;//变量hasRead用于记录已经发送了多少字节的数据到客户端浏览器
                                     // 如果hasRead小于contentLength，说明下载文件还没读取完毕，继续循环读取下载文件的内容，并发送到客户端浏览器
                    while (hasRead < contentLength)
                    {
                        /* HttpContext.RequestAborted.IsCancellationRequested可用于检测客户端浏览器和ASP.NET Core服务器之间的连接状态
                         * 如果HttpContext.RequestAborted.IsCancellationRequested返回true，说明客户端浏览器中断了连接 */
                        if (HttpContext.RequestAborted.IsCancellationRequested)
                        {
                            //如果客户端浏览器中断了到ASP.NET Core服务器的连接，这里应该立刻break，取消下载文件的读取和发送，避免服务器耗费资源
                            break;
                        }
                        buffer = new byte[BUFF_SIZE];
                        //从下载文件中读取bufferSize(10k)大小的内容到服务器内存中
                        int currentRead = await fs.ReadAsync(buffer, 0, BUFF_SIZE);
                        //发送读取的内容数据到客户端浏览器
                        await Response.Body.WriteAsync(buffer, 0, currentRead);
                        await Response.Body.FlushAsync();
                        hasRead += currentRead;//更新已经发送到客户端浏览器的字节数
                    }
                }
            }
            return Ok("下载完成");
        }


        public async Task<bool> FileMerge(string lastModified, string fileName)
        {
            string temporary = Path.Combine($"{Directory.GetCurrentDirectory()}/wwwroot/", lastModified);//临时文件夹
            string fileExt = Path.GetExtension(fileName);//获取文件后缀
            string[] files = Directory.GetFiles(temporary);//获得下面的所有文件
            if (files == null || files.Length < 1)
                return false;

            string finalPath = Path.Combine($"{Directory.GetCurrentDirectory()}/wwwroot/", $"{DateTime.Now.ToString("yyMMddHHmmss")}{fileExt}");
            using (var fs = new FileStream(finalPath, FileMode.Create))
            {
                foreach (var part in files.OrderBy(x => x.Length).ThenBy(x => x))//排一下序，保证从0-N Write
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(part);
                    await fs.WriteAsync(bytes, 0, bytes.Length);
                    bytes = null;
                    System.IO.File.Delete(part);//删除分块
                }
            }
            Directory.Delete(temporary);//删除文件夹
            return true;
        }

    }
}