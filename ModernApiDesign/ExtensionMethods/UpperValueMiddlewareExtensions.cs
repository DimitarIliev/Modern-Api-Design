using Microsoft.AspNetCore.Builder;
using ModernApiDesign.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernApiDesign.ExtensionMethods
{
    public static class UpperValueMiddlewareExtensions
    {
        public static IApplicationBuilder UseUpperValue(this IApplicationBuilder application)
        {
            return application.UseMiddleware<UpperValueMiddleware>();
        }
    }
}
