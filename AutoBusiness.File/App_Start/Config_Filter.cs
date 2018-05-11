using AutoBusiness.Library.Web.Api;
using NLog;
using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace AutoBusiness.File
{
    public class FilterConfig
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static void RegisterGlobalFilters(HttpFilterCollection filters)
        {
            WebApiExceptionFilter custom = new WebApiExceptionFilter();
            custom.FoundException += Custom_FoundException;

            filters.Add(custom);
        }

        private static void Custom_FoundException(Exception info, HttpActionContext context)
        {
            logger.Error(info);
        }
    }
}
