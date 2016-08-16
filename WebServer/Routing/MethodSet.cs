using System;
using System.Collections.Generic;
using WebServer.Application;

namespace WebServer.Routing
{
    public class MethodSet
    {
        private readonly List<Func<GraphNode>> _bindings = new List<Func<GraphNode>>();

        private readonly List<RouteEntry> _routeCollection;

        public string Method { get; }

        public ApplicationModule Module { get; }

        internal MethodSet(string method, ApplicationModule module)
        {
            Method = method;
            Module = module;
            _routeCollection = new List<RouteEntry>();
        }

        public Func<RouteData, object> this[string s]
        {
            // ReSharper disable once ValueParameterNotUsed
            set
            {
                var route = new RouteEntry(Module, Method, s);
                _routeCollection.Add(route);
            }
        }

        public Func<RouteData, object> this[GraphNode s]
        {
            set
            {
                s = s*(f => value(f));
                _bindings.Add(() => s.Base());
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
