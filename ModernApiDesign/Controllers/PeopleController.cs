using Microsoft.AspNetCore.Mvc;
using ModernApiDesign.HATEOAS;
using ModernApiDesign.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernApiDesign.Controllers
{
    [Route("api/[contorller]")]
    public class PeopleController: Controller
    {
        private List<PersonDto> people = new List<PersonDto>();

        [HttpGet(Name = "get-people")]
        public IActionResult Get()
        {
            var result = new ResourceList<PersonDto>(people);
            result.Items.ForEach(x =>
            {
                x.Links.Add(new Link("self", Url.Link("get-people", new { id = x.Id }), "GET"));
                x.Links.Add(new Link("get-person", Url.Link("get-person", new { id = x.Id }), "GET"));
            });
            result.Links.Add(new Link("create-person", Url.Link("create-person", null), "POST"));
            return Ok(people);
        }

        [HttpGet("{id}", Name = "get-person")]
        public IActionResult Get(int id)
        {
            var person = people.FirstOrDefault(x => x.Id == id);
            person.Links.Add(new Link("self", Url.Link("get-person", new { id }), "GET"));
            person.Links.Add(new Link("update-person", Url.Link("update-person", new { id }), "UPDATE"));
            person.Links.Add(new Link("delete-person", Url.Link("delete-person", new { id }), "DELETE"));
            return Ok(person);
        }

        [HttpPost(Name = "create-person")]
        public IActionResult Post([FromBody]PersonDto person)
        {
            return Created("", "");
        }

        [HttpPut("{id}", Name = "update-person")]
        public IActionResult Put(int id, [FromBody]PersonDto person)
        {
            //...
            return Ok();
        }

        [HttpDelete("{id}", Name = "delete-person")]
        public IActionResult Delete(int id)
        {
            //...
            return Ok();
        }
    }
}
