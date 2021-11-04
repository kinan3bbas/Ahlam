﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Security.Claims;
using Microsoft.AspNet.Identity.EntityFramework;
using Ahlam.Models;
//using Microsoft.AspNet.OData.Builder;
//using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNet.OData;

using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;


namespace Ahlam
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<ServicePath>("ServicePaths");
            builder.EntitySet<ApplicationUser>("ApplicationUsers");
            builder.EntitySet<Dream>("Dreams");
            builder.EntitySet<Payment>("Payments");
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());

        }
    }
}
