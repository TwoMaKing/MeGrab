using Eagle.Core.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.Mvc;
using System.Web.Routing;

namespace MeGrab.Login
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Account", action = "Login", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        public static void RegisterWebApiRoutes(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "GetTokenCredential",
                routeTemplate: "api/{controller}/{action}/{token}",
                defaults: new { token = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            RegisterWebApiRoutes(GlobalConfiguration.Configuration);

            AppRuntime.Instance.Create();
        }
    }
}