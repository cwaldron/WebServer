using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WebServer.Application;

namespace WebServer.Routing
{
    public class RouteCollection : IEnumerable<RouteEntry>
    {
        #region Private Members

        private readonly ConcurrentDictionary<Guid, RouteEntry> _routeEntries;

        #endregion

        #region Automatic Properties

        public int Count => _routeEntries.Count;

        public ApplicationModule Module { get; }

        #endregion

        #region Constructors

        public RouteCollection(ApplicationModule module = null)
        {
            _routeEntries = new ConcurrentDictionary<Guid, RouteEntry>();
            Module = module ?? new DefaultModule();
            foreach (var route in Module.Setters.SelectMany(x => x.Routes))
            {
                Add(route);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Maps the specified route template and sets default route values.
        /// </summary>
        /// <param name="method">The name of the route to map.</param>
        /// <param name="pattern">The matching route pattern.</param>
        /// <param name="defaults">An object that contains default route values.</param>
        /// <returns>A reference to the mapped route.</returns>
        public RouteCollection MapRoute(string method, string pattern, object defaults = null)
        {
            return MapRoute(method, null, pattern, defaults);
        }

        public RouteCollection MapRoute(string method, Func<RouteData, object> action, string pattern, object defaults = null)
        {
            Add(new RouteEntry(method, action, pattern, defaults));
            return this;
        }

        public RouteEntry FindRoute(string pattern)
        {
            var entry = new RouteEntry(pattern);
            var found = _routeEntries.FirstOrDefault(x => x.Value.Match(entry));
            return !found.IsDefault() ? found.Value : null;
        }

        public IEnumerator<RouteEntry> GetEnumerator()
        {
            return _routeEntries.Select(pair => pair.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Helpers

        protected void Add(RouteEntry item)
        {
            _routeEntries.AddOrUpdate(item.Id, item, (k, v) => item);
        }

        protected void Clear()
        {
            _routeEntries.Clear();
        }

        protected bool Contains(RouteEntry item)
        {
            return _routeEntries.ContainsKey(item.Id);
        }

        protected bool Remove(RouteEntry item)
        {
            RouteEntry prevItem;
            return _routeEntries.TryRemove(item.Id, out prevItem);
        }

        #endregion
    }
}
