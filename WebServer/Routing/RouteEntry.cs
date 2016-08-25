using System;
using WebServer.Application;

namespace WebServer.Routing
{
    public class RouteEntry
    {
        private readonly RouteGraph _graph;


        public RouteEntry(string protocol)
        {
            Id = Guid.NewGuid();
            Pattern = protocol;
            _graph = new RouteGraph(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="action"></param>
        /// <param name="pattern"></param>
        /// <param name="defaults"></param>
        public RouteEntry(string method, Func<RouteData, object> action, string pattern, object defaults = null)
        {
            Id = Guid.NewGuid();
            Method = method;
            Action = action;
            Pattern = pattern;
            Defaults = defaults;
            _graph = new RouteGraph(this);
        }

        public Guid Id { get; }

        public string Method { get; }

        public Func<RouteData, object> Action { get; }

        public string Pattern { get; }

        public object Defaults { get; }

        public bool Match(RouteEntry entry)
        {
            return _graph.Match(entry._graph);
        }
    }
}
