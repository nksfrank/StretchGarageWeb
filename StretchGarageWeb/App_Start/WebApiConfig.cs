using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace StretchGarageWeb.App_Start
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // TODO: Add any additional configuration code.

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi2",
                routeTemplate: "api/{controller}/{username}/{type}",
                defaults: new { controller = "unit",
                                username = RouteParameter.Optional,
                                type = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi3",
                routeTemplate: "api/{controller}/{id}/{latitude}/{longitude}",
                defaults: new { controller = "checklocation",
                                id = RouteParameter.Optional,
                                latitude = RouteParameter.Optional,
                                longitude = RouteParameter.Optional }
);

            // WebAPI when dealing with JSON & JavaScript!
            // Setup json serialization to serialize classes to camel (std. Json format)
            var formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            formatter.SerializerSettings.ContractResolver =
                new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();

        }
    }
}