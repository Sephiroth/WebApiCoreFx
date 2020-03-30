using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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
        [HttpPost]
        public async Task<IActionResult> UploadAsync()
        {
            string data = Request.Form["data"].ToString();
            string total = Request.Form["total"];
            string fileName = Request.Form["fileName"];
            string index = Request.Form["index"];
            string cacheDirectory = fileName.Split(".")[0];
            string fileFolder = $"{Directory.GetCurrentDirectory()}/wwwroot/";
            string cachePath = Path.Combine(fileFolder, cacheDirectory);//临时保存分块的目录
            bool rs = false;
            string returnName = string.Empty;
            try
            {
                if (!Directory.Exists(cachePath))
                {
                    Directory.CreateDirectory(cachePath);
                }
                string filePart = Path.Combine(cachePath, index);
                if (!string.IsNullOrEmpty(data))
                {
                    using FileStream fs = new FileStream(filePart, FileMode.Create);
                    await fs.WriteAsync(System.Text.Encoding.Default.GetBytes(data)); // Convert.FromBase64String(data)
                    rs = true;
                }
                if (total == index)
                {
                    returnName = await FileMerge(cachePath, fileName);
                    DelectDir(cachePath);//删除文件夹
                }
            }
            catch (Exception ex)
            {
                DelectDir(cachePath);//删除文件夹
                throw ex;
            }
            if (total == index)
                return Ok(returnName);
            else
                return Ok(rs);
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

        private async Task<string> FileMerge(string cachePath, string fileName)
        {
            string newName = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{fileName}";//获取文件后缀
            DirectoryInfo info = new DirectoryInfo(cachePath);
            FileInfo[] files = info.GetFiles();//获得下面的所有文件
            if (files == null || files.Length < 1)
            {
                return string.Empty;
            }

            string finalPath = Path.Combine($"{Directory.GetCurrentDirectory()}/wwwroot/", newName);
            using (FileStream fs = new FileStream(finalPath, FileMode.Create))
            {
                int offset = 0;
                foreach (FileInfo part in files.OrderBy(x => x.Name).ThenBy(x => x))//排一下序，保证从0-N Write
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(part.FullName);
                    await fs.WriteAsync(bytes, offset, bytes.Length);
                    offset += bytes.Length;
                    bytes = null;
                }
            }
            return newName;
        }

        private static void DelectDir(string srcPath)
        {
            DirectoryInfo dir = new DirectoryInfo(srcPath);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
            foreach (FileSystemInfo i in fileinfo)
            {
                if (i is DirectoryInfo)            //判断是否文件夹
                {
                    DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                    subdir.Delete(true);          //删除子目录和文件
                }
                else
                {
                    System.IO.File.Delete(i.FullName);      //删除指定文件
                }
            }
            dir.Delete();
        }

    }
}