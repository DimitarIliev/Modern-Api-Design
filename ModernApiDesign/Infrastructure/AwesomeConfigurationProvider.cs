using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ModernApiDesign.Infrastructure
{
    public class AwesomeConfigurationProvider : ConfigurationProvider, IConfigurationSource
    {

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return this;
        }

        private readonly string fileName;
        public AwesomeConfigurationProvider(string fileName)
        {
            this.fileName = fileName;
        }

        public override void Load()
        {
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                var doc = XDocument.Load(fs);
                var connectionStrings = doc.Root.Descendants()
                    .Where(x => x.Name.Equals(XName.Get("connectionStrings")))
                    .Descendants(XName.Get("add")).Select(x => new KeyValuePair<string, string>($"connectionStrings:{x.Attribute(XName.Get("name")).Value}",
                    x.Attribute(XName.Get("connectionString")).Value));
                var appSettings = doc.Root.Descendants()
                    .Where(x => x.Name.Equals(XName.Get("appSettings")))
                    .Descendants(XName.Get("add")).Select(x => new KeyValuePair<string, string>($"appSettings:{x.Attribute(XName.Get("key")).Value}",
                    x.Attribute(XName.Get("value")).Value));
                Data = connectionStrings.Union(appSettings).ToDictionary(x => x.Key, x => x.Value);
            }
            // ...
        }
    }
}
