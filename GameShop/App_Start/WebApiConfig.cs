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
            // Конфигурация и службы Web API

            // Маршруты Web API
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("Default", "api/{controller}");
        }
    }
}
