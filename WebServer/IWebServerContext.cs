using System;
using System.Net;
using WebServer.Workflow;

namespace WebServer
{
    public interface IWebServerContext
    {
        Guid Id { get; }

        HttpListenerContext WebContext { get; }

        void SendResponseText(string repsonse);
    }
}
