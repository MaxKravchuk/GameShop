using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Services.Interfaces.Utils;

namespace GameShop.WebApi.Filters
{
    public class LogIPFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var loggerManager = actionContext.Request.GetConfiguration()
                .DependencyResolver.GetService(typeof(ILoggerManager)) as ILoggerManager;

            string ipAddress = GetClientIpAddress(actionContext.Request);
            string logText = $"[{DateTime.UtcNow}] {ipAddress}{Environment.NewLine}";
            loggerManager.LogDebug(logText);
            base.OnActionExecuting(actionContext);
        }

        private string GetClientIpAddress(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return IPAddress
                    .Parse(((HttpContextBase)request.Properties["MS_HttpContext"]).Request.UserHostAddress).ToString();
            }

            return string.Empty;
        }
    }
}
