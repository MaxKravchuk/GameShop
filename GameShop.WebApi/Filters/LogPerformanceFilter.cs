using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Services.Interfaces.Utils;

namespace GameShop.WebApi.Filters
{
    public class LogPerformanceFilter : ActionFilterAttribute
    {
        private static string logFilePath = HttpContext.Current.Server.MapPath("~/App_Data");
        private Stopwatch _stopwatch;

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            _stopwatch = Stopwatch.StartNew();
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            _stopwatch.Stop();

            var loggerManager = actionExecutedContext.Request.GetConfiguration()
                .DependencyResolver.GetService(typeof(ILoggerManager)) as ILoggerManager;

            long elapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
            string logText = $"[{DateTime.UtcNow}] Elapsed time: {elapsedMilliseconds} ms{Environment.NewLine}";
            loggerManager.LogDebug(logText);

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
