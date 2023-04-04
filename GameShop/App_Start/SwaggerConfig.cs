using System.Web.Http;
using WebActivatorEx;
using GameShop;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace GameShop
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
