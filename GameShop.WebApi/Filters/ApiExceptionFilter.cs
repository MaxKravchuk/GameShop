using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
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

            context.Response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent(exception == string.Empty ? DefaultErrorMessage : exception)
            };
        }
    }
}
