using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernApiDesign.Middleware
{
    public class AwesomeRateLimiterMiddleware
    {
        private const int limit = 5;
        private readonly RequestDelegate next;
        private readonly IMemoryCache requestStore;

        public AwesomeRateLimiterMiddleware(RequestDelegate next, IMemoryCache requestStore)
        {
            this.next = next;
            this.requestStore = requestStore;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var requestKey = $"{httpContext.Request.Method}-{httpContext.Request.Path}";
            int hitCount = 0;
            var cacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30)
            };

            if (requestStore.TryGetValue(requestKey, out hitCount))
            {
                if (hitCount < limit)
                {
                    await ProcessRequest(httpContext, requestKey, hitCount, cacheEntryOptions);
                }
                else
                {
                    httpContext.Response.Headers["X-Retry-After"] = cacheEntryOptions.AbsoluteExpiration?.ToString();
                    await httpContext.Response.WriteAsync("Quota exceeded");
                }
            }
            else
            {
                await ProcessRequest(httpContext, requestKey, hitCount, cacheEntryOptions);
            }
        }

        private async Task ProcessRequest(HttpContext httpContext, string requestKey, int hitCount, MemoryCacheEntryOptions cacheEntryOptions)
        {
            hitCount++;
            requestStore.Set(requestKey, hitCount, cacheEntryOptions);
            httpContext.Response.Headers["X-Rate-Limit"] = limit.ToString();
            httpContext.Response.Headers["X-Rate-Limit-Remaining"] = (limit - hitCount).ToString();
            await next(httpContext);
        }

    }
}
