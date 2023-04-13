using System.Web.Http;
using GameShop.WebApi.App_Start;
using Swashbuckle.Application;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace GameShop.WebApi.App_Start
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c => c.SingleApiVersion("v1", "GameShop1"))
                .EnableSwaggerUi();
        }
    }
}
