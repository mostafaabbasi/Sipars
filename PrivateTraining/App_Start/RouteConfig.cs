using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace PrivateTraining
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //api
            //routes.MapMvcAttributeRoutes(routes);
//            routes.MapHttpRoute(
//                name: "API Default",
//                routeTemplate: "v1/{controller}/{action}/{id}",
//                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
//            );
//            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


            //----------------------
            //routes.MapRoute(
            //    name: "500-Error",
            //    url: "Error",
            //    defaults: new { controller = "Error", action = "Error" }
            //);

            //-------------------------------
        }
    }
}