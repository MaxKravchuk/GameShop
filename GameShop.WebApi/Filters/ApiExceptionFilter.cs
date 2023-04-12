using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http.Filters;
using GameShop.BLL.Services.Interfaces;
using log4net;
using System.Runtime.InteropServices;
using Unity;

namespace GameShop.WebApi.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private const string DefaultErrorMessage = "An error occurred while processing your request.";
        private readonly UnityContainer _container;

        public ApiExceptionFilter(UnityContainer container)
        {
            _container = container;
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            var _loggerManager = _container.Resolve<ILoggerManager>();

            var controllerName = context.ActionContext.ControllerContext.ControllerDescriptor.ControllerType.FullName;
            var actionName = context.ActionContext.ActionDescriptor.ActionName;
            var exception = context.Exception.Message;
            var errorMessage = $"{controllerName}.{actionName}: {exception}";

            _loggerManager.LogError(errorMessage);

            context.Response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent(exception == string.Empty ? DefaultErrorMessage : exception)
            };
        }
    }
}