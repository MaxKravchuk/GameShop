﻿using GameShop.WebApi.App_Start;
using GameShop.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace GameShop.WebApi.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Filters.Add(new LogIPFilter());
            config.Filters.Add(new LogPerformanceFilter());
            config.Filters.Add(new ApiExceptionFilter());
            config.Routes.MapHttpRoute("Main", "api/{controller}");
        }
    }
}