using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ModernApiDesign.HostedServices
{
    public class AwesomeHostedService : IHostedService
    {
        //example: data synchronization. We have external service providing our app with comments data.
        //To prevent too many calls to this external service and to have the risk of being blocked,
        //we can implement a hosted service that downloads the comments locally for our app to consume safely.
        private readonly IHostingEnvironment hostingEnvironment;

        public AwesomeHostedService(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var client = new HttpClient();
            var file = $@"{hostingEnvironment.ContentRootPath}\wwwroot\comments.json";
            while (true)
            {
                var response = await client.GetAsync("https://api.external.com/comments");
                using (var output = File.OpenWrite(file))
                using (var content = await response.Content.ReadAsStreamAsync())
                {
                    content.CopyTo(output);
                }
                Thread.Sleep(60000);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}
