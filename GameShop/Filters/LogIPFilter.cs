using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace GameShop.Filters
{
    public class LogIPFilter : ActionFilterAttribute
    {
        private const string LogFilePath = @"C:\DiscD\ip_log.txt";

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string ipAddress = GetClientIpAddress(actionContext.Request);
            string logText = $"[{DateTime.Now}] {ipAddress}{Environment.NewLine}";
            using (var stream = new FileStream(LogFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(logText);
                }
            }


            base.OnActionExecuting(actionContext);
        }

        private string GetClientIpAddress(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return IPAddress.Parse(((HttpContextBase)request.Properties["MS_HttpContext"]).Request.UserHostAddress).ToString();
            }
            return String.Empty;
        }
    }
}