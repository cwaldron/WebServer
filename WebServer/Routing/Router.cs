using System;
using System.Linq;
using WebServer.Application;

namespace WebServer.Routing
{
    internal class Router : SingletonBase<Router>
    {
        private readonly RouteCollection _routes;
        private readonly ModuleCollection _modules;


        private Router()
        {
            _routes = new RouteCollection();
            _modules = new ModuleCollection();
        }

        public void RegisterRoute(RouteEntry route)
        {
            
        }

        public static void RegisterModule(ApplicationModule module)
        {
            Instance._modules.Add(module);
        }

        public RouteGraph ResolveRoute(RouteEntry route)
        {
            return null;
        }

        public GraphNode MapToGraph(string routePattern)
        {
            if (string.IsNullOrEmpty(routePattern))
            {
                throw new ArgumentException(@"Route pattern cannot be null", nameof(routePattern));
            }

            if (routePattern == "/")
            {
                return null;
            }

            var parts = routePattern.Split('/').Where(o => !string.IsNullOrEmpty(o.Trim()));

            GraphNode node = null;
            foreach (var part in parts)
            {
                var thisNode = new ConstantNode(part);
                node?.Slash(thisNode);
                node = thisNode;
            }

            return node;
        }
    }
}
