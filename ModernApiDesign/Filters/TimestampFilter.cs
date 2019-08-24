using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernApiDesign.Filters
{
    public class TimestampFilterAttribute : Attribute, IActionFilter, IAsyncActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var ts = DateTime.Parse(context.ActionDescriptor.RouteValues["timestamp"]).AddHours(1).ToString();
            context.HttpContext.Request.Headers["X-EXPIRY-TIMESTAP"] = ts;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.ActionDescriptor.RouteValues["timestamp"] = DateTime.Now.ToString();
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            this.OnActionExecuting(context);
            var resultContext = await next();
            this.OnActionExecuted(resultContext);
        }
    }
}
