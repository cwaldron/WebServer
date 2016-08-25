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
        /// 
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public bool Match(RouteGraph graph)
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
            var parts = route.Pattern.TrimEnd('/').Split('/');
            foreach (var part in parts)
            {
                _graph.Add(new RouteSegment(part));
            }
        }

        #endregion
    }
}