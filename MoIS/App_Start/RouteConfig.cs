using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MoIS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Encrypt",
                url: "{controller}/{action}/{eId}",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Pdf",
                url: "{controller}/{action}/{id}/{page}",
                defaults: new { controller = "Home", action = "PrintViewToPdf", id = UrlParameter.Optional, page = "" }
            );
        }
    }
}
