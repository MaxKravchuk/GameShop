using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using GameShop.BLL.Services.Interfaces.Utils;

namespace GameShop.WebApi.Filters
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute
    {

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var token = actionContext.Request.Headers.Authorization?.Parameter;
            var jwtProvider = actionContext.Request.GetConfiguration()
                .DependencyResolver.GetService(typeof(IJwtTokenProvider)) as IJwtTokenProvider;

            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            var principal = jwtProvider.ValidateToken(token);
            if (principal == null)
            {
                return false;
            }

            return true;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateErrorResponse(
                HttpStatusCode.Unauthorized,
                "Unauthorized access."
            );
        }
    }
}
