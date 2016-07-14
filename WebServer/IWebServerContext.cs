using System;
using System.Net;
using WebServer.Workflow;

namespace WebServer
{
    public interface IWebServerContext
    {
        Guid Id { get; }

        HttpListenerContext HttpContext { get; }

        void SendResponseText(string repsonse);
    }
}
