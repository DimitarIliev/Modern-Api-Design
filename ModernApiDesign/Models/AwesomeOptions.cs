using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernApiDesign.Models
{
    public class AwesomeOptions
    {
        public string Foo { get; set; }
        public string Bar { get; set; }
        public BazOptions Baz { get; set; }
        public class BazOptions
        {
            public string Foo { get; set; }
        }
    }
}
