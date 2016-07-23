using System;
using System.Linq;

namespace WebServer.Routing
{
    public class RouteParser
    {
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
