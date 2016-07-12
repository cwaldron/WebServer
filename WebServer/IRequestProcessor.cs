using System.Net;
using WebServer.Workflow;

namespace WebServer
{
    public interface IRequestProcessor<T>
    {
        Workflow<T> GetWorkflow();
    }
}
