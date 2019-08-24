using Microsoft.AspNetCore.Builder;
using ModernApiDesign.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernApiDesign.ExtensionMethods
{
    public static class NumberCheckerMiddlewareExtensions
    {
        public static IApplicationBuilder UseNumberChecker(this IApplicationBuilder application)
        {
            return application.UseMiddleware<NumberCheckerMiddleware>();
        }
    }
}
