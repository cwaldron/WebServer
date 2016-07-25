using System;
using System.Linq;

namespace WebServer.Routing
{
    /// <summary>
    /// The <see cref="T:System.Web.Http.RouteParameter"/> class can be used to indicate properties about a route parameter (the literals and placeholders  located within segments of a <see cref="M:IHttpRoute.RouteTemplate"/>).  It can for example be used to indicate that a route parameter is optional.
    /// </summary>
    public sealed class RouteParameter
    {
        /// <summary>
        /// An optional parameter.
        /// </summary>
        public static readonly RouteParameter Optional = new RouteParameter();

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents this instance.
        /// </summary>
        /// 
        /// <returns>
        /// A <see cref="T:System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Empty;
        }
    }
}
