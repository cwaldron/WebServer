using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebServer.Workflow;

namespace WebServer.Queueing
{
    internal class RequestManager<T> : IDisposable
    {
        #region Private Members

        private readonly RequestQueue<IWorkflowContext<T>> _loadBalancer;
        private readonly RequestThreadPool<IWorkflowContext<T>> _requestThreadPool;
        private bool _disposed;

        private readonly IRequestProcessor<T> _requestProcessor;

        #endregion

        #region Constructors

        /// <summary>
        /// RequestManager constructor.
        /// </summary>
        /// <param name="requestProcessor">request processor</param>
        public RequestManager(IRequestProcessor<T> requestProcessor)
        {
            if (requestProcessor == null)
            {
                throw new ArgumentNullException(nameof(requestProcessor));
            }

            _requestProcessor = requestProcessor;
            _loadBalancer = new RequestQueue<IWorkflowContext<T>>();
            _requestThreadPool = new RequestThreadPool<IWorkflowContext<T>>();
        }

        #endregion

        #region Methods

        public void Start()
        {
            // Start the request threadpool
            _requestThreadPool.Start(x => {

                while (!_disposed)
                {
                    IWorkflowContext<T> context = x.Dequeue();
                    if (!context.Equals(default(T)))
                    {
                        // Execute context through the workflow.
                        var workflow = _requestProcessor.GetWorkflow();
                        workflow.Run(context);
                    }
                }
            });

            // Start the request monitor.
            Task.Run(() =>
            {
                // Forever...
                while (!_disposed)
                {
                    // Wait until we have received a context.
                    IWorkflowContext<T> context = _loadBalancer.Dequeue();

                    // Post the request to the threadpool.
                    _requestThreadPool.PostRequest(context);
                }
            });
        }

        /// <summary>
        /// Process the web context.
        /// </summary>
        /// <param name="request"></param>
        public void Process(T request)
        {
            _loadBalancer.Enqueue(new WorkflowContext<T>(request, this));
        }

        #endregion

        #region IDisposible

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            Dispose(!_disposed);
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose concurrent queue.
        /// </summary>
        /// <param name="disposing">indicate whether the queue is disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            _requestThreadPool.Dispose();
            _loadBalancer.Dispose();
        }

        #endregion
    }
}
