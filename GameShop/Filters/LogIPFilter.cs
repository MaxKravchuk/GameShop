using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameShop.Filters
{
    public class LogIPFilter : ActionFilterAttribute
    {
        private const string LogFilePath = @"C:\DiscD\ip_log.txt";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                string ipAddress = filterContext.HttpContext.Request.UserHostAddress;
                string logText = $"[{DateTime.Now}] {ipAddress}{Environment.NewLine}";
                using (var stream = new FileStream(LogFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.Write(logText);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as appropriate
                Console.WriteLine($"An error occurred while logging IP address: {ex.Message}");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}