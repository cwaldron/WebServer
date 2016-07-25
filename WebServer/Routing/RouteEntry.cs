namespace WebServer.Routing
{
    public class RouteEntry
    {
        public RouteEntry(string name, string routeTemplate, object defaults)
        {
            Name = name;
            Template = routeTemplate;
            Defaults = defaults;
        }

        public string Name { get; }

        public string Template { get; }

        public object Defaults { get; }
    }
}
