using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using static ApiTest2.Services.UserServices;

namespace ApiTest2
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Add JWT Middleware
            config.MessageHandlers.Add(new JwtMiddleware());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
