using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModernApiDesign.ExtensionMethods;
using ModernApiDesign.Filters;
using ModernApiDesign.Formatters;
using ModernApiDesign.HostedServices;
using ModernApiDesign.Infrastructure;
using ModernApiDesign.Models;
using ModernApiDesign.People;

namespace ModernApiDesign
{
    public class Startup
    {
        //ASP.NET CORE implements DI as a first-class citizen in its infrastructure and has an Inversion of Control (IoC) container built into its core

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        //optional
        public IConfigurationRoot Configuration { get; set; }
        public Startup(Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(config =>
                {
                    config.Path = "awesomeConfig.json";
                    config.ReloadOnChange = true;
                });

            Configuration = builder.Build();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            //all the application-level dependencies are registered inside the default IoC container by adding them to an IServiceCollection
            services.AddRouting();
            services.AddSingleton<IHostedService, AwesomeHostedService>();
            services.AddSingleton<IPeopleRepository, PeopleRepository>()
                .AddMvc((o) =>
                {
                    o.Conventions.Add(new AwesomeApiControllerConvention());
                });

            services.AddMvc(options =>
            {
                options.ModelBinderProviders.Insert(0, new AwesomeModelBinderProvider());
                options.Filters.Add(typeof(TimestampFilterAttribute));
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                options.OutputFormatters.Add(new CsvOutputFormatter());
            });
            services.Configure<AwesomeOptions>(Configuration);
            services.Configure<AwesomeOptions.BazOptions>(Configuration.GetSection("baz"));
            //configuration of an app that imports all MVC bits from an external assembly from a specific folder on disk
            //var assembly = Assembly.LoadFile(@"C:\folder\mylib.dll");
            //services.AddMvc().AddApplicationPart(assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //required
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            //is responsible for the actual configuration of the application's HTTP request pipeline and is required by the runtime

            //app.Use(async (context, next) =>
            //{
            //    if (context.Request.Path == "/foo")
            //        await context.Response.WriteAsync($"Welcome to Foo");
            //    else
            //        await next();
            //});

            //app.Use(async (context, next) =>
            //{
            //    if (context.Request.Path == "/bar")
            //        await context.Response.WriteAsync($"Welcome to Bar");
            //    else
            //        await next();
            //});

            app.Map("/foo", config => config.Use(async (context, next) => await context.Response.WriteAsync("Welcome to /foo")));

            app.MapWhen(context =>
                        context.Request.Method == "POST" &&
                        context.Request.Path == "/bar",
                        config => config.Use(async (context, next) => await context.Response.WriteAsync("Welcome to /bar")));

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync($"Welcome to the default");
            });

            app.UseRouter(builder =>
            {
                //builder.MapRoute(string.Empty, context =>
                //{
                //    return context.Response.WriteAsync($"Welcome to the default route!");
                //});

                builder.MapGet("foo/{name}/{surname?}", (request, response, routeData) =>
                {
                    return response.WriteAsync($"Welcome to Foo, {routeData.Values["name"]} {routeData.Values["surename"]}");
                });

                builder.MapPost("bar/{number:int}", (request, response, routeData) =>
                {
                    return response.WriteAsync($"Welcome to Bar, number is {routeData.Values["number"]}");
                });

                builder.MapRoute(string.Empty, context =>
                {
                    var routeValues = new RouteValueDictionary
                    {
                        { "number", 456 }
                    };

                    var vpc = new VirtualPathContext(context, null, routeValues, "bar/{number:int}");
                    var route = builder.Routes.Single(x => x.ToString().Equals(vpc.RouteName));
                    var barUrl = route.GetVirtualPath(vpc).VirtualPath;

                    return context.Response.WriteAsync($"URL: {barUrl}");
                });
            });
            app.UseMvc();
        }

        //Middleware configuration
        //public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        //{
        //    app.Map("/skip", (skipApp) => skipApp.Run(async (context) => await context.Response.WriteAsync($"Skip the line!")));
        //    app.UseNumberChecker();
        //    app.UseUpperValue();
        //    app.UseVowelMasker();
        //    app.Run(async (context) =>
        //    {
        //        var value = context.Items["value"].ToString();
        //        await context.Response.WriteAsync($"You entered a string: {value}");
        //    });
        //}
    }
}
