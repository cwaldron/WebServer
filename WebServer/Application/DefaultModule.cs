using System;

namespace WebServer.Application
{
    internal class DefaultModule : ApplicationModule
    {
        public DefaultModule()
        {
            Get["/"] = o => new { Message = "Hello World" };

            Get["/"] = o =>
            {
                o.Authorize = s => Console.WriteLine(@"Authorize");
                o.Expiry = s => Console.WriteLine(@"Expiry");
                return new {Message = "Hello World"};
            };

            Post["Save"] = o => new { Message = "Hello World" };
        }
    }
}
