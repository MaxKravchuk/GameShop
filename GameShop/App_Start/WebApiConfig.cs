using GameShop.App_Start;
using GameShop.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace GameShop
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("Default", "api/{controller}");
            config.Filters.Add(new LogIPFilter());
        }
    }
}
