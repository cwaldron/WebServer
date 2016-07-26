using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WebServer
{
    /// <summary>
    /// Represents a full Url of the form scheme://hostname:port/basepath/path?query
    /// </summary>
    /// <remarks>Since this is for  internal use, and fragments are not passed to the server, fragments are not supported.</remarks>
    public sealed class Url
    {
        private string _basePath;
        private string _query;

        /// <summary>
        /// Creates an instance of the <see cref="Url" /> class
        /// </summary>
        public Url()
        {
            Scheme = "http";
            HostName = string.Empty;
            Port = null;
            BasePath = string.Empty;
            Path = string.Empty;
            QueryString = string.Empty;
        }

        /// <summary>
        /// Creates an instance of the <see cref="Url" /> class
        /// </summary>
        /// <param name="url">A <see cref="string" /> containing a URL.</param>
        public Url(string url)
        {
            var uri = new Uri(url);
            HostName = uri.Host;
            Path = uri.LocalPath;
            Port = uri.Port;
            QueryString = uri.Query;
        
            Scheme = uri.Scheme;
        }

        /// <summary>
        /// Gets or sets the HTTP protocol used by the client.
        /// </summary>
        /// <value>The protocol.</value>
        public string Scheme { get; set; }

        /// <summary>
        /// Gets the hostname of the request
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Gets the port name of the request
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        /// Gets the base path of the request i.e. the "Nancy root"
        /// </summary>
        public string BasePath
        {
            get { return _basePath; }
            set { _basePath = (value ?? string.Empty).TrimEnd('/'); }
        }

        /// <summary>
        /// Gets the path of the request, relative to the base path
        /// This property drives the route matching
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets the querystring data of the requested resource.
        /// </summary>
        public string QueryString
        {
            get { return _query; }
            set { _query = GetQuery(value); }
        }

        /// <summary>
        /// Gets the domain part of the request
        /// </summary>
        public string SiteBase => new StringBuilder()
            .Append(Scheme)
            .Append("://")
            .Append(GetHostName(HostName))
            .Append(GetPort(Port))
            .ToString();

        /// <summary>
        /// Gets whether the url is secure or not.
        /// </summary>
        public bool IsSecure => "https".Equals(Scheme, StringComparison.OrdinalIgnoreCase);

        public override string ToString()
        {
            return new StringBuilder()
                .Append(Scheme)
                .Append("://")
                .Append(GetHostName(HostName))
                .Append(GetPort(Port))
                .Append(GetCorrectPath(BasePath))
                .Append(GetCorrectPath(Path))
                .Append(QueryString)
                .ToString();
        }

        /// <summary>
        /// Clones the url.
        /// </summary>
        /// <returns>Returns a new cloned instance of the url.</returns>
        public Url Clone()
        {
            return new Url
            {
                BasePath = BasePath,
                HostName = HostName,
                Port = Port,
                QueryString = QueryString,
                Path = Path,
                Scheme = Scheme
            };
        }

        /// <summary>
        /// Casts the current <see cref="Url"/> instance to a <see cref="string"/> instance.
        /// </summary>
        /// <param name="url">The instance that should be cast.</param>
        /// <returns>A <see cref="string"/> representation of the <paramref name="url"/>.</returns>
        public static implicit operator string(Url url)
        {
            return url.ToString();
        }

        /// <summary>
        /// Casts the current <see cref="string"/> instance to a <see cref="Url"/> instance.
        /// </summary>
        /// <param name="url">The instance that should be cast.</param>
        /// <returns>An <see cref="Url"/> representation of the <paramref name="url"/>.</returns>
        public static implicit operator Url(string url)
        {
            return new Uri(url);
        }

        /// <summary>
        /// Casts the current <see cref="Url"/> instance to a <see cref="Uri"/> instance.
        /// </summary>
        /// <param name="url">The instance that should be cast.</param>
        /// <returns>An <see cref="Uri"/> representation of the <paramref name="url"/>.</returns>
        public static implicit operator Uri(Url url)
        {
            return new Uri(url.ToString(), UriKind.Absolute);
        }

        /// <summary>
        /// Casts a <see cref="Uri"/> instance to a <see cref="Url"/> instance
        /// </summary>
        /// <param name="uri">The instance that should be cast.</param>
        /// <returns>An <see cref="Url"/> representation of the <paramref name="uri"/>.</returns>
        public static implicit operator Url(Uri uri)
        {
            if (uri.IsAbsoluteUri)
            {
                return new Url
                {
                    HostName = uri.Host,
                    Path = uri.LocalPath,
                    Port = uri.Port,
                    QueryString = uri.Query,
                    Scheme = uri.Scheme
                };
            }

            return new Url { Path = uri.OriginalString };
        }

        private static string GetQuery(string query)
        {
            return string.IsNullOrEmpty(query) ? string.Empty : (query[0] == '?' ? query : '?' + query);
        }

        private static string GetCorrectPath(string path)
        {
            return (string.IsNullOrEmpty(path) || path.Equals("/")) ? string.Empty : path;
        }

        private static string GetPort(int? port)
        {
            return port.HasValue ? string.Concat(":", port.Value) : string.Empty;
        }

        private static string GetHostName(string hostName)
        {
            IPAddress address;

            if (IPAddress.TryParse(hostName, out address))
            {
                var addressString = address.ToString();

                return address.AddressFamily == AddressFamily.InterNetworkV6
                    ? $"[{addressString}]"
                    : addressString;
            }

            return hostName;
        }
    }
}
