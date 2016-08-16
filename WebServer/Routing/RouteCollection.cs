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

        public RouteCollection()
        {
            _routeEntries = new ConcurrentDictionary<Guid, RouteEntry>();
        }

        public RouteCollection(ApplicationModule module)
            : this()
        {
            _module = module;
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
            throw new System.NotImplementedException();
        }

        public bool Remove(RouteEntry item)
        {
            RouteEntry prevItem;
            return _routeEntries.TryRemove(item.Id, out prevItem);
        }
    }
}
