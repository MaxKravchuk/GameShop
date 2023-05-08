using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using FluentValidation;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services.Interfaces.Utils;

namespace GameShop.WebApi.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private const string DefaultErrorMessage = "An error occurred while processing your request.";

        public override void OnException(HttpActionExecutedContext context)
        {
            var loggerManager = context.Request.GetConfiguration()
                .DependencyResolver.GetService(typeof(ILoggerManager)) as ILoggerManager;

            var controllerName = context.ActionContext.ControllerContext.ControllerDescriptor.ControllerType.FullName;
            var actionName = context.ActionContext.ActionDescriptor.ActionName;
            var exception = context.Exception.Message;
            var errorMessage = $"{controllerName}.{actionName}: {exception}";

            loggerManager.LogError(errorMessage);

            if (context.Exception is ValidationException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.Forbidden)
                {
                    Content = new StringContent(context.Exception.Message)
                };
            }
            else if (context.Exception is NotFoundException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(context.Exception.Message)
                };
            }
            else if (context.Exception is BadRequestException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(context.Exception.Message)
                };
            }
            else
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(exception == string.Empty ? DefaultErrorMessage : exception)
                };
            }
        }
    }
}
