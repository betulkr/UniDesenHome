using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Unibim.Melanom.WebUI.Models.EntityModel;

namespace Unibim.Melanom.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();

            //routes.Add("UrunDetay", new GetSEOFriendlyRoute("UrunDetay/{id}",
            //new RouteValueDictionary(new { controller = "Kurumsal", action = "UrunDetay" }),
            //new MvcRouteHandler()));

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Kurumsal", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
