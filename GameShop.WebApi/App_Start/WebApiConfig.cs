using System.Web.Http;
using System.Web.Http.Cors;
using GameShop.WebApi.Filters;

namespace GameShop.WebApi.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            UnityConfig.RegisterComponents(config);

            config.MapHttpAttributeRoutes();
            config.Filters.Add(new LogIPFilter());
            config.Filters.Add(new LogPerformanceFilter());
            config.Filters.Add(new ApiExceptionFilter());

            config.Routes.MapHttpRoute("Main", "api/{controller}");

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
        }
    }
}
