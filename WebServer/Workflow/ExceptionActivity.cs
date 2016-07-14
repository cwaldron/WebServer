using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Workflow
{
    internal class ExceptionActivity<T> : Activity<T>
    {
        #region Private Members

        private readonly Action<IWorkflowContext<T>, Exception> _operation;

        #endregion

        #region Constructors

        /// <summary>
        /// Activity constructor.
        /// </summary>
        /// <param name="operation">activity operation</param>
        internal ExceptionActivity(Action<IWorkflowContext<T>, Exception> operation)
        {
            _operation = operation;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Run activity.
        /// </summary>
        /// <param name="context">context</param>
        internal void Run(IWorkflowContext<T> context, Exception ex)
        {
            _operation(context, ex);
        }

        #endregion
    }
}
