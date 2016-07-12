using System;
using System.Net;
using System.Text;

namespace WebServer
{
    internal class WebServerContext : IWebServerContext
    {
        #region Automatic Properties

        public Guid Id { get; }
        public HttpListenerContext WebContext { get; }

        #endregion

        #region Constructors

        internal WebServerContext(HttpListenerContext listener)
        {
            Id = Guid.NewGuid();
            WebContext = listener;
        }

        #endregion

        #region Methods

        public void SendResponseText(string responseText)
        {
            // Setup response.
            var response = new WebResponse(responseText);
            WebContext.Response.ContentType = response.ContentType;
            WebContext.Response.ContentEncoding = response.ContentEncoding;
            WebContext.Response.ContentLength64 = response.ResponseData.Length;
            WebContext.Response.OutputStream.Write(response.ResponseData, 0, response.ResponseData.Length);
            WebContext.Response.StatusCode = response.Status;

            // Send the response.
            WebContext.Response.OutputStream.Close();
        }

        #endregion
    }
}
