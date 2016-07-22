using System;
using System.Collections.Generic;

namespace WebServer.Routing
{
    public class MethodSet<T> where T : RouteData
    {
        private readonly List<Func<GraphNode>> _bindings = new List<Func<GraphNode>>();

        //private readonly List<FinalFunction> _baseFinals = new List<FinalFunction>();

        private readonly string _method;

        //private IRouteEngine _engine;

        //private IStringRouteParser _parser;

        public MethodSet(string method)
        {
            _method = method;
        }


        public Func<T, object> this[string s]
        {
            set
            {
                if (s == "/")
                {
                    //_baseFinals.Add(new ExclusiveFinalFunction(f => value(f), new MethodFilter(_method)));

                }
                else
                {
                    //_bindings.Add(
                    //    () =>
                    //    {
                    //        var node = _parser.MapToGraph(s);
                    //        node.FinalFunctions.Add(new ExclusiveFinalFunction(f => value(f), new MethodFilter(_method)));
                    //        return node;
                    //    });
                }
            }
        }

        public Func<T, object> this[GraphNode s]
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
