using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using WebServer.Sessions;
using WebServer.Utilities;

namespace WebServer
{
    internal class WebServerContext : IWebServerContext
    {
        #region Automatic Properties

        public Guid Id { get; }
        public HttpListenerContext HttpContext { get; }
        public SessionManager SessionManager { get; }
        public Session Session { get; }

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
            Session = GetSession();
        }

        #endregion

        #region Methods

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
            var sessionCookie = HttpContext.Request.Cookies[SessionManager.SessionCookieKey];
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
            var cookie = HttpContext.Response.Cookies[SessionManager.SessionCookieKey];
            if (cookie != null)
            {
                cookie.Value = Session.Id.ToString();
            }
            else
            {
                HttpContext.Response.Cookies.Add(new Cookie(SessionManager.SessionCookieKey, Session.Id.ToString()));
            }
        }

        #endregion
    }
}
