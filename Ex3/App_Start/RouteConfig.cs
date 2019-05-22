using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ex3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("display", "display/{ip}/{port}/{interval}",
                defaults: new { controller = "Info", action = "display", interval="0"});
            routes.MapRoute("index", "", 
                defaults: new {controller= "info", action = "index"});

           
        }
    }
}
