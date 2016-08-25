using System;
using System.Collections.Generic;

namespace WebServer.Routing
{
    /// <summary>
    /// The setter class for routing methods.
    /// </summary>
    public class RouteMethodSetter
    {
        private readonly List<Func<GraphNode>> _bindings = new List<Func<GraphNode>>();

        public string Method { get; }
        public RouteCollection Routes { get; }

        internal RouteMethodSetter(string method)
        {
            Method = method;
            Routes = new RouteCollection();
        }

        /// <summary>
        /// Route method setter.
        /// </summary>
        /// <param name="route">route string</param>
        /// <returns></returns>
        public Func<RouteData, object> this[string route]
        {
            // ReSharper disable once ValueParameterNotUsed
            set
            {
                Routes.MapRoute(Method, value, route);
            }
        }

        public void Initialize()
        {
        }

        public void Expiry()
        {
        }

        public void Authorize()
        {
            
        }

        //public void Initialise(IRouteEngine routeEngine)
        //{
        //    _engine = routeEngine;
        //    _parser = routeEngine.Config.StringRouteParser;

        //    foreach (var final in _baseFinals)
        //    {
        //        _engine.Base.FinalFunctions.Add(final);
        //    }

        //    foreach (var binding in _bindings)
        //    {
        //        ApplyBinding(binding);
        //    }
        //}

        //private void ApplyBinding(Func<GraphNode> o)
        //{
        //    var leaf = o();
        //    _engine.Base.Zip(leaf.Base());
        //}
    }
}
