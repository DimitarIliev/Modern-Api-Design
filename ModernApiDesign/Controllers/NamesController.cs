using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernApiDesign.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/[controller]")]
    public class NamesController: Controller
    {
        [MapToApiVersion("1.0")]
        public IActionResult Get()
        {
            return Ok();
        }

        [MapToApiVersion("2.0")]
        public IActionResult Put(string name)
        {
            return Ok();
        }
    }
}
