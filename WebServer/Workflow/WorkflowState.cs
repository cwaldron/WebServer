namespace WebServer.Workflow
{
    /// <summary>
    /// Workflow State
    /// </summary>
    public enum WorkflowState
    {
        /// <summary>
        /// Terminate execution of the workflow.
        /// </summary>
        Abort,

        /// <summary>
        /// Continue with the execution of the workflow.
        /// </summary>
        Continue,

        /// <summary>
        /// The workflow should terminate without error.  The workflow step
        /// is indicating that it has handled the request and there is no further
        /// need for downstream processing.
        /// </summary>
        Finish
    }
}
