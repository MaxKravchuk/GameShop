using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http.Filters;

namespace GameShop.WebApi.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private const string DefaultErrorMessage = "An error occurred while processing your request.";
        public override void OnException(HttpActionExecutedContext context)
        {
            context.Response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent(DefaultErrorMessage)
            };
        }
    }
}