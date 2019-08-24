using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernApiDesign.People
{
    public class NamesApi
    {
        private readonly IPeopleRepository people;
        
        public NamesApi(IPeopleRepository peopleRepository)
        {
            people = peopleRepository;
        }

        public IActionResult Get()
        {
            //return new OkObjectResult(people.All);
        }
    }
}
