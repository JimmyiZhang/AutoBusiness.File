using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AutoBusiness.File
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // AcrossDomain
            GlobalConfiguration.Configuration.SetCorsEngine(new WildcardCorsEngine());
            GlobalConfiguration.Configuration.EnableCors(new WildcardCorsPolicy());

            // Web
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // WebApi
            GlobalConfiguration.Configure(ApiConfig.Register);
        }
    }
}
