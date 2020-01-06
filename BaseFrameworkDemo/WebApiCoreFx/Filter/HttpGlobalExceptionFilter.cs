using log4net;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiCoreFx.Filter
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private static ILog logger = LogManager.GetLogger(Startup.repository.Name, typeof(Startup));

        public void OnException(ExceptionContext context)
        {
            logger.Error(context.Exception);
        }
    }
}