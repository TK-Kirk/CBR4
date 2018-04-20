using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json.Serialization;

namespace devmapi.cashbackresearch.com
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            string origins = "http://devm.cashbackresearch.com,http://m.cashbackresearch.com";
            var cors1 = new EnableCorsAttribute(origins, "*", "*");
            config.EnableCors(cors1);

            // Web API configuration and services
            var cors = new EnableCorsAttribute("*", "*", "*");
            GlobalConfiguration.Configuration.EnableCors(cors);
            
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
