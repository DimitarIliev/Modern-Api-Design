using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernApiDesign.HATEOAS
{
    public class ResourceList<T>
    {
        public List<T> Items { get; }
        [JsonProperty("_links", Order = -2)]
        public List<Link> Links { get; } = new List<Link>();
        public ResourceList(List<T> items)
        {
            Items = items;
        }
    }
}
