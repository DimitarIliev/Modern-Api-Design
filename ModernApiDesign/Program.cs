using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using ModernApiDesign.ExtensionMethods;
using ModernApiDesign.Models;

namespace ModernApiDesign
{
    public class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static void Main(string[] args)
        {
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("awesomeConfig.json")
            //    .AddJsonFile("awesomeConfig2.json");

            //Configuration = builder.Build();

            //foreach (var item in Configuration.AsEnumerable())
            //{
            //    Console.WriteLine($"Key: {item.Key}, Value: {item.Value}");
            //}
            //Console.ReadKey();

            //load configuration from multiple sources and formats
            //var someSettings = new Dictionary<string, string>()
            //{
            //    { "poco:key1", "value 1" },
            //    { "poco:key2", "value 2" }
            //};
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("awesomeConfig.json")
            //    .AddJsonFile("awesomeConfig2.json")
            //    .AddXmlFile("awesomeConfig.xml")
            //    .AddIniFile("awesomeConfig.ini")
            //    .AddCommandLine(args)
            //    .AddEnvironmentVariables()
            //    .AddInMemoryCollection(someSettings)
            //    .AddUserSecrets("awesomeSecrets");

            //var config = builder.Build();

            //builder.AddAzureKeyVault(config["AzureKeyVault:url"], config["AzureKeyVault:clientId"], config["AzureKeyVault:secret"]);

            //Configuration = builder.Build();

            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddLegacyXmlConfiguration("web.config");
            //add data protection
            var services = new ServiceCollection()
                .AddDataProtection()
                .Services.BuildServiceProvider();

            var protectedProvider = services.GetService<IDataProtectionProvider>();
            var protector = protectedProvider.CreateProtector("AwesomePurpose")
                .ToTimeLimitedDataProtector();

            DateTimeOffset expiryDate;
            try
            {
                Console.Write($"Type something sensitive: ");
                var input = Console.ReadLine();
                var protectedInput = protector.Protect(input, TimeSpan.FromSeconds(10));
                Console.WriteLine($"Protected: {protectedInput}");
                var unprotectedInput = protector.Unprotect(protectedInput, out expiryDate);
                Console.WriteLine($"Unprotected: {unprotectedInput}");
                Console.WriteLine();
            }
            catch (CryptographicException exception)
            {
                Console.WriteLine(exception.Message);
            }
            finally
            {
                Thread.Sleep(1000);
            }
            //
            //adding logger
            new WebHostBuilder()
                .UseKestrel()
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole(options => options.IncludeScopes = true);
                    //logging.SetMinimumLevel(LogLevel.Information);
                    logging.SetMinimumLevel(LogLevel.None)
                    .AddFilter("Default", LogLevel.Error)
                    .AddFilter<ConsoleLoggerProvider>("Program.Startup", LogLevel.Critical);

                    logging.AddFilter(s => s == LogLevel.Warning)
                    .AddFilter<ConsoleLoggerProvider>(s => s == LogLevel.Information);
                })
                .Build()
                .Run();
            //
            new WebHostBuilder()
              .UseKestrel()
              .UseStartup<Startup>()
              .UseContentRoot(Directory.GetCurrentDirectory())
              .ConfigureAppConfiguration((context, config) =>
              {
                  config.AddJsonFile("logger.config.json");
              })
              .ConfigureLogging((context, logging) =>
              {
                  var config = context.Configuration.GetSection("Logging");
                  logging.AddConfiguration(config);
                  logging.AddConsole();
              })
              .Build()
              .Run();
            //
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("awesomeConfig.json");

            var awesomeOptions = new AwesomeOptions();
            builder.Build().Bind(awesomeOptions);

            CreateWebHostBuilder(args).Build().Run();
        }

        //Uses Builder pattern to create Web Host
        //Web host is responsible for the bootstrapping, initialization, and lifetime management of applications
        //for a web app to run it requires 1 host with at least 1 server for serving requests & responses.
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseAwesomeServer(o => o.FolderPath = @"c:\sandbox\in")
                .UseStartup<Startup>().Build();

        //public static void Main(string[] args)
        //{
        //    CreateWebHostBuilder(args).Run();
        //}

        //public static IWebHost CreateWebHostBuilder(string[] args) =>
        //    new WebHostBuilder()
        //        .UseKestrel()
        //        .UseContentRoot(Directory.GetCurrentDirectory())
        //        .ConfigureAppConfiguration(config => config.AddJsonFile("appsettings.json", true))
        //        .ConfigureLogging(logging =>
        //        logging.AddConsole().AddDebug())
        //        .UseIISIntegration()
        //        .UseStartup<Startup>()
        //        .Build();

        //we can use different startup classes for different environments
        //configure services without startup.cs
        //public static IWebHost CreateWebHostBuilder(string[] args)
        //{
        //    WebHost.CreateDefaultBuilder()
        //        .ConfigureServices(services => services.AddSingleton<IComponent, ComponentB>())
        //        .Configure(app =>
        //        {
        //            ...
        //        });
        //}
    }
}
