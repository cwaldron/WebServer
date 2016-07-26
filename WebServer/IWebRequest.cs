using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using WebServer.Sessions;

namespace WebServer
{
    public interface IWebRequest
    {
        /// <summary>
        /// Gets the certificate sent by the client.
        /// </summary>
        X509Certificate ClientCertificate { get; }

        /// <summary>
        /// Gets the HTTP protocol version.
        /// </summary>
        string ProtocolVersion { get; }

        /// <summary>
        /// Gets the IP address of the client
        /// </summary>
        string UserHostAddress { get; }

        /// <summary>
        /// Gets or sets the HTTP data transfer method used by the client.
        /// </summary>
        /// <value>The method.</value>
        string HttpMethod { get; }

        /// <summary>
        /// Gets the url
        /// </summary>
        Url Url { get; }

        /// <summary>
        /// Gets the request path, relative to the base path.
        /// Used for route matching etc.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Gets the query string data of the requested resource.
        /// </summary>
        IReadOnlyDictionary<string, string> Query { get; }

        /// <summary>
        /// Gets a <see cref="Stream"/> that can be used to read the incoming HTTP body
        /// </summary>
        Stream Body { get; }

        /// <summary>
        /// Gets the request cookies.
        /// </summary>
        IReadOnlyDictionary<string, Cookie> Cookies { get; }

        /// <summary>
        /// Gets the current session.
        /// </summary>
        Session Session { get; }
    }
}
