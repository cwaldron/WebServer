using System;

namespace WebServer.Workflow
{
    /// <summary>
    /// Workflow exception.
    /// </summary>
    public class WorkflowException : Exception
    {
        public WorkflowException(string message)
            : this(message, 0)
        {

        }

        public WorkflowException(string message, int status)
        {
            Status = status;
        }

        public int Status { get; set; }
    }

    /// <summary>
    /// Workflow abort exception.
    /// </summary>
    public class WorkflowAbortException : WorkflowException
    {
        public WorkflowAbortException(string message)
            : this(message, 0)
        {

        }

        public WorkflowAbortException(string message, int status)
            : base(message, status)
        {

        }
    }
}
