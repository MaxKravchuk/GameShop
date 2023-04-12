using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace GameShop.WebApi.Filters
{
    public class LogIPFilter : ActionFilterAttribute
    {
        private static string LogFilePath = HttpContext.Current.Server.MapPath("~/App_Data");

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string path = Path.Combine(LogFilePath, "iplogs.txt");
            string ipAddress = GetClientIpAddress(actionContext.Request);
            string logText = $"[{DateTime.UtcNow}] {ipAddress}{Environment.NewLine}";
            using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
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