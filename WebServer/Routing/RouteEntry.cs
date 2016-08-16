using WebServer.Application;

namespace WebServer.Routing
{
    public class RouteEntry
    {
        public RouteEntry(string protocol)
        {
            
        }

        public RouteEntry(ApplicationModule module, string method, string pattern, object defaults = null)
        {
            Module = module;
            Method = method;
            Pattern = pattern;
            Defaults = defaults;
        }

        public ApplicationModule Module { get; }

        public string Method { get; }

        public string Pattern { get; }

        public object Defaults { get; }
    }
}
