using GraphQL.Types;
using ModernApiDesign.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernApiDesign.Infrastructure
{
    public class PersonQuery: ObjectGraphType
    {
        public PersonQuery(IPersonRepository repo)
        {
            Field<PersonType>("person", arguments: new QueryArguments(new QueryArgument<IntGraphType>() { Name = "id" }),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    return repo.GetOne(id);
                });

            Field<ListGraphType<PersonType>>("people", resolve: context =>
            {
                return repo.GetAll();
            });
        }
    }
}
