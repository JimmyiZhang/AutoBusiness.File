﻿using System.Web.Mvc;
using System.Web.Routing;

namespace AutoBusiness.File
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Web",
                url: "web/{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}
