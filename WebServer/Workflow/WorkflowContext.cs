using System;
using WebServer.Queueing;

namespace WebServer.Workflow
{
    /// <summary>
    /// Workflow context.
    /// </summary>
    /// <typeparam name="T">token type</typeparam>
    internal class WorkflowContext<T> : IWorkflowContext<T>
    {
        #region Private Members

        private readonly RequestManager<T> _requestManager;

        #endregion

        #region Constructors

        internal WorkflowContext(T token, RequestManager<T> requestManager)
        {
            Id = Guid.NewGuid();
            Token = token;
            _requestManager = requestManager;
        }

        #endregion

        #region Properties

        public Guid Id { get; }

        public WorkflowState State { get; internal set; }

        public T Token { get; private set; }

        public int Step { get; internal set; }

        #endregion
    }
}
