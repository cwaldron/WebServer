using System;
using WebServer.Workflow;

namespace WebServer
{
    public interface IWorkflowContext<T>
    {
        Guid Id { get; }

        WorkflowState State { get; }

        T Token { get; }

        int Step { get; }
    }
}
