using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using WebServer.Sessions;
using WebServer.Utilities;
using Cookie = WebServer.Sessions.Cookie;

namespace WebServer
{
    internal class WebServerContext : IWebServerContext
    {
        #region Private Variables

        private readonly Dictionary<string, Cookie> _cookies;

        #endregion

        #region Automatic Properties

        public Guid Id { get; }
        public HttpListenerContext HttpContext { get; }
        public SessionManager SessionManager { get; }
        public Session Session { get; }
        public IReadOnlyDictionary<string, Cookie> Cookies => _cookies;

        #endregion

        #region Constructors

        /// <summary>
        /// WebServerContext constructor.
        /// </summary>
        /// <param name="listener">http listner</param>
        /// <param name="sessionManager">session manager</param>
        internal WebServerContext(HttpListenerContext listener, SessionManager sessionManager)
        {
            Id = Guid.NewGuid();
            HttpContext = listener;
            SessionManager = sessionManager;
            _cookies = PopulateCookies(listener);
            Session = GetSession();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a cookie to the context.
        /// </summary>
        /// <param name="name">cookie name</param>
        /// <returns>existing cookie or adds new cookie.</returns>
        public Cookie AddCookie(string name)
        {
            return _cookies.GetOrAdd(name, new Cookie(name));
        }

        /// <summary>
        /// Gets a cookie from the context.
        /// </summary>
        /// <param name="name">cookie name</param>
        /// <returns>cookie or null</returns>
        public Cookie GetCookie(string name)
        {
            Cookie cookie;
            _cookies.TryGetValue(name, out cookie);
            return cookie;
        }

        /// <summary>
        /// Sets a cookie.
        /// </summary>
        /// <param name="cookie">cookie</param>
        public void SetCookie(Cookie cookie)
        {
            _cookies.AddOrUpdate(cookie.Name, cookie, (k, v) => cookie);
        }


        /// <summary>
        /// Return the remote endpoint IP address. 
        /// </summary> 
        public IPAddress GetEndpointAddress()
        {
            return HttpContext.Request.RemoteEndPoint?.Address;
        }

        /// <summary> 
        /// Returns the request header collection.
        /// </summary>
        public IReadOnlyDictionary<string, string> GetRequestHeaders()
        {
            var headers = HttpContext.Request.Headers;
            return headers.AllKeys.ToDictionary(key => key, key => headers[key]);
        }

        /// <summary> 
        /// Returns the verb of the request: GET, POST, PUT, DELETE, and so forth.
        /// </summary>
        public string GetRequestVerb()
        {
            return HttpContext.Request.HttpMethod.ToUpper();
        }

        /// <summary> 
        /// Returns a dictionary of the parameters on the URL.
        /// </summary>
        public IReadOnlyDictionary<string, string> GetQueryParameters()
        {
            return HttpUtility.ParseQueryString(HttpContext.Request.Url.Query);
        }

        /// <summary>
        /// Returns the session object. 
        /// </summary>
        /// <returns>session object</returns>
        public Session GetSession()
        {
            var sessionCookie = HttpContext.Request.Cookies[Session.CookieName];
            dynamic session = sessionCookie?.Value == null ? SessionManager.CreateSession() : SessionManager.GetSession(new Guid(sessionCookie.Value));
            session.EndPoint = HttpContext.Request.LocalEndPoint;
            return session;
        }

        /// <summary> 
        /// Sets a request header.
        /// </summary>
        public void SetRequestHeader(string key, string value)
        {
            HttpContext.Request.Headers[key] = value;
        }

        /// <summary>
        /// Send response text.
        /// </summary>
        /// <param name="responseText"></param>
        public void SendResponseText(string responseText)
        {
            // Set the response cookie.
            SetResponseCookie();

            // Setup response.
            var response = new WebResponse(responseText);
            HttpContext.Response.ContentType = response.ContentType;
            HttpContext.Response.ContentEncoding = response.ContentEncoding;
            HttpContext.Response.ContentLength64 = response.ResponseData.Length;
            HttpContext.Response.OutputStream.Write(response.ResponseData, 0, response.ResponseData.Length);
            HttpContext.Response.StatusCode = response.Status;

            // Send the response.
            HttpContext.Response.OutputStream.Close();
        }

        #endregion

        #region Helpers

        private void SetResponseCookie()
        {
            // Set response cookie
            var cookie = HttpContext.Response.Cookies[Session.CookieName];
            if (cookie != null)
            {
                cookie.Value = Session.Id.ToString();
            }
            else
            {
                HttpContext.Response.Cookies.Add(new System.Net.Cookie(Session.CookieName, Session.Id.ToString()));
            }
        }

        private static Dictionary<string, Cookie> PopulateCookies(HttpListenerContext listener)
        {
            return listener.Request.Cookies.OfType<System.Net.Cookie>().Select(x => new Cookie(x)).ToDictionary(x => x.Name);
        }

        #endregion
    }
}
