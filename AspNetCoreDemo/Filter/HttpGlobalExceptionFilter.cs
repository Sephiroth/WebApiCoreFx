using log4net;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreDemo.Filter
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private static ILog logger = LogManager.GetLogger(Startup.repository.Name, typeof(Startup));

        public void OnException(ExceptionContext context)
        {
            logger.Error(context.Exception);
            //throw new NotImplementedException();
        }
    }
}