using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CmsShppingCart
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("CartPartial", "Cart/{action}/{id}", new { controller = "Cart", action = "Index", id = UrlParameter.Optional }, new[] { "CmsShppingCart.Controllers" });
            routes.MapRoute("CategoryMenuPartial", "Shop/{action}/{name}", new { controller = "Shop", action = "Index", name = UrlParameter.Optional }, new[] { "CmsShppingCart.Controllers" });
            routes.MapRoute("PageMenuPartial", "Pages/PageMenuPartial", new { controller = "Pages", action = "PageMenuPartial" }, new[] { "CmsShppingCart.Controllers" });
            routes.MapRoute("Pages","{page}",new { controller="Pages",action="Index"},new [] { "CmsShppingCart.Controllers" });
            routes.MapRoute("Default", "", new { controller = "Pages", action = "Index" }, new[] { "CmsShppingCart.Controllers" });
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
