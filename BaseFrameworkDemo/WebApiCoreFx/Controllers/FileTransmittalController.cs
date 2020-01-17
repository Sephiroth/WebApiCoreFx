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
        //public async Task<IActionResult> UploadAsync([FromForm]string fileName)
        //{
        //    // 获取boundary
        //    string boundary = HeaderUtilities.RemoveQuotes(MediaTypeHeaderValue.Parse(Request.ContentType).Boundary).Value;
        //    // 得到reader
        //    MultipartReader reader = new MultipartReader(boundary, HttpContext.Request.Body);
        //    // { BodyLengthLimit = 2000 };
        //    MultipartSection section = await reader.ReadNextSectionAsync();
        //    fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"FileFolder/{fileName}");
        //    int hasWrite = 0;
        //    using (FileStream fs = new FileStream(fileName, FileMode.Create))
        //    {
        //        while (section != null)
        //        {
        //            byte[] buff = new byte[BUFF_SIZE];
        //            int len = (int)section.Body.Length;
        //            int readCount = await section.Body.ReadAsync(buff, 0, len);
        //            await fs.WriteAsync(buff, hasWrite, readCount);
        //            //    .ContinueWith(async action =>
        //            //{
        //            //    await section.Body.FlushAsync();
        //            //});
        //            await section.Body.FlushAsync();
        //            hasWrite += readCount;
        //            section = await reader.ReadNextSectionAsync();
        //        }
        //    }
        //    return Ok(true);
        //}

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

    }
}