using Microsoft.Extensions.Configuration;
using ModernApiDesign.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernApiDesign.ExtensionMethods
{
    public static class AwesomeConfigurationExtensions
    {
        public static IConfigurationBuilder AddLegacyXmlConfiguration(this IConfigurationBuilder configurationBuilder, string path)
        {
            return configurationBuilder.Add(new AwesomeConfigurationProvider(path));
        }
    }
}
