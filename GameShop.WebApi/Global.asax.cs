using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using GameShop.WebApi.App_Start;
using log4net.Config;

namespace GameShop.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/Log4Net.config")));
        }
    }
}
