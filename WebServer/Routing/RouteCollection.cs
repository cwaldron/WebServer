using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WebServer.Application;

namespace WebServer.Routing
{
    public class RouteCollection : ICollection<RouteEntry>
    {
        private readonly ConcurrentDictionary<Guid, RouteEntry> _routeEntries;
        private readonly ApplicationModule _module;

        public int Count => _routeEntries.Count;

        public bool IsReadOnly => false;

        internal RouteCollection()
        {
            _routeEntries = new ConcurrentDictionary<Guid, RouteEntry>();
        }

        public RouteCollection(ApplicationModule module)
            : this()
        {
            _module = module;
            foreach (var route in module.Setters.SelectMany(setter => setter.Routes))
            {
                Add(route);
            }
        }

        /// <summary>
        /// Maps the specified route template and sets default route values.
        /// </summary>
        /// <param name="method">The name of the route to map.</param>
        /// <param name="pattern">The matching route pattern.</param>
        /// <param name="defaults">An object that contains default route values.</param>
        /// <returns>A reference to the mapped route.</returns>
        public RouteCollection MapRoute(string method, string pattern, object defaults = null)
        {
            Add(new RouteEntry(method, null, pattern, defaults));
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

        public void Add(RouteEntry item)
        {
            _routeEntries.AddOrUpdate(item.Id, item, (k, v) => item);
        }

        public void Clear()
        {
            _routeEntries.Clear();
        }

        public bool Contains(RouteEntry item)
        {
            return _routeEntries.ContainsKey(item.Id);
        }

        public void CopyTo(RouteEntry[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(RouteEntry item)
        {
            RouteEntry prevItem;
            return _routeEntries.TryRemove(item.Id, out prevItem);
        }
    }
}
