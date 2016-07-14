using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Workflow
{
    public class Activity<T>
    {
        #region Private Members

        private readonly Func<IWorkflowContext<T>, WorkflowState> _operation;

        #endregion

        #region Constructors

        /// <summary>
        /// Protected constructor.
        /// </summary>
        protected Activity()
        {
        }

        /// <summary>
        /// Activity constructor.
        /// </summary>
        /// <param name="operation">activity operation</param>
        internal Activity(Func<IWorkflowContext<T>, WorkflowState> operation)
        {
            _operation = operation;
        }

        #endregion

        #region Properties

        public IWorkflowContext<T> Context { get; protected set; }

        public T Token { get; protected set; }

        #endregion

        #region Methods

        /// <summary>
        /// Run activity.
        /// </summary>
        /// <param name="context">context</param>
        internal virtual void Run(IWorkflowContext<T> context)
        {
            ((WorkflowContext<T>)context).State = _operation(context);
        }

        #endregion
    }
}
