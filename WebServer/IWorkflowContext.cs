using System;
using WebServer.Workflow;

namespace WebServer
{
    public interface IWorkflowContext<out T>
    {
        Guid Id { get; }

        WorkflowState State { get; }

        T Token { get; }

        int Step { get; }
    }
}
