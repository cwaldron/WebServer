using System;
using System.Collections.Generic;
using System.Linq;

namespace WebServer.Workflow
{
    /// <summary>
    /// A coherent progression of server activities
    /// </summary>
    /// <typeparam name="T">type of workflow token</typeparam>
    public class Workflow<T> : Activity<T>
    {
        #region Private Members

        private readonly List<Activity<T>> _activities;
        private ExceptionActivity<T> _abort;
        private ExceptionActivity<T> _exception;

        #endregion

        #region Automatic Properties

        internal bool HasAbortHandler
        {
            get { return _abort != null; }
        }

        internal bool HasExceptionHander
        {
            get { return _exception != null; }
        }

        #endregion

        #region Constructors

        public Workflow()
        {
            _activities = new List<Activity<T>>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Do work.
        /// </summary>
        /// <param name="action">action operator</param>
        /// <returns>workflow</returns>
        public Workflow<T> Do(Action<IWorkflowContext<T>> action)
        {
            _activities.Add(new Activity<T>(action));
            return this;
        }

        /// <summary>
        /// Do work.
        /// </summary>
        /// <param name="action">action operator</param>
        /// <returns>workflow</returns>
        public Workflow<T> OnAbort(Action<IWorkflowContext<T>, Exception> action)
        {
            _abort = new ExceptionActivity<T>(action);
            return this;
        }

        /// <summary>
        /// Do work.
        /// </summary>
        /// <param name="action">action operator</param>
        /// <returns>workflow</returns>
        public Workflow<T> OnException(Action<IWorkflowContext<T>, Exception> action)
        {
            _exception = new ExceptionActivity<T>(action);
            return this;
        }

        /// <summary>
        /// Run the workflow.
        /// </summary>
        /// <param name="context"></param>
        internal override void Run (IWorkflowContext<T> context)
        {
            var workflowContext = (WorkflowContext<T>)context;

            try
            {
                while (context.Step < _activities.Count && context.State != WorkflowState.Done)
                {
                    _activities[workflowContext.Step++].Run(context);
                }
            }
            catch (WorkflowAbortException ex)
            {
                _abort.Run(context, ex);
                workflowContext.State = WorkflowState.Done;
            }
            catch (Exception ex)
            {
                _exception.Run(context, ex);
                workflowContext.State = WorkflowState.Done;
            }
        }

        #endregion
    }
}
