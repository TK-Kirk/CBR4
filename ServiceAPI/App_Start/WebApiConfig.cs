using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ServiceAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            string origins = "http://devm.cashbackresearch.com," +
                             "http://m.cashbackresearch.com," +
                             "http://devcoreg.cashbackresearch.com," +
                             "http://testcoreg.cashbackresearch.com," +
                             "http://coreg.cashbackresearch.com," +
                             "http://apps.cashbackresearch.com";
            var cors = new EnableCorsAttribute(origins, "*", "*");
            config.EnableCors(cors);


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
