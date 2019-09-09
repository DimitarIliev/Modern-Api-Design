using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ModernApiDesign.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernApiDesign.Controllers.V1
{
    [Authorize]
    [ApiVersion("1.0")]
    [EnableCors("AwesomePolicy")]
    [Route("api/v{version:apiVersion}/awesome")]
    public class AwesomeController : Controller
    {
        private readonly AwesomeOptions awesomeOptions;
        private readonly AwesomeOptions.BazOptions bazOptions;

        //IOptions<T> doesn't reflect config change
        //Instead we should use IOptionsSnapshot<T>
        public AwesomeController(IOptionsSnapshot<AwesomeOptions> options, IOptionsSnapshot<AwesomeOptions.BazOptions> bazOptions)
        {
            awesomeOptions = options.Value;
            this.bazOptions = bazOptions.Value;
        }

        [EnableCors("AnotherAwesomePolicy")]
        public IEnumerable<string> Get() => new List<string>();

        [DisableCors]
        public string Post() => "";

        public IActionResult GetVersion() => Ok("Version 1");
    }
}

namespace ModernApiDesign.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/awesome")]
    public class AwesomeController : Controller
    {
        public IActionResult Get() => Ok($"Version 2 - {Request.HttpContext.Connection.Id}");
    }
}

