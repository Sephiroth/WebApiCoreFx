using log4net;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiCoreFx.Filter
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        #region 本地日志对象
        //private static log4net.ILog Logger;
        //private static log4net.Repository.ILoggerRepository repository = log4net.LogManager.CreateRepository("NETCoreRepository");
        //static HttpGlobalExceptionFilter()
        //{
        //    log4net.Config.XmlConfigurator.Configure(repository, new System.IO.FileInfo("Log4net.config"));
        //    Logger = log4net.LogManager.GetLogger(repository.Name, "NETCorelog4net");
        //}
        #endregion

        private static readonly ILog logger = LogManager.GetLogger(Startup.LogRep.Name, typeof(Startup));

        public void OnException(ExceptionContext context)
        {
            logger.Error(context.Exception);
        }
    }
}