using System;
using System.Collections;
using System.Collections.Generic;

namespace WebServer.Routing
{
    internal class RouteGraph : ICollection<RouteSegment>
    {
        private readonly LinkedList<RouteSegment> _graph;

        public int Count => _graph.Count;

        public bool IsReadOnly => false;

        public RouteEntry Route { get; }

        private RouteGraph()
        {
            _graph = new LinkedList<RouteSegment>();
        }

        public RouteGraph(RouteEntry route)
            : this()
        {
            Route = route;
            BuildGraph(route);
        }

        #region Methods

        /// <summary>
        /// Match the route pattern to the route.
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        public RouteGraph PatternMatch(RouteSegment route)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool PatternMatch(string path)
        {
            return true;
        }

        public IEnumerator<RouteSegment> GetEnumerator()
        {
            return _graph.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(RouteSegment item)
        {
            _graph.Add(item);
        }

        public void Clear()
        {
            _graph.Clear();
        }

        public bool Contains(RouteSegment item)
        {
            return _graph.Contains(item);
        }

        public void CopyTo(RouteSegment[] array, int arrayIndex)
        {
            _graph.CopyTo(array, arrayIndex);
        }

        public bool Remove(RouteSegment item)
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