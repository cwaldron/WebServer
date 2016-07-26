using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using WebServer.Sessions;
using WebServer.Utilities;

namespace WebServer
{
    public class WebRequest : IWebRequest
    {
        #region Constructors

        internal WebRequest(IWebServerContext context)
        {
            ClientCertificate = context.HttpContext.Request.GetClientCertificate();
            ProtocolVersion = context.HttpContext.Request.ProtocolVersion.ToString();
            UserHostAddress = context.HttpContext.Request.UserHostAddress;
            HttpMethod = context.HttpContext.Request.HttpMethod;
            Url = context.HttpContext.Request.Url;
            Path = Url.Path;
            Query = (IReadOnlyDictionary<string, string>) ToQueryDictionary(Url.QueryString);
            Body = context.HttpContext.Request.InputStream;
            Cookies = context.Cookies;
            Session = context.Session;
        }

        #endregion

        #region Properties

        public X509Certificate ClientCertificate { get; }
        public string ProtocolVersion { get; }
        public string UserHostAddress { get; }
        public string HttpMethod { get; }
        public Url Url { get; }
        public string Path { get; }
        public IReadOnlyDictionary<string, string> Query { get; }
        public Stream Body { get; }
        public IReadOnlyDictionary<string, Cookie> Cookies { get; }
        public Session Session { get; }

        #endregion

        #region Helpers

        /// <summary>
        /// Gets a dynamic dictionary back from a Uri query string
        /// </summary>
        /// <param name="queryString">The query string to extract values from</param>
        /// <returns>A dynamic dictionary containing the query string values</returns>
        private static IDictionary<string, string> ToQueryDictionary(string queryString)
        {
            var coll = HttpUtility.ParseQueryString(queryString);
            var ret = new Dictionary<string, string>();

            foreach (var key in coll.Keys.Where(key => key != null))
            {
                ret.AddOrUpdate(key, coll[key], (k, v) => coll[key]);
            }

            return ret;
        }

        #endregion
    }
}
