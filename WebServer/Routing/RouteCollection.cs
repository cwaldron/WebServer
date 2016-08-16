using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace WebServer.Routing
{
    public class RouteCollection : IEnumerable<RouteEntry>
    {
        private readonly ConcurrentDictionary<string, RouteEntry> _routeEntries;

        public RouteCollection()
        {
            _routeEntries = new ConcurrentDictionary<string, RouteEntry>();
        }

        /// <summary>
        /// Maps the specified route template and sets default route values.
        /// </summary>
        /// <param name="name">The name of the route to map.</param>
        /// <param name="routeTemplate">The route template for the route.</param>
        /// <param name="defaults">An object that contains default route values.</param>
        /// <returns>A reference to the mapped route.</returns>
        public RouteCollection MapRoute(string name, string routeTemplate, object defaults = null)
        {
            //var entry = new RouteEntry(name, routeTemplate, defaults);
            //_routeEntries.AddOrUpdate(name, entry, (k, v) => entry);
            return this;
        }

        public IEnumerator<RouteEntry> GetEnumerator()
        {
            return _routeEntries.Select(pair => pair.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
