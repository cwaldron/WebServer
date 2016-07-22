using System;
using System.Net;
using WebServer;

namespace ServerApp
{
    public class ServerModule : Module
    {
        public ServerModule()
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
