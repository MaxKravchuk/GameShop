﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace GameShop.WebApi.Filters
{
    public class LogPerformanceFilter : ActionFilterAttribute
    {
        private Stopwatch stopwatch;
        private static string LogFilePath = HttpContext.Current.Server.MapPath("~/App_Data");

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            stopwatch = Stopwatch.StartNew();
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            stopwatch.Stop();
            long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            LogPerformance(elapsedMilliseconds);

            base.OnActionExecuted(actionExecutedContext);
        }

        private void LogPerformance(long elapsedMilliseconds)
        {
            string path = Path.Combine(LogFilePath, "perfomancelogs.txt");
            string logText = $"[{DateTime.Now.ToString()}] Elapsed time: {elapsedMilliseconds} ms{Environment.NewLine}";
            File.AppendAllText(path, logText);
        }
    }
}