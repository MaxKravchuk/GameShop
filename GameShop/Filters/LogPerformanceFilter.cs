using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace GameShop.Filters
{
    public class LogPerformanceFilter : ActionFilterAttribute
    {
        private Stopwatch stopwatch;
        private const string LogFilePath = @"C:\DiscD\spead_log.txt";

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            stopwatch = Stopwatch.StartNew();
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            stopwatch.Stop();
            long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            // TODO: Implement logging logic here, e.g. write to a text file or a database
            LogPerformance(elapsedMilliseconds);

            base.OnActionExecuted(actionExecutedContext);
        }

        private void LogPerformance(long elapsedMilliseconds)
        {
            string logText = $"[{DateTime.Now.ToString()}] Elapsed time: {elapsedMilliseconds} ms{Environment.NewLine}";
            File.AppendAllText(LogFilePath, logText);
        }
    }
}