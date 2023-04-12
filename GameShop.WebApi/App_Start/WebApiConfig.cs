using GameShop.BLL.Services.Interfaces;
using GameShop.WebApi.App_Start;
using GameShop.WebApi.Filters;
using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Unity;
using Unity.AspNet.WebApi;

namespace GameShop.WebApi.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Filters.Add(new LogIPFilter());
            config.Filters.Add(new LogPerformanceFilter());

            var container = new UnityContainer();
            UnityConfig.RegisterTypes(container);

            config.Filters.Add(new ApiExceptionFilter(container));
            config.Routes.MapHttpRoute("Main", "api/{controller}");
        }
    }
}
