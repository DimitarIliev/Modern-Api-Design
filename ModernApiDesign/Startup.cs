﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ModernApiDesign
{
    public class Startup
    {
        //ASP.NET CORE implements DI as a first-class citizen in its infrastructure and has an Inversion of Control (IoC) container built into its core

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        //optional
        public void ConfigureServices(IServiceCollection services)
        {
            //all the application-level dependencies are registered inside the default IoC container by adding them to an IServiceCollection
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //required
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
        }
    }
}