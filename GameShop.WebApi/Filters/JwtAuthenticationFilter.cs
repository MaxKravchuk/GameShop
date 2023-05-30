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
    public class JwtAuthenticationFilter : AuthorizationFilterAttribute
    {
        private readonly string[] _allowedRoles;

        public JwtAuthenticationFilter(params string[] allowedRoles)
        {
            _allowedRoles = allowedRoles;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var authorizationHeader = actionContext.Request.Headers.Authorization;

            if (authorizationHeader == null || authorizationHeader.Scheme != "Bearer")
            {
                if (_allowedRoles.Length == 0)
                {
                    return;
                }
                else
                {
                    HandleUnauthorizedRequest(actionContext);
                    return;
                }
            }

            var token = authorizationHeader.Parameter;
            var jwtProvider = actionContext.Request.GetConfiguration()
                .DependencyResolver.GetService(typeof(IJwtTokenProvider)) as IJwtTokenProvider;

            var principal = jwtProvider.ValidateToken(token);
            if (principal == null)
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            }

            actionContext.RequestContext.Principal = principal;

            if (_allowedRoles.Length > 0)
            {
                var userRoles = principal.Claims
                    .Where(c => c.Type == "userRole")
                    .Select(c => c.Value);

                if (!_allowedRoles.Any(role => userRoles.Contains(role)))
                {
                    HandleUnauthorizedRequest(actionContext);
                    return;
                }
            }
        }

        private void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateErrorResponse(
                HttpStatusCode.Unauthorized,
                "Unauthorized access."
            );
        }
    }
}
