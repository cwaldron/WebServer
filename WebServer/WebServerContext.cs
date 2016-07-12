using System;
using System.Net;
using System.Text;

namespace WebServer
{
    internal class WebServerContext : IWebServerContext
    {
        #region Automatic Properties

        public Guid Id { get; }
        public HttpListenerContext HttpContext { get; }

        #endregion

        #region Constructors

        internal WebServerContext(HttpListenerContext context)
        {
            Id = Guid.NewGuid();
            HttpContext = context;
        }

        #endregion

        #region Methods

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
