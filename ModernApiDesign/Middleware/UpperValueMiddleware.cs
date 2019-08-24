using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernApiDesign.Middleware
{
    public class UpperValueMiddleware
    {
        private readonly RequestDelegate next;

        public UpperValueMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var value = context.Request.Query["value"].ToString();
            context.Items["value"] = value.ToUpper();
            await next(context);
        }
    }
}
