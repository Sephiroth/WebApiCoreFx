using log4net;
using log4net.Repository;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ocelot.GatewayProj.Filter
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private static ILoggerRepository repository = LogManager.CreateRepository("GatewayLogRepository");
        private static readonly ILog logger;

        static HttpGlobalExceptionFilter()
        {
            log4net.Config.XmlConfigurator.Configure(repository, new System.IO.FileInfo("Log4net.config"));
            logger = LogManager.GetLogger(repository.Name, "NETCorelog4net");
        }

        public void OnException(ExceptionContext context)
        {
            logger.Error(context.Exception);
        }
    }
}