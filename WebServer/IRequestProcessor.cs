using System.Net;
using WebServer.Application;
using WebServer.Workflow;

using System;

namespace WebServer
{
    public interface IRequestProcessor<T> : IDisposable
    {
        IApplication Application { get; }

        Workflow<T> Workflow { get; }
    }
}
