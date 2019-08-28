using Microsoft.Extensions.Options;
using ModernApiDesign.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernApiDesign.Controllers
{
    public class AwesomeController
    {
        private readonly AwesomeOptions awesomeOptions;
        private readonly AwesomeOptions.BazOptions bazOptions;

        public AwesomeController(IOptions<AwesomeOptions> options, IOptions<AwesomeOptions.BazOptions> bazOptions)
        {
            awesomeOptions = options.Value;
            this.bazOptions = bazOptions.Value;
        }
    }
}
