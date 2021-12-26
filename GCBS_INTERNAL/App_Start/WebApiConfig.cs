using GCBS_INTERNAL.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApiContrib.Formatting.Jsonp;

namespace GCBS_INTERNAL
{
    public static class WebApiConfig
    {
        private static void EnableCrossSiteRequests(HttpConfiguration config)
        {
        #if DEBUG
            var cors = new EnableCorsAttribute(
                origins: "*",
                headers: "*",
                methods: "*")
            {
                SupportsCredentials = true
            };
            config.EnableCors(cors);
        #endif
        }
        public static void Register(HttpConfiguration config)
        {
            EnableCrossSiteRequests(config);
            // Web API configuration and services
            //var cors = new EnableCorsAttribute("*", "*", "*");
            //config.EnableCors(cors);
            // Web API routes
            config.MapHttpAttributeRoutes();
            var jsonpFormatter = new JsonpMediaTypeFormatter(config.Formatters.JsonFormatter);
            config.Formatters.Add(jsonpFormatter);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
