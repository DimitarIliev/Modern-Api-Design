using Microsoft.AspNetCore.Builder;
using ModernApiDesign.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernApiDesign.ExtensionMethods
{
    public static class VowelMaskerMiddlewareExtensions
    {
        public static IApplicationBuilder UseVowelMasker(this IApplicationBuilder application)
        {
            return application.UseMiddleware<VowelMaskerMiddleware>();
        }
    }
}
