using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ModernApiDesign.Infrastructure
{
    public class FileHttpResponse: HttpResponse
    {
        public override HttpContext HttpContext { get; }

        public override int StatusCode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override IHeaderDictionary Headers => throw new NotImplementedException();

        public override Stream Body { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override long? ContentLength { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string ContentType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override IResponseCookies Cookies => throw new NotImplementedException();

        public override bool HasStarted => throw new NotImplementedException();

        public string Path { get; }

        public FileHttpResponse(HttpContext httpContext, string path)
        {
            var lines = File.ReadAllText(path).Split('\n');
            var request = lines[0].Split(' ');
            Path = request[1];
            HttpContext = httpContext;
        }


        public override void OnCompleted(Func<object, Task> callback, object state)
        {
            using (var reader = new StreamReader(Body))
            {
                Body.Position = 0;
                var text = reader.ReadToEnd();
                File.WriteAllText(Path, $"{StatusCode} - {text}");
                Body.Flush();
                Body.Dispose();
            }
        }

        public override void OnStarting(Func<object, Task> callback, object state)
        {
            throw new NotImplementedException();
        }

        public override void Redirect(string location, bool permanent)
        {
            throw new NotImplementedException();
        }
    }
}
