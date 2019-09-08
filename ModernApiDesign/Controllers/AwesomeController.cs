using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Options;
using ModernApiDesign.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernApiDesign.Controllers
{
    [Authorize]
    [EnableCors("AwesomePolicy")]
    public class AwesomeController
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
    }
}
