using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http.Filters;

namespace GameShop.WebApi.Filters
{
    public class WebApiOutputCacheAttribute : ActionFilterAttribute
    {
        private readonly int _cacheDurationInSeconds;
        private readonly bool _private;

        public WebApiOutputCacheAttribute(int cacheDurationInSeconds, bool isPrivate = false)
        {
            _cacheDurationInSeconds = cacheDurationInSeconds;
            _private = isPrivate;
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext == null)
            {
                throw new ArgumentNullException(nameof(actionExecutedContext));
            }

            if (actionExecutedContext.Request.Method == HttpMethod.Get)
            {
                var cacheKey = actionExecutedContext.Request.RequestUri.AbsoluteUri;

                if (HttpRuntime.Cache[cacheKey] == null)
                {
                    var cacheDuration = TimeSpan.FromSeconds(_cacheDurationInSeconds);
                    var response = actionExecutedContext.Response;

                    if (_private)
                    {
                        response.Headers.CacheControl = new CacheControlHeaderValue
                        {
                            Public = false,
                            Private = true,
                            MaxAge = cacheDuration
                        };
                    }
                    else
                    {
                        response.Headers.CacheControl = new CacheControlHeaderValue
                        {
                            Public = true,
                            MaxAge = cacheDuration
                        };
                    }

                    HttpRuntime.Cache.Insert(cacheKey, response.Content.ReadAsStringAsync().Result, null,
                        DateTime.Now.Add(cacheDuration), TimeSpan.Zero);
                }
                else
                {
                    var cachedContent = (string)HttpRuntime.Cache[cacheKey];
                    actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse();
                    actionExecutedContext.Response.Content = new StringContent(cachedContent);
                }
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
