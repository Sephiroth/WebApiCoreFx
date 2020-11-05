using log4net;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace WebApiCoreFx.Filter
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        #region 本地日志对象
        private static log4net.ILog Logger;
        private static log4net.Repository.ILoggerRepository repository = log4net.LogManager.CreateRepository("NETCoreRepository");
        static HttpGlobalExceptionFilter()
        {
            log4net.Config.XmlConfigurator.Configure(repository, new System.IO.FileInfo("Log4net.config"));
            Logger = log4net.LogManager.GetLogger(repository.Name, "NETCorelog4net");
        }
        #endregion

        //static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //private static readonly ILog logger = LogManager.GetLogger(Startup.LogRep.Name, typeof(Startup));

        public void OnException(ExceptionContext context)
        {
            Logger.Error(context.Exception);
        }
    }
}