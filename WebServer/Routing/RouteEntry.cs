using System;
using WebServer.Application;

namespace WebServer.Routing
{
    public class RouteEntry
    {
        public RouteEntry(string protocol)
        {
            
        }

        public RouteEntry(string method, Func<RouteData, object> action, string pattern, object defaults = null)
        {
            Id = Guid.NewGuid();
            Method = method;
            Action = action;
            Pattern = pattern;
            Defaults = defaults;
        }

        public Guid Id { get; }

        public string Method { get; }

        public Func<RouteData, object> Action { get; }

        public string Pattern { get; }

        public object Defaults { get; }
    }
}
