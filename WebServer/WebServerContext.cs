using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using WebServer.Utilities;

namespace WebServer
{
    internal class WebServerContext : IWebServerContext
    {
        #region Automatic Properties

        public Guid Id { get; }
        public HttpListenerContext HttpContext { get; }

        #endregion

        #region Constructors
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listener"></param>
        internal WebServerContext(HttpListenerContext listener)
        {
            Id = Guid.NewGuid();
            HttpContext = listener;
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
    }
}
