using System;
using System.Collections;
using System.Collections.Generic;

namespace WebServer.Routing
{
    public class RouteCollection : IEnumerable<RouteData>
    {
        //public RouteCollection()
        //{
        //    Environment = new Dictionary<string, object>();
        //    Parameters = new DynamicDictionary();
        //}

        ///// <summary>
        ///// Maps the specified route template and sets default route values.
        ///// </summary>
        ///// <returns>
        ///// A reference to the mapped route.
        ///// </returns>
        ///// <param name="routes">A collection of routes for the application.</param><param name="name">The name of the route to map.</param><param name="routeTemplate">The route template for the route.</param><param name="defaults">An object that contains default route values.</param>
        public RouteCollection MapRoute(string name, string routeTemplate, object defaults)
        {
            return this;
        }

        //public dynamic Parameters { get; set; }

        //public IDictionary<string, object> Environment { get; set; }

        //public object Response { get; set; }

        //public Action<string> Expiry { get; set; }

        //public Action<string> Authorize { get; set; }
        //public IEnumerator<RouteData> GetEnumerator()
        //{
        //    throw new NotImplementedException();
        //}

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return GetEnumerator();
        //}
        public IEnumerator<RouteData> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
