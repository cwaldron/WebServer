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
        private Activity<T> _abort;
        private Activity<T> _exception;

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
        /// <param name="lambda">function operator</param>
        /// <returns>workflow</returns>
        public Workflow<T> Do(Func<IWorkflowContext<T>, WorkflowState> lambda)
        {
            _activities.Add(new Activity<T>(lambda));
            return this;
        }

        /// <summary>
        /// Do work.
        /// </summary>
        /// <param name="lambda">function operator</param>
        /// <returns>workflow</returns>
        public Workflow<T> OnAbort(Func<IWorkflowContext<T>, WorkflowState> lambda)
        {
            _abort = new Activity<T>(lambda);
            return this;
        }

        /// <summary>
        /// Do work.
        /// </summary>
        /// <param name="lambda">function operator</param>
        /// <returns>workflow</returns>
        public Workflow<T> OnException(Func<IWorkflowContext<T>, WorkflowState> lambda)
        {
            _exception = new Activity<T>(lambda);
            return this;
        }

        /// <summary>
        /// Run the workflow.
        /// </summary>
        /// <param name="context"></param>
        internal override void Run (IWorkflowContext<T> context)
        {
            while (context.Step < _activities.Count)
            {
                _activities[((WorkflowContext<T>)context).Step++].Run(context);
                if (context.State == WorkflowState.Abort || context.State == WorkflowState.Finish)
                    break;
            }
        }

        #endregion
    }
}
