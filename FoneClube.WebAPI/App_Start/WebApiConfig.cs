using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Web.Http.Cors;

namespace FoneClube.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SupportedMediaTypes
            .Add(new MediaTypeHeaderValue("text/html"));

            var corsBaseIonic = new EnableCorsAttribute("http://localhost:35729", "*", "*");
            var corsIonic = new EnableCorsAttribute("http://localhost:8100", "*", "*");
            var corsCardozo = new EnableCorsAttribute("http://www.rodrigocardozo.com.br", "*", "*");

            var corsAll = new EnableCorsAttribute(origins: "*",headers: "*",methods: "*");

            

            config.EnableCors(corsBaseIonic);
            config.EnableCors(corsIonic);
            config.EnableCors(corsCardozo);
            config.EnableCors(corsAll);
        }
    }
}
