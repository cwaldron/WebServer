using System;
using System.Collections;
using System.Collections.Generic;

namespace WebServer.Routing
{
    internal class RouteGraph : ICollection<RouteEntry>
    {
        private readonly LinkedList<RouteEntry> _graph;

        public int Count => _graph.Count;

        public bool IsReadOnly => false;

        private RouteGraph()
        {
            _graph = new LinkedList<RouteEntry>();
        }

        public RouteGraph(RouteEntry route)
            : this()
        {
            BuildGraph(route);
        }

        #region Methods

        /// <summary>
        /// Match the route pattern to the route.
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        public RouteGraph PatternMatch(RouteEntry route)
        {
            return null;
        }

        public IEnumerator<RouteEntry> GetEnumerator()
        {
            return _graph.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(RouteEntry item)
        {
            _graph.Add(item);
        }

        public void Clear()
        {
            _graph.Clear();
        }

        public bool Contains(RouteEntry item)
        {
            return _graph.Contains(item);
        }

        public void CopyTo(RouteEntry[] array, int arrayIndex)
        {
            _graph.CopyTo(array, arrayIndex);
        }

        public bool Remove(RouteEntry item)
        {
            return _graph.Remove(item);
        }

        #endregion

        #region Helpers

        private void BuildGraph(RouteEntry route)
        {
            Console.WriteLine(route.Method);
            Console.WriteLine(route.Pattern);
        }

        #endregion
    }
}