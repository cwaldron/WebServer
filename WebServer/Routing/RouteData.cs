using System;
using System.Collections.Generic;
using WebServer.Utilities;

namespace WebServer.Routing
{
    public class RouteData
    {
        public RouteData()
        {
            Environment = new Dictionary<string, object>();
            Parameters = new DynamicDictionary();
        }

        public dynamic Parameters { get; set; }

        public IDictionary<string, object> Environment { get; set; }

        public object Response { get; set; }

        public Action<string> Expiry { get; set; }

        public Action<string> Authorize { get; set; }
    }
}
