using System.Web.Http;

namespace AutoBusiness.File
{
    public static class ApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Api",
                routeTemplate: "{controller}/{action}"
            );

            FilterConfig.RegisterGlobalFilters(config.Filters);
        }
    }
}
